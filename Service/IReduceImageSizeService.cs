namespace fotosmigracion.Service
{
    public interface IReduceImageSizeService
    {
        byte[] ConvertirImagenABytes(string imageRoute);
    }
}
