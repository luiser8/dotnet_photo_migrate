using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace fotosmigracion.Service;

public class ReduceImageSizeService : IReduceImageSizeService
{
    public byte[] ConvertirImagenABytes(string imageRoute)
    {
        // Cargar la imagen original
        using Image image = Image.Load(imageRoute);

        // Crear una imagen en miniatura
        image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));

        // Guardar la imagen optimizada en un MemoryStream temporal
        using MemoryStream ms = new();
        // Configurar las opciones del codificador JPEG para reducir la calidad de la imagen
        var encoderOptions = new JpegEncoder
        {
            Quality = 75
        };

        // Guardar la imagen en el MemoryStream
        image.Save(ms, encoderOptions);

        // Si el tamaño de la imagen es mayor que 100KB, reducir la calidad
        while (ms.Length > 100 * 1024) // 100KB
        {
            ms.SetLength(0); // Limpiar el MemoryStream

            // Reducir la calidad de la imagen
            encoderOptions = new JpegEncoder
            {
                Quality = encoderOptions.Quality - 10
            };
            image.Save(ms, encoderOptions);
        }

        // Convertir el MemoryStream a byte[]
        return ms.ToArray();
    }
}