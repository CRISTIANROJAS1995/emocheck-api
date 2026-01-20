namespace Domain.Entities
{
    public class BusinessGroup
    {
        public int BusinessGroupID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsVisible { get; set; }

        private BusinessGroup() { }

        public BusinessGroup(string name, string createdBy, string modifiedBy, bool isVisible)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("El nombre del grupo empresarial no puede ser nulo o vacío.");
            }

            Name = name;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
            IsVisible = isVisible;
        }
    }
}
