namespace ManejoPresupuesto.Models
{
    public class ReporteSemanalViewModel
    {
        public decimal Ingresos => TransaccionesSemana.Sum(x => x.Ingreso);
        public decimal Gastos => TransaccionesSemana.Sum(x => x.Gastos);
        public decimal Total => Ingresos - Gastos;
        public DateTime FechaReferencia { get; set; }
        public IEnumerable<ResultadoObtenerSemana> TransaccionesSemana { get; set; }
    }
}
