using System;
using System.Collections.Generic;

namespace User_Comments_API.DbModels
{
    public class User
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<Post> Posts { get; set; }
    }
}
