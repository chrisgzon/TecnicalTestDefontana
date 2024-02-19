namespace Defontana.Domain.Marcas
{
    internal class Marca : IMarca
    {
        public long IdMarca { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
