namespace fotosmigracion.Repository;

public interface IRepositoryPhotos
{
    Task<int> UpdateAllFotosRepository(string cedula, byte[] newFoto);
}