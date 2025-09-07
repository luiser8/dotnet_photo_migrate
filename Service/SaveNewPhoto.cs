using fotosmigracion.Repository;

namespace fotosmigracion.Service
{
    public class SaveNewPhoto : ISaveNewPhoto
    {
        private readonly IRepositoryPhotos _repositoryPhotos;
        private readonly IReduceImageSizeService _imageSizeService;

        public SaveNewPhoto(IRepositoryPhotos repositoryPhotos, IReduceImageSizeService imageSizeService)
        {
            _repositoryPhotos = repositoryPhotos;
            _imageSizeService = imageSizeService;
        }

        private async Task<int> SaveNewPhotoToDbAsync(string cedula, byte[] newImage)
        {
            if (string.IsNullOrEmpty(cedula) || newImage.Length <= 0)
                throw new ArgumentNullException(nameof(cedula));
            return await _repositoryPhotos.UpdateAllFotosRepository(cedula, newImage);
        }

        public async Task SaveNewPhotoAsync(string pathPhotos, string destinationFolder)
        {
            // Crear la carpeta destino si no existe
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
                Console.WriteLine($"Carpeta destino creada: {destinationFolder}");
            }

            // Obtén todos los archivos jpg en la carpeta especificada
            string[] fileEntries = Directory.GetFiles(pathPhotos, "*.jpg");

            Console.WriteLine($"Encontrados {fileEntries.Length} archivos JPG");
            Console.WriteLine($"Copiando a: {destinationFolder}");

            foreach (string file in fileEntries)
            {
                try
                {
                    var newImage = _imageSizeService.ConvertirImagenABytes(file);

                    // Obtiene el nombre del archivo sin la ruta completa
                    string cedula = Path.GetFileNameWithoutExtension(file);
                    string fileName = Path.GetFileName(file);
                    string destinationPath = Path.Combine(destinationFolder, fileName);

                    Console.WriteLine($"Procesando: {cedula}");

                    // 1. Guardar en db
                    int result = await SaveNewPhotoToDbAsync(cedula, newImage);

                    if (result > 0)
                    {
                        Console.WriteLine($"✓ Cédula {cedula} procesada exitosamente en BD. ID: {result}");
                        // 2. Mueve / copia archivo a la carpeta destino
                        File.Move(file, destinationPath, true); // true para sobrescribir si existe
                        Console.WriteLine($"✓ Imagen copiada a: {destinationPath}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ Cédula {cedula} no se pudo procesar en BD");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error procesando archivo {file}: {ex.Message}");
                }
            }
        }
    }
}