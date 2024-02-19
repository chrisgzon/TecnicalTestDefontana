namespace Defontana.Domain.Locales
{
    public interface ILocal
    {
        long IdLocal { get; set; }
        string Nombre { get; set; }
        string Direccion { get; set; }
    }
}