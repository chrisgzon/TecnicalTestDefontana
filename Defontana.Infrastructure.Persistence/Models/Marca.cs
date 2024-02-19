using Defontana.Domain.Marcas;

namespace Defontana.Infrastructure.Persistence.Models
{
    public partial class Marca : IMarca
    {
        public Marca()
        {
            Productos = new HashSet<Producto>();
        }

        public long IdMarca { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Producto> Productos { get; set; }
    }
}
