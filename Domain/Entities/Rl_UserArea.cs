namespace Domain.Entities
{
    public class Rl_UserArea
    {
        public int UserID { get; set; }
        public User User { get; set; }

        public int AreaID { get; set; }
        public Area Area { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        private Rl_UserArea() { }

        public Rl_UserArea(int userID, int areaID, string createdBy, string modifiedBy)
        {
            UserID = userID;
            AreaID = areaID;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
