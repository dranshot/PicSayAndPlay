using System;
using System.Linq;
using System.Web.Http;

namespace PicSayAndPlay.Service.Controllers
{
    public class UserController : ApiController
    {
        private picsayplaydbEntities db = new picsayplaydbEntities();

        [HttpGet]
        public Models.User GetLogin(string username, string password)
        {
            var query = db.SP_Login(username, password);
            var result = query.FirstOrDefault();

            if (result == null)
                return null;

            var user = new Models.User()
            {
                Birthday = result.Birthday.HasValue ? result.Birthday.Value : new DateTime(),
                Email = result.Email,
                FirstName = result.FirstName,
                LastName = result.LastName,
                ID = result.UserId,
                NickName = result.NickName,
                Country = new Models.Country() { Name = result.Country },
                Level = new Models.Level() { Value = result.Level }
            };
            return user;
        }
    }
}