namespace Domain.Entities
{
    public class City
    {
        public int CityID { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }
        public ICollection<Rl_UserCity> UserCities { get; set; } = new List<Rl_UserCity>();

        private City() { }

        public City(string name, string createdBy, string modifiedBy)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("El nombre de la ciudad no puede ser nulo o vacío.");
            }

            Name = name;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
