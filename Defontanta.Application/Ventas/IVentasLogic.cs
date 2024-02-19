using Defontana.Domain.Ventas;

namespace Defontanta.Application.Ventas
{
    public interface IVentasLogic
    {
        Task<Venta> ConsultaTotalDeVentas(int numDays);
    }
}
