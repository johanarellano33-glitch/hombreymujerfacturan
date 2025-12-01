using blazorfactura.Components;
using blazorfactura.Components.Data;
using blazorfactura.Components.Servicios;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<ServicioControlador>();
builder.Services.AddSingleton<ServicioFacturas>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

string ruta = "facturas.db";
using var conexion = new SqliteConnection($"DataSource={ruta}");
conexion.Open();

var comando = conexion.CreateCommand();
comando.CommandText = @"
    CREATE TABLE IF NOT EXISTS Facturas (
        Identificador INTEGER PRIMARY KEY,
        Fecha TEXT,
        NombreCliente TEXT,
        Archivada INTEGER DEFAULT 0  -- <--- ¡ESTA LÍNEA ES OBLIGATORIA!
    )";
comando.ExecuteNonQuery();

comando.CommandText = @"
    CREATE TABLE IF NOT EXISTS Articulos (
        Identificador INTEGER,
        FacturaId INTEGER,
        Nombre TEXT,
        Precio REAL,
        Cantidad INTEGER,
        FOREIGN KEY(FacturaId) REFERENCES Facturas(Identificador)
    )";
comando.ExecuteNonQuery();

app.Run();
