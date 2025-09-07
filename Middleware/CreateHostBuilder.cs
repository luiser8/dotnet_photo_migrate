using fotosmigracion.Db;
using fotosmigracion.DT;
using fotosmigracion.Repository;
using fotosmigracion.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace fotosmigracion.Middleware
{
    public static class CreateHostBuilder
    {
        public static IHostBuilder CreateHostBuilderExecute()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // Configurar explícitamente la ruta base y el archivo de configuración
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // Configurar la conexión a la base de datos
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

                    if (string.IsNullOrEmpty(connectionString))
                    {
                        // Mensaje más descriptivo para debugging
                        var availableConnections = context.Configuration.GetSection("ConnectionStrings").GetChildren();
                        var connectionNames = string.Join(", ", availableConnections.Select(c => c.Key));

                        throw new InvalidOperationException(
                            $"Connection string 'DefaultConnection' not found in configuration. " +
                            $"Available connection strings: {connectionNames}"
                        );
                    }

                    Console.WriteLine($"Connection string found: {connectionString.Substring(0, Math.Min(50, connectionString.Length))}...");

                    // Registrar servicios
                    services.AddScoped<IConnectionFactory>(provider => new ConnectionFactory(connectionString));
                    services.AddScoped<IDataTableExecute, DataTableExecute>();
                    services.AddScoped<IRepositoryPhotos, RepositoryPhotos>();
                    services.AddScoped<IReduceImageSizeService, ReduceImageSizeService>();
                    services.AddScoped<ISaveNewPhoto, SaveNewPhoto>();
                });
        }
    }
}
