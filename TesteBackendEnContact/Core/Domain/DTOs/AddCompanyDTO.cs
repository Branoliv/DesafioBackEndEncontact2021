using System.ComponentModel.DataAnnotations;

namespace TesteBackendEnContact.Core.Domain.DTOs
{
    public class AddCompanyDTO
    {
        public AddCompanyDTO(int contactBookId, string name)
        {
            ContactBookId = contactBookId;
            Name = name;
        }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A agenda é obrigatória.")]
        public int ContactBookId { get; private set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome da agenda deve conter entre 3 e 50 caracteres.")]
        public string Name { get; private set; }
    }
}
