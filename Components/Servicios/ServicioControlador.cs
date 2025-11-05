using blazorfactura.Components.Data;
namespace blazorfactura.Components.Servicios
{
    public class ServicioControlador
    {
        private readonly ServicioFacturas _servicioFacturas;

        public ServicioControlador(ServicioFacturas servicioFacturas)
        {
            _servicioFacturas = servicioFacturas;
        }
        public async Task <List<Factura>> ObtenerFacturas()
        {
            return await _servicioFacturas.ObtenerFacturas();
        }
        public async Task AgregarFactura (Factura factura)
        {
            factura.Identificador = await GenerarNuevoID();
            int articuloID = 1;
            foreach (var articulo in factura.Articulos)
            {
                articulo.Identificador = articuloId++;
            }

            await _servicioFacturas.AgregarFactura(factura);
        }

        private async Task<int> GenerarNuevoID()
        {
            var facturas = await _servicioFacturas.ObtenerFacturas();
            return facturas.Any() ? facturas.Max(f => f.Identificador) + 1 : 1;
        }
    }

    }
}
