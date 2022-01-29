using System.ComponentModel.DataAnnotations;

namespace TesteBackendEnContact.Core.Domain.DTOs
{
    public class CompanyDTO
    {
        public CompanyDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }

        [Required]
        [StringLength(50)]
        public string Name { get; private set; }
    }
}
