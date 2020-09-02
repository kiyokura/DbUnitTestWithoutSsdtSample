using System.IO;
using System.Reflection;

namespace MyDataBaseTest
{
  public sealed class GlobalSettings
  {
    public static GlobalSettings Instance { get; } = new GlobalSettings();

    public string LocalDbInstanceName { get; }

    public string DataBaseName { get; }

    public string LocalDbVersion { get; }

    public string ConnectionString { get; }

    public string MsBuildPath { get; }

    public string SqlPackagePath { get; }

    public string AssemblyDir { get; }

    public string DacpacFileName { get; }


    private GlobalSettings()
    {
      LocalDbInstanceName = "MyDataBaseTest";
      DataBaseName = "MyDataBase";
      LocalDbVersion = "13.0";
      ConnectionString =
      $"Data Source=(localdb)\\{LocalDbInstanceName};Initial Catalog={DataBaseName};Integrated Security=True;Persist Security Info=False;Pooling=False;";
      AssemblyDir = GetAssemblyDir();
      MsBuildPath = LoadMsBuildPath();
      SqlPackagePath = LoadSqlPackagePath();
      DacpacFileName = "output";
    }

    private string GetAssemblyDir()
    {
      return System.IO.Path.GetDirectoryName((Assembly.GetExecutingAssembly()).Location);
    }

    private string LoadMsBuildPath()
    {
      using (var sr = new StreamReader(Path.Combine(AssemblyDir, "msbuildpath.txt")))
      {
        return sr.ReadLine().Trim();
      }
    }

    private string LoadSqlPackagePath()
    {
      using (var sr = new StreamReader(Path.Combine(AssemblyDir, "sqlpackagepath.txt")))
      {
        return sr.ReadLine().Trim();
      }
    }
  }
}
