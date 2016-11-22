using System;
using System.Collections.Generic;
using Android.Content;
using PicSayAndPlay.Droid;

namespace BlinkXamarinProject.AndroidApp.Helpers
{
    public class UserSessionManager
    {

        ISharedPreferences pref;
        ISharedPreferencesEditor editor;
        Context context;

        //Constants
        private const string PREFER_NAME = "XamarinPSPPref";
        private const string IS_USER_LOGIN = "IsUserLoggedIn";
        public const string KEY_NICKNAME = "firstname";
        public const string KEY_PASSWORD = "password";

        public bool IsUserLoggedIn { get { return pref.GetBoolean(IS_USER_LOGIN, false); } }

        // Constructor
        public UserSessionManager(Context context)
        {
            this.context = context;

            pref = this.context.GetSharedPreferences(PREFER_NAME, FileCreationMode.Private);
            editor = pref.Edit();
        }


        public void CreateUserLoginSession(string nickname, string password)
        {
            editor.PutBoolean(IS_USER_LOGIN, true);
            editor.PutString(KEY_NICKNAME, nickname);
            editor.PutString(KEY_PASSWORD, password);
            editor.Commit();
        }

        public bool CheckLogin()
        {
            if (!IsUserLoggedIn)
            {
                Intent i = new Intent(context, typeof(LoginActivity));
                i.AddFlags(ActivityFlags.ClearTop);
                i.SetFlags(ActivityFlags.NewTask);
                context.StartActivity(i);
                return true;
            }
            return false;
        }


        public Dictionary<string, string> GetUserDetails()
        {
            Dictionary<string, string> user = new Dictionary<string, string>();
            user[KEY_NICKNAME] = pref.GetString(KEY_NICKNAME, null);
            user[KEY_PASSWORD] = pref.GetString(KEY_PASSWORD, null);
            return user;
        }

        public void LogoutUser()
        {
            editor.Clear();
            editor.Commit();

            Intent i = new Intent(context, typeof(LoginActivity));
            i.AddFlags(ActivityFlags.ClearTop);
            i.SetFlags(ActivityFlags.NewTask);
            context.StartActivity(i);
        }

    }
}
