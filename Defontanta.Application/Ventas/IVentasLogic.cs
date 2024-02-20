using Defontana.Domain.Locales;
using Defontana.Domain.Marcas;
using Defontana.Domain.Productos;
using Defontana.Domain.Ventas;

namespace Defontanta.Application.Ventas
{
    public interface IVentasLogic
    {
        Task<(long montoTotalVentas, int cantidadVentas)> ConsultaTotalDeVentas();
        Task<Venta> ConsultaVentaMontoMasAlto();
        Task<(string codigoProducto, long totalVentas)> ConsultaProductoMasVendido();
        Task<(Local local, long totalVentas)> ConsultaLocalMayorMontoDeVentas();
        Task<(Marca marca, decimal margenGanancia)> ConsultaMarcaMayorMargenDeGanancias();
        Task<List<ProductoMasVendidoPorLocal>> ProductoMasVendidoLocal();
    }
}
