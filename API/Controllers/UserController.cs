using Microsoft.AspNetCore.Mvc;
using Domain.Exceptions;
using Domain.Interfaces.Services;
using Application.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Application.Facades;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserHierarchyFacade _userAccessFacade;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            UserHierarchyFacade userHierarchyFacade,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _userAccessFacade = userHierarchyFacade;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Obténer el ID del usuario autenticado
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized("No se pudo identificar al usuario que hace la petición");
                }

                var user = await _userService.GetByUserIdAsync(Convert.ToInt32(currentUserId));
                return Ok(UserMapper.ToDto(user!));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning($"Acceso no autorizado: {ex.Message}");
                return Forbid(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al consultar el usuario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [Authorize(Roles = "admin,investigator")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByUserIdAsync(id);
                return Ok(UserMapper.ToDto(user!));
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "admin,investigator")]
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            var userDtos = UserMapper.ToDtoList(users);
            return Ok(userDtos);
        }

        [Authorize(Roles = "admin,investigator")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatedUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Obténer el ID del usuario autenticado
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized("No se pudo identificar al usuario que hace la petición");
            }

            var user = UserMapper.ToEntity(userDto, currentUserId);
            try
            {
                var createdUser = await _userService.CreateAsync(user, Convert.ToInt32(currentUserId), userDto.Roles, userDto.Areas);
                return CreatedAtAction(nameof(GetById), new { id = createdUser }, UserMapper.ToDto(createdUser!));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning($"Acceso no autorizado: {ex.Message}");
                return Forbid(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"{ex.Message}");
                return BadRequest($"Error en los datos: {ex.Message}");
            }
            catch (EntityAlreadyExistsException ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear eñ usuario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [Authorize(Roles = "admin,investigator")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Obténer el ID del usuario autenticado
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized("No se pudo identificar al usuario que hace la petición");
            }

            var user = UserMapper.ToEntity(userDto, currentUserId);
            user.UserID = id;

            try
            {
                var updatedUser = await _userService.UpdateAsync(user, Convert.ToInt32(currentUserId), userDto.Roles, userDto.Areas);
                return Ok(UserMapper.ToDto(updatedUser!));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning($"Acceso no autorizado: {ex.Message}");
                return Forbid(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"{ex.Message}");
                return BadRequest($"Error en los datos: {ex.Message}");
            }
            catch (EntityAlreadyExistsException ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return Conflict(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el usuario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [Authorize]
        [HttpGet("access")]
        public async Task<IActionResult> GetUserAccess()
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized("No se pudo identificar al usuario que hace la petición");
                }

                var hierarchy = await _userAccessFacade.GetUserHierarchyAsync(Convert.ToInt32(currentUserId));
                return Ok(hierarchy);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener la jerarquía de acceso del usuario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
