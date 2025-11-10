using blazorfactura.Components.Data;

namespace blazorfactura.Components.Data
{
    public class ServicioFacturas
    {
      
        private List<Factura> facturas = new List<Factura>();

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



      



