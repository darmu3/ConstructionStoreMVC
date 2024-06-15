using LinqToDB.Mapping;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace applicationmvc.Models
{
    [Table(Name = "Role")]
    public class Role
    {
        [PrimaryKey, Identity]
        public int RoleId { get; set; }

        [Column]
        [Display(Name = "Название роли")]
        [Required(ErrorMessage = "Поле Название роли обязательно для заполнения.")]
        public string RoleName { get; set; }

        // Связь с таблицей User
        public List<User> Users { get; set; }
    }
}
