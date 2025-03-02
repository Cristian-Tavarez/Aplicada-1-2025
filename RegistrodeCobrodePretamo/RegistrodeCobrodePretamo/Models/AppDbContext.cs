using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RegistrodeCobrodePretamo.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Prestamo> Prestamos { get; set; }
    public DbSet<PrestamosDetalle> PrestamosDetalles { get; set; }
}
