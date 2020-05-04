using System;
using System.Collections.Generic;
using System.Text;

namespace StackOverflowClone.Data
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<QuestionTag> QuestionTags { get; set; }
    }
}
