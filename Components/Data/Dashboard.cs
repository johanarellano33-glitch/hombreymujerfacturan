namespace blazorfactura.Components.Data
{
    public class DashboardDatos
    {
       
        public string ProductoMasVendido { get; set; } = "Sin datos";

        
        public string MesMejorVenta { get; set; } = "Sin datos";

        public string ClienteVip { get; set; } = "Sin datos";

       
        public decimal IngresosTotales { get; set; } = 0;

        
        public decimal TicketPromedio { get; set; } = 0;
        public string ProductoMenosVendido { get; set; } = "Sin datos";

        public int TotalArticulosVendidos { get; set; } = 0;

    }

}