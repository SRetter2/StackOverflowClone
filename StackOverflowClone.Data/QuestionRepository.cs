using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace StackOverflowClone.Data
{
    public class QuestionRepository
    {
        private string _connectionString;
        public QuestionRepository(string conn)
        {
            _connectionString = conn;
        }

        public List<Question> GetAllQuestions()
        {
            using (var context = new QuestionContext(_connectionString))
            {
                return context.Questions.Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.Tag)
                    .Include(q => q.Likes)
                    .Include(q => q.Answers)
                    .OrderByDescending(q => q.DatePosted)
                    .ToList();
            }
        }
        public Question GetQuestionById(int id)
        {
            using (var context = new QuestionContext(_connectionString))
            {
                return context.Questions.Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.Tag)
                    .Include(q => q.Likes)
                    .ThenInclude(l => l.Question)
                    .Include(q => q.Likes)
                    .ThenInclude(l => l.User)
                    .Include(q => q.Answers)
                    .FirstOrDefault(q => q.Id == id);
            }
        }
        public void AddAnswer(Answer answer)
        {
            using (var context = new QuestionContext(_connectionString))
            {
                context.Answers.Add(answer);
                context.SaveChanges();
            }
        }
        public void LikeQuestion(Likes like)
        {
            using (var context = new QuestionContext(_connectionString))
            {
                context.Likes.Add(like);
                context.SaveChanges();
            }
        }
        public void AddQuestion(Question question, IEnumerable<string> tags)
        {
            using (var context = new QuestionContext(_connectionString))
            {
                context.Questions.Add(question);
                foreach (string name in tags)
                {
                    var tag = GetTag(name);
                    int tagId;
                    if (tag == null)
                    {
                        tagId = AddTag(name);
                    }
                    else
                    {
                        tagId = tag.Id;
                    }

                    context.QuestionTags.Add(new QuestionTag
                    {
                        QuestionId = question.Id,
                        TagId = tagId
                    });
                    
                }
                context.SaveChanges();
            }
        }
        private Tag GetTag(string name)
        {
            using (var context = new QuestionContext(_connectionString))
            {
                return context.Tags.FirstOrDefault(t => t.Name == name);
            }
        }
        private int AddTag(string name)
        {
            using (var context = new QuestionContext(_connectionString))
            {
                Tag tag = new Tag { Name = name };
                context.Tags.Add(tag);
                context.SaveChanges();
                return tag.Id;
            }
        }

    }
}
