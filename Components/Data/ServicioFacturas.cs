using Microsoft.Data.Sqlite;
using blazorfactura.Components.Data;

namespace blazorfactura.Components.Data
{
    public class ServicioFacturas
    {
        private List<Factura> facturas = new List<Factura>();
        private string ruta = "facturas.db";

 
        public async Task<List<Factura>> ObtenerFacturas(bool incluirArchivadas = false)
        {
            facturas.Clear();

            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();


            string sql = @"
                SELECT Identificador, Fecha, NombreCliente, Archivada 
                FROM Facturas 
                WHERE Archivada = $ESTADO";

            comando.CommandText = sql;

            comando.Parameters.AddWithValue("$ESTADO", incluirArchivadas ? 1 : 0);

            using var lector = await comando.ExecuteReaderAsync();
            while (await lector.ReadAsync())
            {
                var factura = new Factura
                {
                    Identificador = lector.GetInt32(0),
                    Fecha = DateTime.Parse(lector.GetString(1)),
                    NombreCliente = lector.GetString(2),

                    Archivada = lector.GetInt32(3) == 1
                };


                factura.Articulos = await ObtenerArticulosPorFactura(factura.Identificador);
                facturas.Add(factura);
            }

            return facturas;
        }

        public async Task CambiarEstadoArchivo(int id, bool archivar)
        {
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = "UPDATE Facturas SET Archivada = $ESTADO WHERE Identificador = $ID";

            comando.Parameters.AddWithValue("$ESTADO", archivar ? 1 : 0);
            comando.Parameters.AddWithValue("$ID", id);

            await comando.ExecuteNonQueryAsync();
        }

        private async Task<List<Articulo>> ObtenerArticulosPorFactura(int facturaId)
        {
            var articulos = new List<Articulo>();
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

        public async Task AgregarFactura(Factura factura)
        {
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
      
            comando.CommandText = @"
                INSERT INTO Facturas (Identificador, Fecha, NombreCliente, Archivada) 
                VALUES ($IDENTIFICADOR, $FECHA, $NOMBRECLIENTE, 0)";

            comando.Parameters.AddWithValue("$IDENTIFICADOR", factura.Identificador);
            comando.Parameters.AddWithValue("$FECHA", factura.Fecha.ToString("yyyy-MM-dd"));
            comando.Parameters.AddWithValue("$NOMBRECLIENTE", factura.NombreCliente);

            await comando.ExecuteNonQueryAsync();

            foreach (var articulo in factura.Articulos)
            {
                await AgregarArticulo(factura.Identificador, articulo);
            }

            facturas.Add(factura);
        }

        private async Task AgregarArticulo(int facturaId, Articulo articulo)
        {
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = @"
                INSERT INTO Articulos (Identificador, FacturaId, Nombre, Precio, Cantidad) 
                VALUES ($IDENTIFICADOR, $FACTURAID, $NOMBRE, $PRECIO, $CANTIDAD)";

            comando.Parameters.AddWithValue("$IDENTIFICADOR", articulo.Identificador);
            comando.Parameters.AddWithValue("$FACTURAID", facturaId);
            comando.Parameters.AddWithValue("$NOMBRE", articulo.Nombre);
            comando.Parameters.AddWithValue("$PRECIO", articulo.Precio);
            comando.Parameters.AddWithValue("$CANTIDAD", articulo.Cantidad);

            await comando.ExecuteNonQueryAsync();
        }

        public async Task EliminarFactura(int identificador)
        {
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();


            var comando = conexion.CreateCommand();
            comando.CommandText = @"DELETE FROM Articulos WHERE FacturaId=$IDENTIFICADOR";
            comando.Parameters.AddWithValue("$IDENTIFICADOR", identificador);
            await comando.ExecuteNonQueryAsync();

            comando = conexion.CreateCommand();
            comando.CommandText = @"DELETE FROM Facturas WHERE Identificador=$IDENTIFICADOR";
            comando.Parameters.AddWithValue("$IDENTIFICADOR", identificador);
            await comando.ExecuteNonQueryAsync();
        }

        public async Task ActualizarFactura(Factura factura)
        {
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = @"
                UPDATE Facturas 
                SET Fecha=$FECHA, NombreCliente=$NOMBRECLIENTE 
                WHERE Identificador=$IDENTIFICADOR";

            comando.Parameters.AddWithValue("$IDENTIFICADOR", factura.Identificador);
            comando.Parameters.AddWithValue("$FECHA", factura.Fecha.ToString("yyyy-MM-dd"));
            comando.Parameters.AddWithValue("$NOMBRECLIENTE", factura.NombreCliente);

            await comando.ExecuteNonQueryAsync();
        }

        public async Task EliminarArticulo(int facturaId, int articuloId)
        {
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = @"
                DELETE FROM Articulos 
                WHERE FacturaId=$FACTURAID AND Identificador=$ARTICULOID";
            comando.Parameters.AddWithValue("$FACTURAID", facturaId);
            comando.Parameters.AddWithValue("$ARTICULOID", articuloId);
            await comando.ExecuteNonQueryAsync();
        }

        public async Task ActualizarArticulo(int facturaId, Articulo articulo)
        {
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = @"
                UPDATE Articulos 
                SET Nombre=$NOMBRE, Precio=$PRECIO, Cantidad=$CANTIDAD 
                WHERE FacturaId=$FACTURAID AND Identificador=$ARTICULOID";

            comando.Parameters.AddWithValue("$FACTURAID", facturaId);
            comando.Parameters.AddWithValue("$ARTICULOID", articulo.Identificador);
            comando.Parameters.AddWithValue("$NOMBRE", articulo.Nombre);
            comando.Parameters.AddWithValue("$PRECIO", articulo.Precio);
            comando.Parameters.AddWithValue("$CANTIDAD", articulo.Cantidad);

            await comando.ExecuteNonQueryAsync();
        }

        public async Task AgregarArticuloAFacturaExistente(int facturaId, Articulo articulo)
        {
            await AgregarArticulo(facturaId, articulo);
        }
    }
}