﻿using Defontana.Domain.Locales;
using Defontana.Domain.Marcas;
using Defontana.Domain.Productos;
using Defontana.Domain.VentaDetalles;
using Defontana.Domain.Ventas;

namespace Defontanta.Application.Ventas
{
    public class VentasLogic : IVentasLogic
    {
        const int ConsultaDiasVentas = 30;
        private readonly IVentasRepository _ventasRepository;
        private List<VentaDetalle> ventas = new();

        public VentasLogic(IVentasRepository ventasRepository)
        {
            _ventasRepository = ventasRepository;
        }

        private async Task ConsultaVentas()
        {
            if (ventas.Count == 0)
            {
                ventas = await _ventasRepository.ConsultaDetalleDeVentas(ConsultaDiasVentas);
            }
        }

        public async Task<(long montoTotalVentas, int cantidadVentas)> ConsultaTotalDeVentas()
        {
            await ConsultaVentas();
            long montoTotalVentas = ventas
                                    .GroupBy(d => d.IdVenta)
                                    .Select(g => g.Sum(d => d.TotalLinea))
                                    .Sum();

            int cantidadVentas = ventas
                                .GroupBy(d => d.IdVenta)
                                .Count();
            return (montoTotalVentas, cantidadVentas);
        }

        public async Task<Venta> ConsultaVentaMontoMasAlto()
        {
            await ConsultaVentas();
            return ventas
                    .GroupBy(d => d.Venta)
                    .Select(g => g.Key)
                    .OrderByDescending(v => v.Total)
                    .First();
        }

        public async Task<(string codigoProducto, long totalVentas)> ConsultaProductoMasVendido()
        {
            await ConsultaVentas();
            var detallesPorVentas = ventas
                                    .GroupBy(d => d.Producto.Codigo)
                                    .Select(g => new { g.Key, Total = g.Select(x => x.TotalLinea).Sum() });
            var producto = detallesPorVentas.OrderByDescending(v => v.Total).First();
            return (producto.Key, producto.Total);
        }

        public async Task<(Local local, long totalVentas)> ConsultaLocalMayorMontoDeVentas()
        {
            await ConsultaVentas();
            var detallesPorVentas = ventas
                                    .GroupBy(d => new { d.Venta.IdLocal, d.Venta.Local.Nombre })
                                    .Select(g => new { g.Key.Nombre, g.Key.IdLocal, TotalVentas = g.Select(d => d.TotalLinea).Sum() });
            var local = detallesPorVentas.OrderByDescending(v => v.TotalVentas).First();
            return (new Local { Nombre = local.Nombre, IdLocal = local.IdLocal }, local.TotalVentas);
        }

        public async Task<(Marca marca, decimal margenGanancia)> ConsultaMarcaMayorMargenDeGanancias()
        {
            await ConsultaVentas();
            var detallesPorVentas = ventas
                                    .GroupBy(d => new { d.Producto.Marca.Nombre, d.Producto.IdMarca, d.Cantidad, d.Producto.CostoUnitario, d.IdVenta })
                                    .Select(g => new { g.Key.Nombre, g.Key.IdMarca, TotalGanancias = g.Select(d => d.TotalLinea).Sum(), TotalCostoProductos = g.Key.CostoUnitario * g.Key.Cantidad })
                                    .GroupBy(d => new { d.IdMarca, d.Nombre })
                                    .Select(g => new { g.Key.IdMarca, g.Key.Nombre, Margen = g.Select(d => d.TotalGanancias).Sum() - g.Select(d => d.TotalCostoProductos).Sum() });
            var marca = detallesPorVentas.OrderByDescending(v => v.Margen).First();
            return (new Marca { Nombre = marca.Nombre, IdMarca = marca.IdMarca }, marca.Margen);
        }

        public async Task<List<ProductoMasVendidoPorLocal>> ProductoMasVendidoLocal()
        {
            await ConsultaVentas();
            return ventas
                                    .GroupBy(d => new { d.Venta.IdLocal, d.Venta.Local.Nombre })
                                    .Select(g => new
                                    {
                                        g.Key.IdLocal,
                                        g.Key.Nombre,
                                        ProducToMasVendido = g.GroupBy(d => new { d.Producto.Nombre, d.IdProducto })
                                        .Select(p => new
                                        {
                                            p.Key.Nombre,
                                            p.Key.IdProducto,
                                            CantidadVendida = p.Sum(d => d.Cantidad)
                                        })
                                        .OrderByDescending(l => l.CantidadVendida)
                                        .First()
                                    })
                                    .Select(l => new ProductoMasVendidoPorLocal
                                    {
                                        Local = new Local
                                        {
                                            Nombre = l.Nombre,
                                            IdLocal = l.IdLocal
                                        },
                                        Producto = new Producto
                                        {
                                            Nombre = l.ProducToMasVendido.Nombre,
                                            IdProducto = l.ProducToMasVendido.IdProducto
                                        },
                                        CantidadVendida = l.ProducToMasVendido.CantidadVendida
                                    })
                                    .ToList();
        }
    }
}