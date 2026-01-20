namespace Domain.Entities
{
    public class User
    {
        public int UserID { get; set; }

        public int StateID { get; set; }
        public State State { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string ProfileImage { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public ICollection<Rl_UserRole> UserRoles { get; set; } = new List<Rl_UserRole>();
        public ICollection<Rl_UserCity> UserCities { get; set; } = new List<Rl_UserCity>();
        public ICollection<Rl_UserArea> UserAreas { get; set; } = new List<Rl_UserArea>();


        private User() { }

        public User(int stateId, string email, string password, string fullName)
        {
            StateID = stateId;
            Email = email;
            Password = password;
            FullName = fullName;
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
        }

        public void ChangePassword(string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException("La nueva contraseña no puede ser nula o vacía.");
            }

            Password = newPassword;
        }
    }
}
