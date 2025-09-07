using fotosmigracion.Db;
using System.Collections;
using System.Data;

namespace fotosmigracion.Repository;
public class RepositoryPhotos : IRepositoryPhotos
{
    private readonly IDataTableExecute _dbCon;
    private DataTable? _dt;
    private readonly Hashtable _params;

    public RepositoryPhotos(IDataTableExecute dataTableExecute)
    {
        _dt = new DataTable();
        _dbCon = dataTableExecute ?? throw new ArgumentNullException(nameof(dataTableExecute)); ;
        _params = [];
    }

    public async Task<int> UpdateAllFotosRepository(string cedula, byte[] newFoto)
    {
        _params.Clear();
        _params.Add("@Cedula", cedula);
        _params.Add("@NewFoto", newFoto);

        _dt = await _dbCon.ExecuteAsync("SP_Update_newFotos", _params);

        if (_dt.Rows.Count == 0)
            return 0;

        return Convert.ToInt32(_dt.Rows[_dt.Rows.Count - 1]["idAl"]);
    }
}