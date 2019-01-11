using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemonade_Stand
{
    class UserManager
    {
        /// <summary>
        /// An object containing the logged in user.
        /// </summary>
        public User CurrentUser { get; private set; }

        /// <summary>
        /// A boolean to show whether a user is logged in or not.
        /// </summary>
        public bool IsLoggedIn => CurrentUser != null;

        public UserManager()
        {
            
        }

        /// <summary>
        /// Login using worker credentials.
        /// </summary>
        /// <param name="username">The username or ID of the worker</param>
        /// <param name="password">The password of the worker</param>
        /// <returns>A boolean to show whether the login was successful</returns>
        public bool Login(string username, string password)
        {
            // TODO: check username and password against database

            return username == "a" && password == "a";
        }

        /// <summary>
        /// Logout of the worker's account.
        /// </summary>
        /// <returns>A boolean to show whether the logout was successful</returns>
        public bool Logout()
        {
            if (!IsLoggedIn)
            {
                return false;
            }

            CurrentUser = null;
            return true;
        }
    }
}
