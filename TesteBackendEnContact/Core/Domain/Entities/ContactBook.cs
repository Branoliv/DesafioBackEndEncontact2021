using Dapper.Contrib.Extensions;

namespace TesteBackendEnContact.Core.Domain.Entities
{
    [Table("ContactBook")]
    public class ContactBook
    {
        protected ContactBook() { }

        public ContactBook(string name)
        {
            Name = name;
        }

        public ContactBook(int id, string name)
        {
            Id = id;
            Name = name;
        }

        [Key]
        public int Id { get; private set; }
        public string Name { get; private set; }
    }
}
