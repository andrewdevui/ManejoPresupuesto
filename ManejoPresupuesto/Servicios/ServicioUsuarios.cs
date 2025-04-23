using System.Security.Claims;

namespace ManejoPresupuesto.Servicios
{
    public interface IServiciousuarios
    {
        int ObtenerUsuarioId();
    }
    public class ServicioUsuarios : IServiciousuarios
    {
        private readonly HttpContext httpContextAccessor;

        public ServicioUsuarios(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor.HttpContext;
        }
        public int ObtenerUsuarioId()
        {
            if (httpContextAccessor.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContextAccessor.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id = int.Parse(idClaim.Value);
                return id;
            }
            else
            {
                throw new ApplicationException("El usuario no esta autenticado");
            }
        }
    }
}
