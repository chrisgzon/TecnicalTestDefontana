namespace Defontana.Domain.VentasDetalle
{
    public interface IVentaDetalle
    {
        long IdVentaDetalle { get; set; }
        long IdVenta { get; set; }
        int PrecioUnitario { get; set; }
        int Cantidad { get; set; }
        int TotalLinea { get; set; }
        long IdProducto { get; set; }
    }
}
