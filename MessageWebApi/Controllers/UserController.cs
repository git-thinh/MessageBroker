using System;
using System.Web.Http;

namespace MessageWebApi
{
    public class User
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateCreated { get; set; }

        public User()
        {
            DateCreated = DateTime.Now;
        }
    }

    public class UserController : ApiController
    {
        [HttpPost]
        public User Create([FromBody]User user)
        {
            return new User {Username = user.Username, FirstName = user.FirstName, LastName = user.LastName};
        }

    }
}