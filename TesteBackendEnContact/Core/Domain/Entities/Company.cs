using Dapper.Contrib.Extensions;

namespace TesteBackendEnContact.Core.Domain.Entities
{
    [Table(nameof(Company))]
    public class Company
    {
        protected Company() { }
        public Company(int id, int contactBookId, string name)
        {
            Id = id;
            ContactBookId = contactBookId;
            Name = name;
        }

        public Company(int contactBookId, string name)
        {
            ContactBookId = contactBookId;
            Name = name;
        }

        [Key]
        public int Id { get; private set; }

        public int ContactBookId { get; private set; }

        public string Name { get; private set; }
    }
}
