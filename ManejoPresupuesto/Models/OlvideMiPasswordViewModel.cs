using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class OlvideMiPasswordViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo {0} no es un correo electrónico válido")]
        public string Email { get; set; }
    }
}
