using Library.Enums;
using Library.Models;

public interface IAuthService
{
    Task<(int, string)> Registeration(RegistrationModel model, Role role);
    Task<(int, string)> Login(LoginModel model);
}