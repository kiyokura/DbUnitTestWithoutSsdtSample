using MartinCostello.SqlLocalDb;
using System;
using System.Data;
using System.Linq;

namespace MyDataBaseTest.Helper
{
  /// <summary>
  /// Utility for SQL Server Express LocalDB
  /// </summary>
  public static class LocalDbUtility
  {
    /// <summary>
    /// Create an instance of SQL Server LocalDB
    /// </summary>
    /// <param name="instanceName">instance name</param>
    /// <param name="version">version of SQL Serer LocalDB</param>
    /// <param name="isRecreate">If an instance of the same name exists, destroy it before creating it</param>
    public static void CreateInstance(string instanceName, string version, bool isRecreate)
    {
      var sqlLocalDbApi = new SqlLocalDbApi();
      if (sqlLocalDbApi.GetInstanceNames().Contains(instanceName))
      {
        if (isRecreate)
        {
          // For details of options, see API Reference : https://msdn.microsoft.com/ja-JP/library/hh234692.aspx
          sqlLocalDbApi.StopInstance(instanceName, StopInstanceOptions.KillProcess, new TimeSpan(0, 0, 30));
          sqlLocalDbApi.DeleteInstance(instanceName, true);
        }
        else
        {
          return;
        }
      }
      sqlLocalDbApi.CreateInstance(instanceName, version);
    }

    /// <summary>
    /// Create a database under the instance folde
    /// </summary>
    /// <param name="instanceName">instance name</param>
    /// <param name="databaseName">database name</param>
    public static void CreateDatabase(string instanceName, string databaseName)
    {
      var connectionString = GetConnectionString(instanceName, "master");
      var path = System.IO.Path.Combine(GetInstancePath(instanceName), databaseName + ".mdf");

      var nameSring = "'" + databaseName + "'";
      var filePath = "'" + path + "'";
      var sql = $"create database {databaseName}"
                    + $" on ( "
                    + $"   name = {nameSring},"
                    + $"   filename = {filePath}"
                    + $" )";

      using (var cn = new System.Data.SqlClient.SqlConnection(connectionString))
      {
        cn.Open();
        using (var cmd = cn.CreateCommand())
        {
          cmd.CommandText = sql;
          cmd.CommandType = CommandType.Text;
          cmd.ExecuteNonQuery();
        }
      }
    }

    /// <summary>
    /// Get the connection string for SQL Serer LocalDB
    /// </summary>
    /// <param name="instanceName">instance name</param>
    /// <param name="databaseName">database name</param>
    /// <returns>connection string for the database of the SQL Serer LocalDB</returns>
    public static string GetConnectionString(string instanceName, string databaseName)
    {
      const string connectionStringBase = "Data Source=(localdb)\\{0};Initial Catalog={1};Integrated Security=True;Persist Security Info=False;Pooling=False;";
      return GetConnectionString(connectionStringBase, instanceName, databaseName);
    }

    /// <summary>
    /// Get the connection string for SQL Serer LocalDB
    /// </summary>
    /// <param name="connectionStringBase">connection string that instancename and databasename are place folder</param>
    /// <param name="instanceName">instance name</param>
    /// <param name="databaseName">database name</param>
    /// <returns>connection string for the database of the SQL Serer LocalDB</returns>
    public static string GetConnectionString(string connectionStringBase, string instanceName, string databaseName)
    {
      return string.Format(connectionStringBase, instanceName, databaseName);
    }

    /// <summary>
    /// Get the full path of the directory containing the SQL server LocalDB instance files for the current user.
    /// </summary>
    /// <param name="instanceName">instance name</param>
    /// <returns>full path of the directory containing the SQL server LocalDB instance</returns>
    public static string GetInstancePath(string instanceName)
    {
      return System.IO.Path.Combine(SqlLocalDbApi.GetInstancesFolderPath(), instanceName);
    }
  }
}
