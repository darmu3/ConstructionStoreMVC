using LinqToDB.Mapping;
using System.ComponentModel.DataAnnotations;

namespace applicationmvc.Models
{
    [Table(Name = "User")]
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [System.ComponentModel.DataAnnotations.DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }

}
