namespace Defontana.Domain.Productos
{
    public interface IProducto
    {
        long IdProducto { get; set; }
        string Nombre { get; set; }
        string Codigo { get; set; }
        long IdMarca { get; set; }
        string Modelo { get; set; }
        int CostoUnitario { get; set; }
    }
}