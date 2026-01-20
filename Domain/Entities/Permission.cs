namespace Domain.Entities
{
    public class Permission
    {
        public int PermissionID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public ICollection<Rl_RolePermission> RolePermissions { get; set; } = new List<Rl_RolePermission>();

        private Permission() { }

        public Permission(string name, string createdBy, string modifiedBy)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("El nombre del permiso no puede ser nulo o vacío.");
            }

            Name = name;
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.Now;
        }
    }
}
