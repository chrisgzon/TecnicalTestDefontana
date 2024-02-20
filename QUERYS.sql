
	--· El total de ventas de los últimos 30 días (monto total y cantidad total de ventas).
  SELECT SUM(TOTAL) MontoVentas, COUNT(*) TotalVentas FROM 
  (SELECT v.Total FROM Producto p
  INNER JOIN VentaDetalle d ON p.ID_Producto = d.ID_Producto
  INNER JOIN Venta v ON d.ID_Venta = v.ID_Venta
  WHERE CAST(v.Fecha AS DATE) >= CAST(GETDATE()-30 AS DATE)
		AND CAST(v.Fecha AS DATE) <= CAST(GETDATE() AS DATE)
	GROUP BY v.ID_Venta, v.Fecha, v.Total) as c


	--· El día y hora en que se realizó la venta con el monto más alto (y cuál es aquel monto).
	SELECT TOP(1)c.ID_Venta, c.Fecha, c.Total FROM 
  (SELECT v.ID_Venta, v.Fecha, v.Total FROM Producto p
  INNER JOIN VentaDetalle d ON p.ID_Producto = d.ID_Producto
  INNER JOIN Venta v ON d.ID_Venta = v.ID_Venta
  WHERE CAST(v.Fecha AS DATE) >= CAST(GETDATE()-30 AS DATE)
		AND CAST(v.Fecha AS DATE) <= CAST(GETDATE() AS DATE)
	GROUP BY v.ID_Venta, v.Fecha, v.Total) as c
	ORDER BY c.Total DESC


	--· Indicar cuál es el producto con mayor monto total de ventas.
  	SELECT TOP(1)c.ID_Producto, c.Codigo, c.Total FROM 
  (SELECT p.ID_Producto, p.Codigo, SUM(d.TotalLinea) Total FROM Producto p
  INNER JOIN VentaDetalle d ON p.ID_Producto = d.ID_Producto
  INNER JOIN Venta v ON d.ID_Venta = v.ID_Venta
  WHERE CAST(v.Fecha AS DATE) >= CAST(GETDATE()-30 AS DATE)
		AND CAST(v.Fecha AS DATE) <= CAST(GETDATE() AS DATE)
	GROUP BY p.ID_Producto, p.Codigo) as c
	ORDER BY c.Total DESC


	--· Indicar el local con mayor monto de ventas.
	SELECT TOP(1)c.ID_Local, c.Nombre, c.Total FROM 
  (SELECT l.ID_Local, l.Nombre, SUM(d.TotalLinea) Total FROM Producto p
  INNER JOIN VentaDetalle d ON p.ID_Producto = d.ID_Producto
  INNER JOIN Venta v ON d.ID_Venta = v.ID_Venta
  INNER JOIN Local l ON v.ID_Local = l.ID_Local
  WHERE CAST(v.Fecha AS DATE) >= CAST(GETDATE()-30 AS DATE)
		AND CAST(v.Fecha AS DATE) <= CAST(GETDATE() AS DATE)
	GROUP BY l.ID_Local, l.Nombre) as c
	ORDER BY c.Total DESC


	--· ¿Cuál es la marca con mayor margen de ganancias?
	SELECT TOP(1)c.ID_Marca, c.Nombre, SUM(totalGanancia) Ganancia, SUM(TotalCosto) costo, SUM(totalGanancia) - SUM(TotalCosto) Margen FROM 
  (SELECT m.ID_Marca, m.Nombre, SUM(d.TotalLinea) totalGanancia, (d.Cantidad * p.Costo_Unitario) TotalCosto FROM Producto p
  INNER JOIN Marca m ON p.ID_Marca = m.ID_Marca
  INNER JOIN VentaDetalle d ON p.ID_Producto = d.ID_Producto
  INNER JOIN Venta v ON d.ID_Venta = v.ID_Venta
  INNER JOIN Local l ON v.ID_Local = l.ID_Local
  WHERE CAST(v.Fecha AS DATE) >= CAST(GETDATE()-30 AS DATE)
		AND CAST(v.Fecha AS DATE) <= CAST(GETDATE() AS DATE)
	GROUP BY m.ID_Marca, m.Nombre, d.Cantidad, p.Costo_Unitario, v.ID_Venta) as c
	GROUP BY c.ID_Marca, c.Nombre
	ORDER BY Margen DESC


	--· ¿Cómo obtendrías cuál es el producto que más se vende en cada local?
	-- consulta para obtener un unico producto con mas ventas por local(aparecera un unico producto tomado al azar si existe la misma cantidad vendida para varios productos)
	SELECT A.*, 
