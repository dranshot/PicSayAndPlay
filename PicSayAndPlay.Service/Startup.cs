﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PicSayAndPlay.Service.Startup))]

namespace PicSayAndPlay.Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}