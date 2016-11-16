using System;
using System.Linq;
using System.Web.Http;

namespace PicSayAndPlay.Service.Controllers
{
    public class UserController : ApiController
    {
        private picsayplaydbEntities db = new picsayplaydbEntities();

        [HttpGet]
        public Models.UserInformation GetLogin(string username, string password)
        {
            var query = db.SP_Login(username, password);
            var result = query.FirstOrDefault();

            if (result == null)
                return null;

            var user = new Models.UserInformation()
            {
                Birthday = result.Birthday,
                Email = result.Email,
                FirstName = result.FirstName,
                LastName = result.LastName,
                ID = result.IDUser,
                NickName = result.NickName,
                Country = result.Country,
                Level = result.Level,
                TotalPoints = result.TotalPoints
            };
            return user;
        }

        [HttpPost]
        public bool RegisterUser(Models.UserToRegister user)
        {
            var query = db.SP_RegisterUser(user.FirstName, user.LastName, user.NickName, user.Email, user.Password, user.Birthday.Date, user.Country);
            return true;
        }
    }
}