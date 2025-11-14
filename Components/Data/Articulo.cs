namespace blazorfactura.Components.Data
{
    public class Articulo
    {
       public int Identificador { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => Precio * Cantidad;   

    }
}
