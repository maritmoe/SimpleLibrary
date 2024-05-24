using Library.Models;

namespace Library.DTOs
{
    class LogInResponse
    {
        public string Token { get; set; }

        public LogInResponse(String token)
        {
            Token = token;
        }
    }

    class RegisteredResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public RegisteredResponse(RegistrationModel model, string id)
        {
            Id = id;
            Email = model.Email;
            Name = model.Name;
        }
    }
}