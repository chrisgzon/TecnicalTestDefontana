using Defontana.Domain.Locales;
using Defontana.Domain.Marcas;
using Defontana.Domain.Productos;
using Defontana.Domain.VentaDetalles;
using Defontana.Domain.Ventas;
using Defontana.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Defontana.Infrastructure.Persistence.Repositories
{
    public class VentasRepository : IVentasRepository
    {

        private readonly DefontanaDbContext _Context;
        public VentasRepository(DefontanaDbContext context)
        {
            _Context = context;
        }
        public async Task<List<VentaDetalle>> ConsultaDetalleDeVentas(int numDias)
        {
            DateTime DiaActual = DateTime.Now;
            return await _Context.VentaDetalles
                        .Where(v => v.IdVentaNavigation.Fecha.Date >= DiaActual.AddDays(-numDias).Date
                                    && v.IdVentaNavigation.Fecha.Date <= DiaActual.Date)
                        .Select(d => new VentaDetalle
                        {
                            IdVenta = d.IdVenta,
                            Cantidad = d.Cantidad,
                            IdProducto = d.IdProducto,
                            IdVentaDetalle = d.IdVentaDetalle,
                            PrecioUnitario = d.PrecioUnitario,
                            TotalLinea = d.TotalLinea,
                            Producto = new Producto
                            {
                                Codigo = d.IdProductoNavigation.Codigo,
                                CostoUnitario = d.IdProductoNavigation.CostoUnitario,
                                IdMarca = d.IdProductoNavigation.IdMarca,
                                Modelo = d.IdProductoNavigation.Modelo,
                                Nombre = d.IdProductoNavigation.Nombre,
                                Marca = new Marca
                                {
                                    Nombre = d.IdProductoNavigation.IdMarcaNavigation.Nombre,
                                }
                            },
                            Venta = new Venta
                            {
                                Fecha = d.IdVentaNavigation.Fecha,
                                Total = d.IdVentaNavigation.Total,
                                IdLocal = d.IdVentaNavigation.IdLocal,
                                Local = new Local
                                {
                                    Direccion = d.IdVentaNavigation.IdLocalNavigation.Direccion,
                                    Nombre = d.IdVentaNavigation.IdLocalNavigation.Nombre
                                }
                            }
                        })
                        .ToListAsync();
        }
    }
}
