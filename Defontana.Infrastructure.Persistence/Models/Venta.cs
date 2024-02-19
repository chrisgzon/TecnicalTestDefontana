using Defontana.Domain.Ventas;

namespace Defontana.Infrastructure.Persistence.Models
{
    public partial class Venta : IVenta
    {
        public Venta()
        {
            VentaDetalles = new HashSet<VentaDetalle>();
        }

        public long IdVenta { get; set; }
        public int Total { get; set; }
        public DateTime Fecha { get; set; }
        public long IdLocal { get; set; }

        public virtual Local IdLocalNavigation { get; set; } = null!;
        public virtual ICollection<VentaDetalle> VentaDetalles { get; set; }
    }
}
