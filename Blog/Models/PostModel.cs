namespace Blog.Models
{
    using Entities;
    using System.Collections.Generic;

    public class PostModel
    {
        public List<Posts> Posts { get; set; }
        public bool IsWriter { get; set; }
    }
}