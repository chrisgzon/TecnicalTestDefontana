using Defontana.Domain.VentaDetalles;

namespace Defontana.Domain.Ventas
{
    public interface IVentasRepository
    {
        Task<List<VentaDetalle>> ConsultaDetalleDeVentas(int numDias);
    }
}