using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QAProject2.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [MaxLength(300, ErrorMessage = "Description can't be more than 1000 letters long")]
        [MinLength(1, ErrorMessage = "Description can't be shorter than 3 letters long")]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int? QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public int? AnswerId { get; set; }
        public virtual Answer Answer { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}