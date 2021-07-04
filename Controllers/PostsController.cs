using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ForumApp.Models;
using ForumApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using X.PagedList;

namespace ForumApp.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private ForumAppContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHostEnvironment _environment;
        private readonly FileUploadService _uploadService;

        public PostsController(ForumAppContext db, UserManager<User> userManager, IHostEnvironment environment, FileUploadService uploadService)
        {
            _db = db;
            _userManager = userManager;
            _environment = environment;
            _uploadService = uploadService;
        }


        public IActionResult Index(string searchByName, int? page)
        {
            List<Post> posts = _db.Posts.ToList();

            if (searchByName != null)
            {
                posts = posts.Where(s => s.User.UserName.ToLower().Contains(searchByName.ToLower())).ToList();
                ViewBag.searchByName = searchByName;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(posts.OrderByDescending(p => p.DatePublish).ToPagedList(pageNumber, pageSize));
        }


        public IActionResult Create()
        {
            string userId = _userManager.GetUserId(User);
            Post post = new Post(){UserId =  userId};
            return View(post);
        }
        
        [HttpPost]
        public IActionResult Create(Post post, string userId)
        {
            if (post != null && userId != null)
            {
                User user = _userManager.FindByIdAsync(userId).Result;
                if (user == null)
                    return NotFound("Пользователь не найден");
                
                if (ModelState.IsValid)
                {
                    string rootDirName = "wwwroot";
                    DirectoryInfo dirInfo = new DirectoryInfo(rootDirName);
                    foreach (var dir in dirInfo.GetDirectories())
                    {
                        if (!Directory.Exists("Images"))
                            dirInfo.CreateSubdirectory("Images");
                    }
                    
                    rootDirName = String.Concat(rootDirName, "\\Images\\");
                    string rootDirPath = Path.Combine(_environment.ContentRootPath, rootDirName);

                    if (post.File != null)
                    {
                        post.Path = $"/Images/{post.File.FileName}";
                        _uploadService.Upload(rootDirPath, post.File.FileName, post.File);
                    }
                    
                    post.UserId = userId;
                    _db.Posts.Add(post);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Posts");
                }    
            }
            return View(post);
        }
        
        
    }
}