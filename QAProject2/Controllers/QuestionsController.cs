using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QAProject2.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace QAProject2.Controllers
{
    public class QuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Questions
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? tagId)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.VoteSortParm = string.IsNullOrEmpty(sortOrder) ? "vote_desc" : "";
            ViewBag.AnswerSortParm = sortOrder == "Answer" ? "answer_desc" : "Answer";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var questions = from s in db.Questions select s;
            var tag = db.Tags.Find(tagId);
            if(tag != null)
            {
                questions = db.Questions.Where(q => q.Tags.Any(t => t.Id == tag.Id));
            }
            switch (sortOrder)
            {
                case "vote_desc":
                    questions = questions.OrderByDescending(q => q.Vote);
                    break;
                case "Answer":
                    questions = questions.OrderByDescending(q => q.Answers.Count);
                    break;
                case "answer_desc":
                    questions = questions.OrderBy(q => q.Answers.Count);
                    break;
                default:
                    questions = questions.OrderBy(q => q.Vote);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(questions.ToPagedList(pageNumber, pageSize));
        }

        // GET: Questions/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            Answer answer = new Answer();
            answer.QuestionId = (int) id;
            answer.Question = question;
            return View(answer);
        }
        [HttpPost]
        public ActionResult Details( Answer answer, int id)
        {
            Question question = db.Questions.Find(id);

            answer.QuestionId = id;
            answer.Date = DateTime.Now;
            answer.Mark = false;
            answer.Vote = 0;
            answer.UserId = User.Identity.GetUserId();
            question.Answers.Add(answer);
            db.Answers.Add(answer);
            db.SaveChanges();
            return View(answer);
        }
        // GET: Questions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description")] Question question)
        {
            if (ModelState.IsValid)
            {
                question.Date = DateTime.Now;
                question.Vote = 0;
                question.UserId = User.Identity.GetUserId();
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", question.UserId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Vote,Date,UserId")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", question.UserId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult AddTag(int? id)
        {
            var question = db.Questions.Find(id);
            if(question == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.TagId = new SelectList(db.Tags, "Id", "Name");

            return View();
        }
        [HttpPost]
        public ActionResult AddTag(int TagId, int id)
        {
            var question = db.Questions.Find(id);
            var tag = db.Tags.Find(TagId);
            ViewBag.TagId = new SelectList(db.Tags, "Id", "Name");

            question.Tags.Add(tag);
            tag.Questions.Add(question);
            db.SaveChanges();
            return View(question);
        }

        [HttpGet]
        public ActionResult UpVoteAnswer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpVoteAnswer( int? answerId)
        {
            var answer = db.Answers.Find(answerId);
            var user = User.Identity.GetUserId();
            bool check;
            if (answer != null)
            {
                if (answer.UserId != user)
                {
                    check = false;
                    answer.Vote += 1;
                    answer.ApplicationUser.Reputation += 5;
                }
                else
                {
                    check = true;
                }
                ViewBag.Check = check;
            }
            db.SaveChanges();
            return View();
        }

        [HttpGet]
        public ActionResult DownVoteAnswer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DownVoteAnswer(int? answerId)
        {
            var answer = db.Answers.Find(answerId);
            var user = User.Identity.GetUserId();
            bool check;
            if (answer != null)
            {
                if (answer.UserId != user)
                {
                    check = false;
                    answer.Vote -= 1;
                    answer.ApplicationUser.Reputation -= 5;
                }
                else
                {
                    check = true;
                }
                ViewBag.Check = check;
            }
            db.SaveChanges();
            return View();
        }

        [HttpGet]
        public ActionResult UpVoteQuestion()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpVoteQuestion(int? questionId)
        {
            var question = db.Questions.Find(questionId);
            var user = User.Identity.GetUserId();
            bool check;
            if (question != null)
            {
                if (question.UserId != user)
                {
                    check = false;
                    question.Vote += 1;
                    question.ApplicationUser.Reputation += 5;
                }
                else
                {
                    check = true;
                }
                ViewBag.Check = check;
            }
            db.SaveChanges();
            return View();
        }

        [HttpGet]
        public ActionResult DownVoteQuestion()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DownVoteQuestion(int? questionId)
        {
            var question = db.Questions.Find(questionId);
            var user = User.Identity.GetUserId();
            bool check ;
            if (question != null)
            {
                if (question.UserId != user)
                {
                    check = false;
                    question.Vote -= 1;
                    question.ApplicationUser.Reputation -= 5;
                }
                else
                {
                    check = true;
                }
                ViewBag.Check = check;
            }
            db.SaveChanges();
            return View();
        }

        [HttpGet]
        public ActionResult CheckAnswer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckAnswer(int? answerId)
        {
            var answer = db.Answers.Find(answerId);
            var user = User.Identity.GetUserId();
            if (answer != null)
            {
                if (answer.Question.ApplicationUser.Id == user)
                {
                    if (answer.Mark == false)
                    {
                        answer.Mark = true;
                    }
                    else
                    {
                        answer.Mark = false;
                    }
                }
                ViewBag.Mark = answer.Mark;
            }
            db.SaveChanges();
            return View();
        }
    }
}
