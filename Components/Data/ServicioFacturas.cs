using blazorfactura.Components.Data;

namespace blazorfactura.Components.Data
{
    public class ServicioFacturas
    {
        private List<Factura> facturas = new List<Factura>
        {
            new Factura
            {
                Identificador = 1,
                Fecha = DateTime.Now.AddDays(-5),
                NombreCliente = "Juan Pérez",
                Articulos = new List<Articulo>
                {
                    new Articulo { Identificador = 1, Nombre = "Laptop", Precio = 15000, Cantidad = 1 },
                    new Articulo { Identificador = 2, Nombre = "Mouse", Precio = 1000, Cantidad = 2 }
                }
            },
            new Factura
            {
                Identificador = 2,
                Fecha = DateTime.Now.AddDays(-2),
                NombreCliente = "María García",
                Articulos = new List<Articulo>
                {
                    new Articulo { Identificador = 1, Nombre = "Teclado", Precio = 1200, Cantidad = 1 }
                }
            }
        };
        public Task <List<Factura>> ObtenerFacturas() => Task.FromResult(facturas);
       
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



      



