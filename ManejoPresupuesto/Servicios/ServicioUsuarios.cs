namespace ManejoPresupuesto.Servicios
{
    public interface IServiciousuarios
    {
        int ObtenerUsuarioId();
    }
    public class ServicioUsuarios : IServiciousuarios
    {
        public int ObtenerUsuarioId()
        {
            return 1;
        }
    }
}
