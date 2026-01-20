namespace Domain.Entities
{
    public class Country
    {
        public int CountryID { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        private Country() { }

        public Country(string name, string createdBy, string modifiedBy)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("El nombre del país no puede ser nulo o vacío.");
            }

            Name = name;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
