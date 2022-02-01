using System.ComponentModel.DataAnnotations;

namespace TesteBackendEnContact.Core.Domain.DTOs
{
    public class AddUserAuthDTO
    {
        public AddUserAuthDTO(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        [Required(ErrorMessage = "Um user name é obrigatório.")]
        public string UserName { get; private set; }
        [Required(ErrorMessage = "Uma senha deve ser informada.")]
        public string Password { get; private set; }
    }
}
