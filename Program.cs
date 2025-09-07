using fotosmigracion.Middleware;
using fotosmigracion.Service;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Iniciando migración de fotos...");
        var host = CreateHostBuilder.CreateHostBuilderExecute().Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var saveNewPhotos = services.GetRequiredService<ISaveNewPhoto>();

            string folderPath = @"Z:\NuevasFotos";
            string destinationFolder = @"Z:\";

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"La carpeta {folderPath} no existe.");
                return;
            }
            await saveNewPhotos.SaveNewPhotoAsync(folderPath, destinationFolder);
        }

        Console.WriteLine("Fin migración de fotos...");
    }
}