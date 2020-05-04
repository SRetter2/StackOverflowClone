using System;
using System.Collections.Generic;
using System.Text;

namespace StackOverflowClone.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }

        public int UserId { get; set; }
        public User user { get; set; }      
       

        public List<QuestionTag> QuestionTags { get; set; }
        public List<Answer> Answers { get; set; }
        public List<Likes> Likes { get; set; }
    }
}
