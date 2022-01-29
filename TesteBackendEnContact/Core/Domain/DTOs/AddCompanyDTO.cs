using System.ComponentModel.DataAnnotations;

namespace TesteBackendEnContact.Core.Domain.DTOs
{
    public class AddCompanyDTO
    {
        public AddCompanyDTO(string name)
        {
            Name = name;
        }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome da agenda deve conter entre 3 e 50 caracteres.")]
        public string Name { get; private set; }

        public static implicit operator AddCompanyDTO(string line)
        {
            var data = line.Split(";");
            return new AddCompanyDTO(data[4]);
        }
    }
}
