namespace blazorfactura.Components.Data
{
    public class Factura
    {
        public int Identificador { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreCliente { get; set; }
        public List<Articulo> Articulos { get; set; } = new List<Articulo>();
        public decimal Total => Articulos.Sum(a => a.SubTotal);
    }
}
