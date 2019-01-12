using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemonade_Stand
{
    public class User
    {
        /// <summary>
        /// The username/ID of the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The password of the user (plain-text)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Whether the user has elevated privileges (e.g adding users, editing stock, etc..)
        /// </summary>
        public bool Administrator { get; set; }
    }
}
