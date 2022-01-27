using Dapper.Contrib.Extensions;

namespace TesteBackendEnContact.Core.Domain.Entities
{
    [Table(nameof(Contact))]
    public class Contact
    {
        protected Contact() { }
        public Contact(string name, string phone, string email, int companyId, int contactBookId, string address)
        {
            Name = name;
            Phone = phone;
            Email = email;
            CompanyId = companyId;
            ContactBookId = contactBookId;
            Address = address;
        }

        public Contact(int id, string name, string phone, string email, int companyId, int contactBookId, string address)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Email = email;
            CompanyId = companyId;
            ContactBookId = contactBookId;
            Address = address;
        }

        [Key]
        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Phone { get; private set; }

        public string Email { get; private set; }

        public int CompanyId { get; private set; }

        public int ContactBookId { get; private set; }

        public string Address { get; private set; }
    }
}
