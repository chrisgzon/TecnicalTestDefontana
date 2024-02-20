using Defontana.Domain.Locales;
using Defontana.Domain.VentaDetalles;

namespace Defontana.Domain.Ventas
{
    public sealed class Venta : IVenta
    {
        public long IdVenta { get; set; }
        public int Total { get; set; }
        public DateTime Fecha { get; set; }
        public long IdLocal { get; set; }

        public List<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
        public Local Local { get; set; } = null!;
    }
}
