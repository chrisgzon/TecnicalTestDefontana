namespace Defontana.Domain.Ventas
{
    public class Venta : IVenta
    {
        public long IdVenta { get; set; }
        public int Total { get; set; }
        public DateTime Fecha { get; set; }
        public long IdLocal { get; set; }
    }
}