(
	SELECT TOP(1)c.NombreProducto FROM 
	  (SELECT l.ID_Local, l.Nombre NombreLocal, p.ID_Producto, p.Nombre NombreProducto, SUM(d.Cantidad) cantidad FROM Producto p
	  INNER JOIN Marca m ON p.ID_Marca = m.ID_Marca
	  INNER JOIN VentaDetalle d ON p.ID_Producto = d.ID_Producto
	  INNER JOIN Venta v ON d.ID_Venta = v.ID_Venta
	  INNER JOIN Local l ON v.ID_Local = l.ID_Local
	  WHERE CAST(v.Fecha AS DATE) >= CAST(GETDATE()-30 AS DATE)
			AND CAST(v.Fecha AS DATE) <= CAST(GETDATE() AS DATE)
		GROUP BY l.ID_Local, l.Nombre, p.ID_Producto, p.Nombre) as c
		WHERE c.ID_Local = A.ID_Local AND c.cantidad = A.cantidad
) Producto
FROM (
SELECT c.ID_Local, c.NombreLocal, MAX(c.cantidad) cantidad FROM 
	  (SELECT l.ID_Local, l.Nombre NombreLocal, SUM(d.Cantidad) cantidad FROM Producto p
	  INNER JOIN Marca m ON p.ID_Marca = m.ID_Marca
	  INNER JOIN VentaDetalle d ON p.ID_Producto = d.ID_Producto
	  INNER JOIN Venta v ON d.ID_Venta = v.ID_Venta
	  INNER JOIN Local l ON v.ID_Local = l.ID_Local
	  WHERE CAST(v.Fecha AS DATE) >= CAST(GETDATE()-30 AS DATE)
			AND CAST(v.Fecha AS DATE) <= CAST(GETDATE() AS DATE)
		GROUP BY l.ID_Local, l.Nombre, p.ID_Producto, p.Nombre) as c
		GROUP BY c.ID_Local, c.NombreLocal) AS A

	-- consulta para obtener los productos mas vendidos por local(aparecera mas de un producto por local si se repite la misma cantidad en varios productos)
	select b.* from
(
		SELECT c.ID_Local, c.NombreLocal, MAX(c.cantidad) cantidad
		FROM 
	  (SELECT l.ID_Local, l.Nombre NombreLocal, p.ID_Producto, p.Nombre NombreProducto, SUM(d.Cantidad) cantidad FROM Producto p
	  INNER JOIN Marca m ON p.ID_Marca = m.ID_Marca
	  INNER JOIN VentaDetalle d ON p.ID_Producto = d.ID_Producto
	  INNER JOIN Venta v ON d.ID_Venta = v.ID_Venta
	  INNER JOIN Local l ON v.ID_Local = l.ID_Local
	  WHERE CAST(v.Fecha AS DATE) >= CAST(GETDATE()-30 AS DATE)
			AND CAST(v.Fecha AS DATE) <= CAST(GETDATE() AS DATE)
		GROUP BY l.ID_Local, l.Nombre, p.ID_Producto, p.Nombre) as c
		GROUP BY c.ID_Local, c.NombreLocal
) a
inner join 
(
	SELECT c.ID_Local, c.NombreLocal, MAX(c.cantidad) cantidad, c.ID_Producto, c.NombreProducto FROM 
	  (SELECT l.ID_Local, l.Nombre NombreLocal, p.ID_Producto, p.Nombre NombreProducto, SUM(d.Cantidad) cantidad FROM Producto p
	  INNER JOIN Marca m ON p.ID_Marca = m.ID_Marca
	  INNER JOIN VentaDetalle d ON p.ID_Producto = d.ID_Producto
	  INNER JOIN Venta v ON d.ID_Venta = v.ID_Venta
	  INNER JOIN Local l ON v.ID_Local = l.ID_Local
	  WHERE CAST(v.Fecha AS DATE) >= CAST(GETDATE()-30 AS DATE)
			AND CAST(v.Fecha AS DATE) <= CAST(GETDATE() AS DATE)
		GROUP BY l.ID_Local, l.Nombre, p.ID_Producto, p.Nombre) as c
		GROUP BY c.ID_Local, c.NombreLocal, c.ID_Producto, c.NombreProducto
) b on a.ID_Local = b.ID_Local AND a.cantidad = b.cantidad
ORDER BY a.cantidad DESC, a.ID_Local DESC