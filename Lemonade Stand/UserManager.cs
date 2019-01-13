using System.Linq;

namespace Lemonade_Stand
{
    public class UserManager
    {
        /// <summary>
        ///     The data manager object to use for storing/retrieving data
        /// </summary>
        private readonly DataManager _dataManager;

        public UserManager(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        /// <summary>
        ///     An object containing the logged in user.
        /// </summary>
        public User CurrentUser { get; private set; }

        /// <summary>
        ///     A boolean to show whether a user is logged in or not.
        /// </summary>
        public bool IsLoggedIn => CurrentUser != null;

        /// <summary>
        ///     Login using worker credentials.
        /// </summary>
        /// <param name="id">The username or ID of the worker</param>
        /// <param name="password">The password of the worker</param>
        /// <returns>A boolean to show whether the login was successful</returns>
        public bool Login(int id, string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            var user = _dataManager.Users.FirstOrDefault(u => u.Id == id);

            if (user == null) return false;

            if (user.Password != password) return false;

            CurrentUser = user;
            return true;
        }

        /// <summary>
        ///     Logout of the worker's account.
        /// </summary>
        /// <returns>A boolean to show whether the logout was successful</returns>
        public bool Logout()
        {
            if (!IsLoggedIn) return false;

            CurrentUser = null;
            return true;
        }

        /// <summary>
        ///     Create a new user in the database
        /// </summary>
        /// <param name="password">The password for the worker</param>
        /// <returns>A generated worker ID</returns>
        public int AddUser(string password)
        {
            var entity = _dataManager.Users.Add(new User
            {
                Password = password
            });

            _dataManager.SaveChanges();

            return entity.Entity.Id;
        }
    }
}