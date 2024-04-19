using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO_s
{
    public class RegistroDto
    {
        [Required(ErrorMessage = "Username es obligatorio")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "La contraseña debe tener entre 4 y 10 caracteres")]
        public string Password { get; set; }
    }
}
