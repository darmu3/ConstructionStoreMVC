using LinqToDB.Mapping;
using System.ComponentModel.DataAnnotations;

namespace applicationmvc.Models
{
    [Table(Name = "User")]
    public class User
    {
        [PrimaryKey, Identity]
        public int UserId { get; set; }

        [Column]
        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = "Поле Имя пользователя обязательно для заполнения.")]
        public string UserName { get; set; }

        [Column]
        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Поле Номер телефона обязательно для заполнения.")]
        public string PhoneNumber { get; set; }

        [Column]
        [Display(Name = "Email пользователя")]
        [Required(ErrorMessage = "Поле Email пользователя обязательно для заполнения.")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email.")]
        public string UserEmail { get; set; }

        [Column]
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Поле Пароль обязательно для заполнения.")]
        public string Password { get; set; }
    }
}
