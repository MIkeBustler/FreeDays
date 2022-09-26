using System.ComponentModel.DataAnnotations;
namespace FreeDOW.API.WebHost.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Отсутствует имя пользователя")]
        public string? UserName { get; set; }
        [Required(ErrorMessage ="Отсутсвует пароль")]
        public string? Password { get; set; }
    }

    public class RegisterModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "отсутствует логин")]
        public string? Login { get; set; }
        [Required(ErrorMessage = "отсутствует пароль"), MinLength(6, ErrorMessage = "минимальная длина пароля 6 символов")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Отсутствует адрес электронной почты ")]
        public string? Email { get; set; }
    }
}
