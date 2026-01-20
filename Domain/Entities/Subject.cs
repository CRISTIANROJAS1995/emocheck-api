namespace Domain.Entities
{
    public class Subject
    {
        public int SubjectID { get; set; }
        public string Identifier { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string IdentifierExternal { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        private Subject() { }

        public Subject(string identifier, int companyID, string name, string lastName, string email, string phone, string createdBy, string modifiedBy)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("El nombre del candidado no puede ser nulo o vacío.");
            }

            Identifier = identifier;
            CompanyID = companyID;
            Name = name;
            LastName = lastName;
            Email = email;
            Phone = phone;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
