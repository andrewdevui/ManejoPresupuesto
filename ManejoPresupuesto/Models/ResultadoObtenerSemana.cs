namespace ManejoPresupuesto.Models
{
    public class ResultadoObtenerSemana
    {
        public int Semana { get; set; }
        public decimal Monto { get; set; }
        public TipoOperacion tipoOperacionId { get; set; }
        public decimal Ingreso { get; set; }
        public decimal Gastos { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
