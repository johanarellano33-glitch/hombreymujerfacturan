using blazorfactura.Components.Data;
using Microsoft.Data.Sqlite;

namespace blazorfactura.Components.Data
{
    public class ServicioFacturas
    {
      
        private List<Factura> facturas = new List<Factura>();

        public async Task<List<Factura>> ObtenerFacturas()
        {
            facturas.Clear();
            string ruta = "facturas.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = @"
                SELECT Identificador, Fecha, NombreCliente FROM Facturas";

            using var lector = await comando.ExecuteReaderAsync();
            while (await lector.ReadAsync())
            {
                var factura = new Factura
                {
                    Identificador = lector.GetInt32(0),
                    Fecha = DateTime.Parse(lector.GetString(1)),
                    NombreCliente = lector.GetString(2)
                };

             
                factura.Articulos = await ObtenerArticulosPorFactura(factura.Identificador);
                facturas.Add(factura);
            }

            return facturas;
        }
        private async Task<List<Articulo>> ObtenerArticulosPorFactura(int facturaId)
        {
            var articulos = new List<Articulo>();
            string ruta = "facturas.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = @"
                SELECT Identificador, Nombre, Precio, Cantidad 
                FROM Articulos WHERE FacturaId = $FACTURAID";
            comando.Parameters.AddWithValue("$FACTURAID", facturaId);

            using var lector = await comando.ExecuteReaderAsync();
            while (await lector.ReadAsync())
            {
                articulos.Add(new Articulo
                {
                    Identificador = lector.GetInt32(0),
                    Nombre = lector.GetString(1),
                    Precio = lector.GetDecimal(2),
                    Cantidad = lector.GetInt32(3)
                });
            }

            return articulos;
        }

        public Task AgregarFactura(Factura factura)
{
    facturas.Add(factura);
    return Task.CompletedTask;
}

public Task EliminarFactura(int identificador)
{
    var factura = facturas.FirstOrDefault(f => f.Identificador == identificador);
    if (factura != null)
    {
        facturas.Remove(factura);
    }
    return Task.CompletedTask;
}
        public Task ActualizarFactura(Factura factura)
        {
            var facturaExistente = facturas.FirstOrDefault(f => f.Identificador == factura.Identificador);
            if (facturaExistente != null)
            {
                facturaExistente.Fecha = factura.Fecha;
                facturaExistente.NombreCliente = factura.NombreCliente;
                facturaExistente.Articulos = factura.Articulos;
            }
            return Task.CompletedTask;
        }


    }
}



      



