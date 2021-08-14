using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QAProject2.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Required]
        [MaxLength(1000, ErrorMessage = "Description can't be more than 10 letters long")]
        [MinLength(1, ErrorMessage = "Description can't be shorter than 3 letters long")]
        [AllowHtml]
        public string Content { get; set; }
        public int Vote { get; set; }
        public bool Mark { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public Answer()
        {
            Comments = new HashSet<Comment>();
        }
    }
}