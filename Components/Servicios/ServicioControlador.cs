using blazorfactura.Components.Data;
using System.Runtime.CompilerServices;
namespace blazorfactura.Components.Servicios
{
    public class ServicioControlador
    {
        private readonly ServicioFacturas _servicioFacturas;

        public ServicioControlador(ServicioFacturas servicioFacturas)
        {
            _servicioFacturas = servicioFacturas;
        }
        public async Task<List<Factura>> ObtenerFacturas()
        {
            return await _servicioFacturas.ObtenerFacturas();
        }
        public async Task AgregarFactura(Factura factura)
        {
            factura.Identificador = await GenerarNuevoID();
            int articuloID = 1;
            foreach (var articulo in factura.Articulos)
            {
                articulo.Identificador = articuloID++;
            }

            await _servicioFacturas.AgregarFactura(factura);
        }

        private async Task<int> GenerarNuevoID()
        {
            var facturas = await _servicioFacturas.ObtenerFacturas();
            return facturas.Any() ? facturas.Max(f => f.Identificador) + 1 : 1;
        }
    
    public async Task EliminarFactura(int identificador)
        {
            await _servicioFacturas.EliminarFactura(identificador);
        }
        public async Task ActualizarFactura(Factura factura)
        {
            await _servicioFacturas.ActualizarFactura(factura);

        }
        public async Task EliminarArticulo(int facturaId, int articuloId)
        {
            await _servicioFacturas.EliminarArticulo(facturaId, articuloId);
        }

        public async Task ActualizarArticulo(int facturaId, Articulo articulo)
        {
            await _servicioFacturas.ActualizarArticulo(facturaId, articulo);
        }

        public async Task AgregarArticuloAFacturaExistente(int facturaId, Articulo articulo)
        {
            var facturas = await _servicioFacturas.ObtenerFacturas();
            var factura = facturas.FirstOrDefault(f => f.Identificador == facturaId);

            if (factura != null && factura.Articulos.Any())
            {
                articulo.Identificador = factura.Articulos.Max(a => a.Identificador) + 1;
            }
            else
            {
                articulo.Identificador = 1;
            }

            await _servicioFacturas.AgregarArticuloAFacturaExistente(facturaId, articulo);
        }
    }
}
