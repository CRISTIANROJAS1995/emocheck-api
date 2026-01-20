using System.ComponentModel.Design;

namespace Domain.Entities
{
    public class Area
    {
        public int AreaID { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; }

        public int StateID { get; set; }
        public State State { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsVisible { get; set; }
        public ICollection<Rl_UserArea> UserAreas { get; set; } = new List<Rl_UserArea>();

        private Area() { }

        public Area(int companyID, int stateID, string name, string createdBy, string modifiedBy, bool isVisible)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("El nombre del área no puede ser nulo o vacío.");
            }

            CompanyID = companyID;
            StateID = stateID;
            Name = name;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
            IsVisible = isVisible;
        }
    }
}
