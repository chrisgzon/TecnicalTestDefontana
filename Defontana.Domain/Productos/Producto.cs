using Defontana.Domain.Marcas;

namespace Defontana.Domain.Productos
{
    public sealed class Producto : IProducto
    {
        public long IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public long IdMarca { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public int CostoUnitario { get; set; }

        public Marca Marca { get; set; } = null!;
    }
}