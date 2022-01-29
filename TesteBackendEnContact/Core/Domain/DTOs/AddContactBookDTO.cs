using System.ComponentModel.DataAnnotations;

namespace TesteBackendEnContact.Core.Domain.DTOs
{
    public class AddContactBookDTO
    {
        public AddContactBookDTO(string name)
        {
            Name = name;
        }

        [Required(ErrorMessage = "O nome da agenda é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome da agenda deve conter entre 3 e 50 caracteres.")]
        public string Name { get; private set; }

        public static implicit operator AddContactBookDTO(string line)
        {
            var data = line.Split(";");
            return new AddContactBookDTO(data[6]);
        }
    }
}
