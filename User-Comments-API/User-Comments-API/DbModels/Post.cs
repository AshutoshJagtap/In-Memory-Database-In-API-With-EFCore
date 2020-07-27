using System;

namespace User_Comments_API.DbModels
{
    public class Post
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public string Content { get; set; }
    }
}
