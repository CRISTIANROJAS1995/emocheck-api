namespace Domain.Entities
{
    public class Rl_UserRole
    {
        public int UserID { get; set; }
        public User User { get; set; }

        public int RoleID { get; set; }
        public Role Role { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        private Rl_UserRole() { }

        public Rl_UserRole(int userID, int roleID, string createdBy, string modifiedBy)
        {
            UserID = userID;
            RoleID = roleID;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
