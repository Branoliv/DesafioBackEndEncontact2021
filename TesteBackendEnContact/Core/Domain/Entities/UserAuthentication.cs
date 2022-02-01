using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteBackendEnContact.Core.Domain.Entities
{
    [Table("UserAuth")]
    public class UserAuthentication
    {
        protected UserAuthentication() { }
        public UserAuthentication(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public UserAuthentication(int id, string userName, string password)
        {
            Id = id;
            UserName = userName;
            Password = password;
        }

        [Key]
        public int Id { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
    }
}
