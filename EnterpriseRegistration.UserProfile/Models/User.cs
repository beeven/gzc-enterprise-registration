using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.UserProfile.Models
{
    public class User
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string PersonId { get; set; }
        public string deptName { get; set; }
        public string CustomCode { get; set; }
        public List<Role> Roles { get; set; }
    }

    public class Role
    {
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string RoleId { get; set; }
    }

    public class RoleCollection
    {
        public List<Role> Roles { get; set; }
    }
}
