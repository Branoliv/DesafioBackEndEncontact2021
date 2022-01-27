using System;
using System.ComponentModel.DataAnnotations;

namespace TesteBackendEnContact.Core.Domain.DTOs
{
    public class AddContactDTO
    {
        public AddContactDTO(string name, string phone, string email, int companyId, int contactBookId, string address)
        {
            Name = name;
            Phone = phone;
            Email = email;
            CompanyId = companyId;
            ContactBookId = contactBookId;
            Address = address;
        }


        [Required(ErrorMessage = "O nome do contato é obrigatório.")]
        public string Name { get; private set; }

        
        public string Phone { get; private set; }

        [Required(ErrorMessage = "Necessario informar um e-mail.")]
        [EmailAddress(ErrorMessage = "O formato de e-mail informado não corresponde ao padrão de e-mail.")]
        public string Email { get; private set; }

        public int CompanyId { get; private set; }

        [Required(ErrorMessage = "Necessario infomar um agenda.")]
        [Range(1, int.MaxValue, ErrorMessage = "Necessario informar uma agenda.")]
        public int ContactBookId { get; private set; }

        public string Address { get; private set; }
    }
}
