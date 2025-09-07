namespace fotosmigracion.Service
{
    public interface ISaveNewPhoto
    {
        Task SaveNewPhotoAsync(string pathPhotos, string destinationFolder);
    }
}