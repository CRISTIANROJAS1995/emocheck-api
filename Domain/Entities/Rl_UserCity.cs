namespace Domain.Entities
{
    public class Rl_UserCity
    {
        public int UserID { get; set; }
        public User User { get; set; }

        public int CityID { get; set; }
        public City City { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        private Rl_UserCity() { }

        public Rl_UserCity(int userID, int cityID, string createdBy, string modifiedBy)
        {
            UserID = userID;
            CityID = cityID;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
