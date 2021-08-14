using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QAProject2.Models
{
    public class Question
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40, ErrorMessage = "Title can't be more than 40 letters long")]
        [MinLength(1, ErrorMessage = "Title can't be shorter than 3 letters long")]
        public string Title { get; set; }
        [Required]
        [MaxLength(1000, ErrorMessage = "Description can't be more than 1000 letters long")]
        [MinLength(1, ErrorMessage = "Description can't be shorter than 3 letters long")]
        [AllowHtml]
        public string Description { get; set; }
        public int Vote { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public Question()
        {
            Answers = new HashSet<Answer>();
            Comments = new HashSet<Comment>();
            Tags = new HashSet<Tag>();
        }
    }
}