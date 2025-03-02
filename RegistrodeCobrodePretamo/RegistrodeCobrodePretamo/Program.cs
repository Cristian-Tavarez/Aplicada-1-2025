using Microsoft.EntityFrameworkCore;
using RegistrodeCobrodePretamo.Models;
using RegistrodeCobrodePretamo.Layout;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=RegistroPrestamosDB;Trusted_Connection=True;"));

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

app.UseHttpsRedirection();

// 📌 SOLUCIÓN: Todos los `MapPost` deben devolver `IResult`
app.MapPost("/api/prestamos", async (Prestamo prestamo, AppDbContext db) =>
{
    if (prestamo == null) return Results.BadRequest("Datos inválidos");

    db.Prestamos.Add(prestamo);
    await db.SaveChangesAsync();

    return Results.Ok(prestamo); // ✅ Devuelve `IResult`
});

// 📌 SOLUCIÓN: Ajustar retorno en `aplicar-pago`
app.MapPost("/api/prestamos/aplicar-pago", async (PagoDto pago, AppDbContext db) =>
{
    var prestamo = await db.Prestamos.Include(p => p.Cuotas)
                        .FirstOrDefaultAsync(p => p.Id == pago.PrestamoId);

    if (prestamo == null) return Results.NotFound("Préstamo no encontrado");

    decimal montoRestante = pago.Monto;

    foreach (var cuota in prestamo.Cuotas.OrderBy(c => c.CuotaNo))
    {
        if (montoRestante <= 0) break;

        if (cuota.Balance > 0)
        {
            decimal pagoAplicado = Math.Min(montoRestante, cuota.Balance);
            cuota.Balance -= pagoAplicado;
            montoRestante -= pagoAplicado;
        }
    }

    prestamo.MontoPagado += pago.Monto;
    prestamo.EstaPagado = prestamo.Cuotas.All(c => c.Balance == 0);

    await db.SaveChangesAsync();
    return Results.Ok(prestamo); // ✅ Devuelve `IResult`
});

app.Run();

