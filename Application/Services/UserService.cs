using Domain.Extension;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Security.Cryptography;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Enums;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtRepository _jwtRepository;
        private readonly IRl_UserRoleRepository _rl_UserRoleRepository;
        private readonly IRl_UserAreaRepository _rl_UserAreaRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IJwtRepository jwtRepository, IRl_UserRoleRepository rl_UserRoleRepository, IRl_UserAreaRepository rl_UserAreaRepository, IAreaRepository areaRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _jwtRepository = jwtRepository;
            _rl_UserRoleRepository = rl_UserRoleRepository;
            _rl_UserAreaRepository = rl_UserAreaRepository;
            _areaRepository = areaRepository;
            _logger = logger;
        }

        public async Task<Tokens> AuthenticateAsync(string email, string password)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser == null)
                throw new EntityNotFoundException("El email");

            var validUser = await _userRepository.ValidAuth(email, EncryptPassword(password));
            if (validUser == null)
                throw new ArgumentException("Combinación incorrecta de nombre de usuario y contraseña.");

            var validActive = await _userRepository.ValidActive(validUser.UserID);
            if (validActive == null)
                throw new ArgumentException("El usuario no esta activo en el sistema.");

            // Obtener roles activos
            var userRoles = await _userRepository.GetUserRolesAsync(validUser.UserID);

            var token = _jwtRepository.GenerateToken(validUser.UserID.ToString(), validUser.FullName, validUser.Email, userRoles);

            return new Tokens { Token = token, UserData = validUser };
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllUserAsync();
        }

        public async Task<User?> GetByUserIdAsync(int userId)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException("Usuario");
            }
            return user;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user != null)
            {
                throw new EntityAlreadyExistsException("El Usuario");
            }
            return user;
        }

        public async Task<User?> CreateAsync(User user, int userCreatedBy, List<int> roles, List<int> areas)
        {
            var consultRoles = await _rl_UserRoleRepository.GetAllRoleByUserAsync(userCreatedBy);

            var hasAdminRole = consultRoles.Any(r => r.RoleID == (int)RoleEnum.Admin);
            var hasInvestigatorRole = consultRoles.Any(r => r.RoleID == (int)RoleEnum.Investigator);

            if (!hasInvestigatorRole && !hasAdminRole)
            {
                throw new ArgumentException("No tiene permisos para crear usuarios.");
            }

            if (roles == null || roles.Count == 0)
            {
                throw new ArgumentException("Debe asignar al menos un rol al usuario.");
            }

            if (areas == null || areas.Count == 0)
            {
                throw new ArgumentException("Debe asignar al menos un área al usuario.");
            }

            var existingAreas = await _areaRepository.GetByIdsAsync(areas);
            if (existingAreas.Count != areas.Count)
            {
                throw new ArgumentException("Algunas áreas enviadas no existen.");
            }

            var validRoleValues = Enum.GetValues(typeof(RoleEnum)).Cast<int>().ToList();

            if (hasAdminRole)
            {
                var invalidRoles = roles.Where(r => !validRoleValues.Contains(r)).ToList();
                if (invalidRoles.Any())
                {
                    var invalidRolesStr = string.Join(", ", invalidRoles);
                    throw new ArgumentException($"Se enviaron roles inválidos: {invalidRolesStr}");
                }
            }
            else if (hasInvestigatorRole)
            {
                if (roles.Count != 1 || roles[0] != (int)RoleEnum.Basic)
                {
                    throw new ArgumentException("Un usuario con rol Investigator solo puede asignar el rol Basic.");
                }
            }

            user.Password = EncryptPassword(user.Password);

            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new EntityAlreadyExistsException("El usuario ya existe.");
            }

            await _userRepository.AddAsync(user);

            foreach (var roleId in roles)
            {
                var rlUserRole = new Rl_UserRole(
                    user.UserID,
                    roleId,
                    userCreatedBy.ToString(),
                    userCreatedBy.ToString()
                );

                await _rl_UserRoleRepository.AddAsync(rlUserRole);
            }

            foreach (var areaId in areas)
            {
                var rlUserArea = new Rl_UserArea(
                    user.UserID,
                    areaId,
                    userCreatedBy.ToString(),
                    userCreatedBy.ToString()
                );

                await _rl_UserAreaRepository.AddAsync(rlUserArea);
            }

            return await GetByUserIdAsync(user.UserID);
        }

        public async Task<User?> UpdateAsync(User user, int userModifiedBy, List<int>? roles = null, List<int>? areas = null)
        {
            var consultRoles = await _rl_UserRoleRepository.GetAllRoleByUserAsync(userModifiedBy);

            var hasAdminRole = consultRoles.Any(r => r.RoleID == (int)RoleEnum.Admin);
            var hasInvestigatorRole = consultRoles.Any(r => r.RoleID == (int)RoleEnum.Investigator);

            if (!hasAdminRole && !hasInvestigatorRole)
            {
                throw new ArgumentException("No tiene permisos para modificar usuarios.");
            }

            var existingUser = await _userRepository.GetByUserIdAsync(user.UserID);
            if (existingUser == null)
            {
                throw new EntityNotFoundException("El usuario");
            }

            // Validar cambio de email
            if (!string.Equals(existingUser.Email, user.Email, StringComparison.OrdinalIgnoreCase))
            {
                var userWithEmail = await _userRepository.GetByEmailAsync(user.Email);
                if (userWithEmail != null && userWithEmail.UserID != user.UserID)
                {
                    throw new EntityAlreadyExistsException("Ya existe un usuario con el correo electrónico indicado.");
                }
            }

            // Actualizar campos
            existingUser.Email = user.Email ?? existingUser.Email;
            existingUser.FullName = user.FullName ?? existingUser.FullName;
            existingUser.Phone = user.Phone ?? existingUser.Phone;
            existingUser.Address = user.Address ?? existingUser.Address;
            existingUser.StateID = user.StateID > 0 ? user.StateID : existingUser.StateID;
            existingUser.ModifiedBy = userModifiedBy.ToString();
            existingUser.ModifiedAt = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                existingUser.Password = EncryptPassword(user.Password);
            }

            await _userRepository.UpdateAsync(existingUser);

            // Actualizar roles
            if (roles != null)
            {
                var validRoleValues = Enum.GetValues(typeof(RoleEnum)).Cast<int>().ToList();

                if (hasAdminRole)
                {
                    var invalidRoles = roles.Where(r => !validRoleValues.Contains(r)).ToList();
                    if (invalidRoles.Any())
                    {
                        var invalidRolesStr = string.Join(", ", invalidRoles);
                        throw new ArgumentException($"Se enviaron roles inválidos: {invalidRolesStr}");
                    }
                }
                else if (hasInvestigatorRole)
                {
                    if (roles.Count != 1 || roles[0] != (int)RoleEnum.Basic)
                    {
                        throw new ArgumentException("Un usuario con rol Investigator solo puede asignar el rol Basic.");
                    }
                }

                var currentRoles = await _rl_UserRoleRepository.GetAllRoleByUserAsync(existingUser.UserID);
                foreach (var rl in currentRoles)
                {
                    await _rl_UserRoleRepository.DeleteAsync(rl.UserID, rl.RoleID);
                }

                foreach (var roleId in roles)
                {
                    var rlUserRole = new Rl_UserRole(
                        existingUser.UserID,
                        roleId,
                        userModifiedBy.ToString(),
                        userModifiedBy.ToString()
                    );

                    await _rl_UserRoleRepository.AddAsync(rlUserRole);
                }
            }

            // Actualizar áreas
            if (areas != null)
            {
                if (areas.Count == 0)
                {
                    throw new ArgumentException("Debe asignar al menos un área al usuario.");
                }

                var existingAreas = await _areaRepository.GetByIdsAsync(areas);
                if (existingAreas.Count != areas.Count)
                {
                    throw new ArgumentException("Algunas áreas enviadas no existen.");
                }

                var currentAreas = await _rl_UserAreaRepository.GetAllAreaByUserAsync(existingUser.UserID);
                foreach (var rl in currentAreas)
                {
                    await _rl_UserAreaRepository.DeleteAsync(rl.UserID, rl.AreaID);
                }

                foreach (var areaId in areas)
                {
                    var rlUserArea = new Rl_UserArea(
                        existingUser.UserID,
                        areaId,
                        userModifiedBy.ToString(),
                        userModifiedBy.ToString()
                    );

                    await _rl_UserAreaRepository.AddAsync(rlUserArea);
                }
            }

            return await GetByUserIdAsync(user.UserID);
        }

        public async Task<List<Rl_UserRole>> GetAllRolesAsync(int userID)
        {
            return await _rl_UserRoleRepository.GetAllRoleByUserAsync(userID);
        }

        private static string EncryptPassword(string password)
        {
            var md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(Encoding.ASCII.GetBytes(password));

            //get hash result after compute it
            var result = md5.Hash;

            var strBuilder = new StringBuilder();
            foreach (var t in result)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(t.ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
