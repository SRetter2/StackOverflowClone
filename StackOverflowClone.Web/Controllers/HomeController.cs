using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackOverflowClone.Data;
using StackOverflowClone.Web.Models;

namespace StackOverflowClone.Web.Controllers
{
    public class HomeController : Controller 
    {
        private readonly string _connectionString;
        public HomeController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConStr");
        }
        public IActionResult Index()
        {
            var repos = new QuestionRepository(_connectionString);
            return View(repos.GetAllQuestions());
        }
        public IActionResult ViewQuestion(int id)
        {
            var repos = new QuestionRepository(_connectionString);
            var repos2 = new AccountRepository(_connectionString);
            var vm = new QuestionViewModel();
            var question = repos.GetQuestionById(id);
            if (question == null)
            {
                return null;
            }
            vm.Question = question;
            vm.LoggedIn = User.Identity.IsAuthenticated;
            if (vm.LoggedIn)
            {
                var likedQuestionIds = HttpContext.Session.Get<List<int>>("likedquestionids");
                if (likedQuestionIds != null)
                {
                    vm.CanLikeQuestion = likedQuestionIds.All(i => i != id);
                }
                else
                {
                    vm.CanLikeQuestion = true;
                }
            }
            return View(vm);
        }
        [HttpPost]
        public IActionResult Answer(int UserId, string Text, int QuestionId)
        {
            var answer = new Answer
            {
                UserId = UserId,
                Text = Text,
                QuestionId = QuestionId
            };
            answer.DatePosted = DateTime.Now;           
            var repos = new QuestionRepository(_connectionString);
            repos.AddAnswer(answer);
            return Json(answer);
        }
        [HttpPost]
        public IActionResult LikeQuestion(int userId, int questionId)
        {
            var like = new Likes
            {
                UserId = userId,
                QuestionId = questionId
            };
            var repos = new QuestionRepository(_connectionString);
            repos.LikeQuestion(like);
            List<int> likedQuestionIds = HttpContext.Session.Get<List<int>>("likedquestionids") ?? new List<int>();
            likedQuestionIds.Add(like.QuestionId);
            HttpContext.Session.Set("likedids", likedQuestionIds);
            return Json(like);
        }
        [Authorize]
        public IActionResult NewQuestion()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddQuestion(Question question, string tag)
        {
            var repos = new QuestionRepository(_connectionString);
            var repos2 = new AccountRepository(_connectionString);
            var user = repos2.GetUserByEmail(User.Identity.Name);
           var tags = new List<string>();
            if(tag != null)
            {
              tags = tag.Split(',').ToList();
            }            
            question.UserId = user.Id;
            question.DatePosted = DateTime.Now;
            repos.AddQuestion(question, tags);
            return Redirect("/");
        }


    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
