using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ForumApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,12}", ErrorMessage = "Пароль должен содержать минимум одну букву в верхнем регистре, одну - в нижнем и одну цифру, длина 6-12 знаков")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно")]
        [Display(Name = "Введите пароль повторно")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }


    }
}