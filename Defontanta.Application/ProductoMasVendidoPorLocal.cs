using Defontana.Domain.Locales;
using Defontana.Domain.Productos;

namespace Defontanta.Application
{
    public class ProductoMasVendidoPorLocal
    {
        public Local Local { get; set; } = null!;
        public Producto Producto { get; set; } = null!;
        public int CantidadVendida { get; set; }
    }
}
