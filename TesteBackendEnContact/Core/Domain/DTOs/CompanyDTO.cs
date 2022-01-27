using System.ComponentModel.DataAnnotations;

namespace TesteBackendEnContact.Core.Domain.DTOs
{
    public class CompanyDTO
    {
        public CompanyDTO(int id, int contactBookId, string name)
        {
            Id = id;
            ContactBookId = contactBookId;
            Name = name;
        }

        public int Id { get; private set; }
        [Required]
        public int ContactBookId { get; private set; }
        [Required]
        [StringLength(50)]
        public string Name { get; private set; }
    }
}
