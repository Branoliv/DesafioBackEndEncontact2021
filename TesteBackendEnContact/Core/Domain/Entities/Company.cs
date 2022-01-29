using Dapper.Contrib.Extensions;

namespace TesteBackendEnContact.Core.Domain.Entities
{
    [Table(nameof(Company))]
    public class Company
    {
        protected Company() { }
        public Company(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Company(string name)
        {
            Name = name;
        }

        [Key]
        public int Id { get; private set; }

        public string Name { get; private set; }

        public static implicit operator Company(string line)
        {
            var data = line.Split(";");
            return new Company(data[4]);
        }
    }
}
