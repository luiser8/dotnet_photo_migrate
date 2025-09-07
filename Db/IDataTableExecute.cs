using System.Collections;
using System.Data;

namespace fotosmigracion.Db;

public interface IDataTableExecute
{
    Task<DataTable> ExecuteAsync(string name, Hashtable hashtable);
}
