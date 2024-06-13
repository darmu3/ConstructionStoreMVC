using System.ComponentModel.DataAnnotations;

namespace applicationmvc.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Текущий пароль обязателен.")]
        [Display(Name ="Текущий пароль")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Новый пароль обязателен.")]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }
    }
}
