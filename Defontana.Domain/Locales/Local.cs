namespace Defontana.Domain.Locales
{
    internal class Local : ILocal
    {
        public long IdLocal { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
    }
}
