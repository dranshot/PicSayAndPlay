using PicSayAndPlay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PicSayAndPlay.Services
{
    public class PicSayAndPlayService
    {
        private static string baseUri = "http://picsayandplayapiservice.azurewebsites.net/api/";

        public static async Task<User> LoginUser(string username, string password)
        {
            return await BaseApiEndpoint.GetApiAsync<User>($"{baseUri}User?username={username}&password={password}");
        }

        public static async Task<bool> RegisterUser(UserToRegister user)
        {
            return await BaseApiEndpoint.PostApiAsync<bool>($"{baseUri}User", user);
        }
    }
}
