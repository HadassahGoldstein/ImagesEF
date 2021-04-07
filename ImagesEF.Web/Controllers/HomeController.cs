using ImagesEF.Data;
using ImagesEF.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImagesEF.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        private IWebHostEnvironment _environment;       
        public IActionResult Index()
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
            var vm = new ImagesViewModel()
            {
                Images = repo.GetImages()
            };
            return View(vm);
        }
        public IActionResult ViewImage(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
            var ids = HttpContext.Session.Get<List<int>>("Like-Ids");
            var vm = new ViewImageViewModel()
            {
                Image = repo.GetById(id)
            };
            if (ids != null)
            {
                vm.Liked=ids.Contains(id);
            }
            return View(vm);
        }
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(string title, IFormFile imageFile)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string fullPath = Path.Combine(_environment.WebRootPath, "uploads", fileName);
            using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                imageFile.CopyTo(stream);
            }           
            var image = new Image { Address = fileName, Title = title, DateUploaded = DateTime.Now };
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
            repo.AddImage(image);
            return Redirect("/");
        }
        public IActionResult CurrentLikes(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
            return Json(repo.GetLikesOfImage(id));
        }
        [HttpPost]
        public IActionResult AddLike(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
            repo.AddLike(id);
            var session = HttpContext.Session.Get<List<int>>("Like-Ids");
            if (session == null)
            {
                session = new List<int>();
            }
            session.Add(id);
            HttpContext.Session.Set("Like-Ids", session);
            return Json(id);
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
