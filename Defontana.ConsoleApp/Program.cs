using Defontana.Domain.Ventas;
using Defontana.Infrastructure.Persistence.Context;
using Defontana.Infrastructure.Persistence.Repositories;
using Defontanta.Application;
using Defontanta.Application.Ventas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

internal class Program
{
    static async Task Main(string[] args)
    {
        var services = CreateServices();

        Application app = services.GetRequiredService<Application>();
        await app.MyLogic();
    }

    private static ServiceProvider CreateServices()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();
        string connectionString = config.GetConnectionString("DefaultString");

        var serviceProvider = new ServiceCollection()
            .AddDbContext<DefontanaDbContext>(options =>
                options.UseSqlServer(connectionString)
            )
            .AddScoped<IVentasRepository, VentasRepository>()
            .AddScoped<IVentasLogic, VentasLogic>()
            .AddLogging(options =>
            {
                options.ClearProviders();
                options.AddConsole();
            })
            .AddSingleton<Application, Application>()
            .BuildServiceProvider();

        return serviceProvider;
    }
}

public class Application
{
    private readonly ILogger<Application> _logger;
    private readonly IVentasLogic _ventasLogic;

    public Application(
        ILogger<Application> logger,
        IVentasLogic ventasLogic)
    {
        _logger = logger;
        _ventasLogic = ventasLogic;
    }

    public async Task MyLogic()
    {
        try
        {
            // El total de ventas de los últimos 30 días(monto total y cantidad total de ventas).
            var (montoTotalVentas, cantidadVentasMes) = await _ventasLogic.ConsultaTotalDeVentas();
            Console.Clear();
            Console.WriteLine("* El total de ventas de los últimos 30 días (monto total y cantidad total de ventas): ");
            Console.WriteLine($"R: Monto total: {montoTotalVentas}. Cantidad Total: {cantidadVentasMes}");
            Console.WriteLine();

            // El día y hora en que se realizó la venta con el monto más alto(y cuál es aquel monto)
            var resultMontoMasAlto = await _ventasLogic.ConsultaVentaMontoMasAlto();
            Console.WriteLine("* El día y hora en que se realizó la venta con el monto más alto(y cuál es aquel monto: ");
            Console.WriteLine($"R: Día: {resultMontoMasAlto.Fecha}. Monto: {resultMontoMasAlto.Total}");
            Console.WriteLine();

            // Indicar cuál es el producto con mayor monto total de ventas
            var (codigoProducto, totalVentasProducto) = await _ventasLogic.ConsultaProductoMasVendido();
            Console.WriteLine("* Indicar cuál es el producto con mayor monto total de ventas: ");
            Console.WriteLine($"R: Producto: {codigoProducto}. Monto: {totalVentasProducto}");
            Console.WriteLine();

            // Indicar el local con mayor monto de ventas
            var (local, montoVentas) = await _ventasLogic.ConsultaLocalMayorMontoDeVentas();
            Console.WriteLine("* Indicar el local con mayor monto de ventas: ");
            Console.WriteLine($"R: local: {local.Nombre}({local.IdLocal}). Monto: {montoVentas}");
            Console.WriteLine();

            // Cuál es la marca con mayor margen de ganancias
            var (marca, margen) = await _ventasLogic.ConsultaMarcaMayorMargenDeGanancias();
            Console.WriteLine("* Cuál es la marca con mayor margen de ganancias: ");
            Console.WriteLine($"R: Marca: {marca.Nombre}({marca.IdMarca}). Margen: {margen}");
            Console.WriteLine();

            // Cómo obtendrías cuál es el producto que más se vende en cada local
            List<ProductoMasVendidoPorLocal> productosPorLocal = await _ventasLogic.ProductoMasVendidoLocal();
            Console.WriteLine("* Cómo obtendrías cuál es el producto que más se vende en cada local: ");
            Console.WriteLine("**-------------------------------**-----------------------**--------------------------** ");
            foreach (ProductoMasVendidoPorLocal localProducto in productosPorLocal)
            {
                Console.WriteLine();
                Console.WriteLine($"R: El producto mas vendido para el local {localProducto.Local.Nombre}({localProducto.Local.IdLocal}) es: {localProducto.Producto.Nombre}({localProducto.Producto.IdProducto}) Con la cantidad de: {localProducto.CantidadVendida}");
            }
        } catch (Exception ex)
        {
            _logger.LogInformation("Error: {error}", ex.Message);
        }
    }
}