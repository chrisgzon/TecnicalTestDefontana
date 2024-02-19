namespace Defontana.Domain.Ventas
{
    public interface IVenta
    {
        long IdVenta { get; set; }
        int Total { get; set; }
        DateTime Fecha { get; set; }
        long IdLocal { get; set; }
    }
}
