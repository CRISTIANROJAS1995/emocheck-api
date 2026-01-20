namespace Domain.Entities
{
    public class Rl_RolePermission
    {
        public int RoleID { get; set; }
        public Role Role { get; set; }

        public int PermissionID { get; set; }
        public Permission Permission { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        private Rl_RolePermission() { }

        public Rl_RolePermission(int roleID, int permissionID, string createdBy, string modifiedBy)
        {
            RoleID = roleID;
            PermissionID = permissionID;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
