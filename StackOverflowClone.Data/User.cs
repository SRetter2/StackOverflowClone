using System;
using System.Collections.Generic;

namespace StackOverflowClone.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public List<Likes> Likes { get; set; }
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }

    }
}
