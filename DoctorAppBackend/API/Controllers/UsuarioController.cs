using Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTO_s;
using Models.Entidades;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")] // api/usuario
    [ApiController]
    public class UsuarioController : BaseApiController
    {
        private readonly ApplicationDbContext _db;
        private readonly ITokenServicio _tokenServicio;

        public UsuarioController(ApplicationDbContext db, ITokenServicio tokenServicio)
        {
            _db = db;
            _tokenServicio = tokenServicio;
        }

        [Authorize]
        [HttpGet] //api/usuario
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _db.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        [Authorize] 
        [HttpGet("{id}")] //api/usuario/[$id]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _db.Usuarios.FindAsync(id);
            return Ok(usuario);
        }

        [HttpPost("registro")] //POST: api/usuario/registro
        public async Task<ActionResult<UsuarioDto>> RegistroUsuario(RegistroDto registro)
        {
            if (await UsuarioExiste(registro.UserName)) return BadRequest("Username ya está registrado");
            
            using var hmac = new HMACSHA512();
            var usuario = new Usuario
            {
                UserName = registro.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registro.Password)),
                PasswordSalt = hmac.Key
            };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();
            return new UsuarioDto
            {
                UserName = usuario.UserName,
                Token = _tokenServicio.CrearToken(usuario)
            };
        }

        [HttpPost("login")] //POST: api/usuario/login
        public async Task<ActionResult<UsuarioDto>> Login(LoginDto login)
        {
            var usuario = await _db.Usuarios.SingleOrDefaultAsync(x => x.UserName == login.UserName.ToLower());
            if (usuario == null) return Unauthorized("Usuario no válido");

            using var hmac = new HMACSHA512(usuario.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != usuario.PasswordHash[i]) return Unauthorized("Contraseña incorrecta");
            }
            return new UsuarioDto
            {
                UserName = usuario.UserName,
                Token = _tokenServicio.CrearToken(usuario)
            };
        }

        private async Task<bool> UsuarioExiste(string username)
        {
            return await _db.Usuarios.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
