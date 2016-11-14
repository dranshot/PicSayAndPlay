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
        //public PicSayAndPlayService Instance { get { return new PicSayAndPlayService(); } }

        //protected override string BaseUri
        //{
        //    get { return "http://picsayandplayservice.azurewebsites.net/api/"; }
        //}

        //protected override HttpClient HttpClient
        //{
        //    get { return new HttpClient(); }
        //}
        private static string baseUri = "http://picsayandplayapiservice.azurewebsites.net/api/";

        public static async Task<User> LoginUser(string username, string password)
        {
            return await BaseApiEndpoint.GetApiAsync<User>($"{baseUri}User?username={username}&password={password}");
        }
    }
}
