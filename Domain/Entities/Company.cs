namespace Domain.Entities
{
    public class Company
    {
        public int CompanyID { get; set; }

        public int BusinessGroupID { get; set; }
        public BusinessGroup BusinessGroup { get; set; }

        public int CountryID { get; set; }
        public Country Country { get; set; }

        public int StateID { get; set; }
        public State State { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Nit { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsVisible { get; set; }
        public ICollection<CompanyLicense> CompanyLicenses { get; set; }

        private Company() { }

        public Company(int businessGroupID, int countryID, int stateID, string name, string nit, string createdBy, string modifiedBy)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("El nombre de la empresa no puede ser nulo o vacío.");
            }

            if (string.IsNullOrEmpty(nit))
            {
                throw new ArgumentException("El NIT de la empresa no puede ser nulo o vacío.");
            }

            BusinessGroupID = businessGroupID;
            CountryID = countryID;
            StateID = stateID;
            Name = name;
            Nit = nit;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
