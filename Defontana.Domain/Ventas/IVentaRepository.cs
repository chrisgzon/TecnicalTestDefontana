namespace Defontana.Domain.Ventas
{
    public interface IVentaRepository
    {
        Task<List<Venta>> ConsultaDetalleDeVentas(int numDays);
    }
}