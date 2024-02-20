namespace Defontana.Domain.Marcas
{
    public sealed class Marca : IMarca
    {
        public long IdMarca { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
