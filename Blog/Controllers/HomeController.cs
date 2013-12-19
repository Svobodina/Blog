namespace Blog.Controllers
{
    using Entities;
    using Models;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private readonly BlogDBEntities _context = new BlogDBEntities();

        public ActionResult Index()
        {
            var user = GetUserByName(User.Identity.Name);
            var model = new PostModel
            {
                Posts = _context.Posts.ToList(),
                IsWriter = user != null && user.IsWriter
            };

            return View(model);
        }

        public ActionResult Post(int? id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            return View(post);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddComment(int post, string text)
        {
            var user = GetUserByName(User.Identity.Name);
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var comment = new Comments
            {
                AuthorId = user.Id,
                Created = DateTime.Now,
                PostId = post,
                Text = text
            };
            
            _context.Comments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("Post", new { id = post });
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddPost(string title, string text)
        {
            var user = GetUserByName(User.Identity.Name);
            if (user == null || !user.IsWriter)
            {
                throw new UnauthorizedAccessException();
            }

            var post = new Posts
            {
                Created = DateTime.Now,
                Text = text,
                Title = title,
                WriterId = user.Id
            };

            _context.Posts.Add(post);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private Users GetUserByName(string name)
        {
            return _context.Users.FirstOrDefault(u => u.Login == name);
        }
    }
}
