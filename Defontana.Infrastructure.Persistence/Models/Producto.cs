using Defontana.Domain.Productos;

namespace Defontana.Infrastructure.Persistence.Models
{
    public partial class Producto : IProducto
    {
        public Producto()
        {
            VentaDetalles = new HashSet<VentaDetalle>();
        }

        public long IdProducto { get; set; }
        public string Nombre { get; set; } = null!;
        public string Codigo { get; set; } = null!;
        public long IdMarca { get; set; }
        public string Modelo { get; set; } = null!;
        public int CostoUnitario { get; set; }

        public virtual Marca IdMarcaNavigation { get; set; } = null!;
        public virtual ICollection<VentaDetalle> VentaDetalles { get; set; }
    }
}
