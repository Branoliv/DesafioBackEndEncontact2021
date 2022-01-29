using Dapper.Contrib.Extensions;

namespace TesteBackendEnContact.Core.Domain.Entities
{
    [Table(nameof(Contact))]
    public class Contact
    {
        public Contact() { }

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

        [Write(false)]
        public Company Company { get; set; }

        [Write(false)]
        public ContactBook ContactBook { get; set; }

        public override string ToString() => $"{Id};{Name};{Phone};{Email};{CompanyId};{Company.Name};{ContactBookId};{ContactBook.Name};{Address}";

        public static implicit operator string(Contact contact)
            => $"{contact.Id};{contact.Name};{contact.Phone};{contact.Email};{(contact.CompanyId == 0 ? "" : contact.CompanyId)};" +
            $"{ (contact.Company == null ? "" : contact.Company.Name)};{(contact.ContactBookId == 0 ? "" : contact.ContactBookId)};" +
            $"{(contact.ContactBook == null ? "" : contact.ContactBook.Name)};{contact.Address}";

        public static implicit operator Contact(string line)
        {
            var data = line.Split(";");
            return new Contact(data[0], data[1], data[2], (data[3] == "" ? 0 : int.Parse(data[3])), (data[5] == "" ? 0 : int.Parse(data[5])), data[7]);
        }
    }
}
