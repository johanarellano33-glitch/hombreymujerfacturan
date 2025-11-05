using blazorfactura.Components.Data;

namespace blazorfactura.Components.Data
{
    public class ServicioFacturas
    {
        public List<Factura> = new List<Factura>
        {
           new Factura 
            {
            Indentificador = 1,
             Fecha = DateTime.Now.AddDays(-5),
            NombreCliente = "Jose",
            Articulos = new List<Articulo>
    {
            new Articulo {Identificador = 1, Nombre = "Mouse", Precio = 1500, Cantidad = 2},
            new Articulo {Identificador = 2, Nombre = "Teclado", Precio = 1000, Cantidad = 1}
}
},
     new Factura
     {
         Identificador = 2,
         Fecha = DateTime.Now.AddDays(-2),
         NombreCliente = "Maria",
         Articulos = new List<Articulo>
                {
                    new Articulo { Identificador = 1, Nombre = "Monitor", Precio = 3500, Cantidad = 1 }
                    }
     }
        };
        public Task <List<Factura>> ObtenerFacturas() => Task.FromResult(Facturas);
{
            return Facturas;
}
 public Task AgregarFactura(Factura factura)
{
    Facturas.Add(factura);
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
}
}



      



