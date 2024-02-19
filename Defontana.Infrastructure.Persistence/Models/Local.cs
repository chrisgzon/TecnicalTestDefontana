using Defontana.Domain.Locales;

namespace Defontana.Infrastructure.Persistence.Models
{
    public partial class Local : ILocal
    {
        public Local()
        {
            Venta = new HashSet<Venta>();
        }

        public long IdLocal { get; set; }
        public string Nombre { get; set; } = null!;
        public string Direccion { get; set; } = null!;

        public virtual ICollection<Venta> Venta { get; set; }
    }
}
