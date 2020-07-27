using System;

namespace User_Comments_API.DatabaseContext
{
    public class InMemoryData
    {
        public static void Add(ApiContext context)
        {
            var userOne = new DbModels.User
            {
                Id = Guid.NewGuid(),
                FirstName = "Bill",
                LastName = "Gates"
            };

            context.Users.Add(userOne);

            var postOne = new DbModels.Post
            {
                Id = Guid.NewGuid(),
                UserId = userOne.Id,
                Content = "This is my first comment."
            };

            context.Posts.Add(postOne);

            context.SaveChanges();
        }
    }
}
