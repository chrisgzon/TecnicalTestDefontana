namespace Defontana.Domain.Locales
{
    public sealed class Local : ILocal
    {
        public long IdLocal { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
    }
}
