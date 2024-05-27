using Library.Models;

namespace Library.DTOs
{
    class LogInResponse
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public LogInResponse(string token, User user)
        {
            Token = token;
            Id = user.Id;
            Email = user.Email;
            Name = user.Name;
            Role = user.Role.ToString();
        }
    }

    class RegisteredResponse
    {
        public string Email { get; set; }
        public string Name { get; set; }

        public RegisteredResponse(RegistrationModel model)
        {
            Email = model.Email;
            Name = model.Name;
        }
    }
}