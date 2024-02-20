using Defontana.Domain.Productos;
using Defontana.Domain.Ventas;

namespace Defontana.Domain.VentaDetalles
{
    public sealed class VentaDetalle : IVentaDetalle
    {
        public long IdVentaDetalle { get; set; }
        public long IdVenta { get; set; }
        public int PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public int TotalLinea { get; set; }
        public long IdProducto { get; set; }

        public Producto Producto { get; set; } = null!;
        public Venta Venta { get; set; } = null!;
    }
}