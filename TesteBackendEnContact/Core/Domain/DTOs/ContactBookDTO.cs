namespace TesteBackendEnContact.Core.Domain.DTOs
{
    public class ContactBookDTO
    {
        public ContactBookDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
    }
}
