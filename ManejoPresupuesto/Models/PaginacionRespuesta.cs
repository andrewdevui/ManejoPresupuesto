namespace ManejoPresupuesto.Models
{
    public class PaginacionRespuesta
    {
        public int Pagina { get; set; }
        public int RecordsPorPagina { get; set; }
        public int CantidadTotalRecords { get; set; }
        public int CantitadTotalDePaginas => (int)Math.Ceiling((double)CantidadTotalRecords / RecordsPorPagina);
        public string BaseUrl { get; set; }
    }

    public class PaginacionRespuesta<T> : PaginacionRespuesta
    {
        public IEnumerable<T> Elementos { get; set; }
    }
}
