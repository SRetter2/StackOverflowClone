using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackOverflowClone.Data;

namespace StackOverflowClone.Web.Models
{
    public class QuestionViewModel
    {
        public Question Question { get; set; }
        public bool LoggedIn { get; set; }
        public bool CanLikeQuestion { get; set; }
    }
}
