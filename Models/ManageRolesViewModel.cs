using applicationmvc.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace applicationmvc.Models
{
    public class ManageRolesViewModel
    {
        public List<User> Users { get; set; }
        public List<Role> Roles { get; set; }
    }
}
