using NUnit.Framework;
using System.IO;

namespace MyDataBaseTest
{
  [SetUpFixture]
  public class Startup
  {
    [OneTimeSetUp]
    public void InitializeTest()
    {
      var settings = GlobalSettings.Instance;

      // Compile Database to dacpac
      var builder = new Helper.DacpacBuilder(settings.MsBuildPath);
      var targetProject = Path.Combine(settings.AssemblyDir, "..\\..\\..\\", "MyDataBase\\MyDataBase.sqlproj");
      builder.BuildDacpac(targetProject, settings.AssemblyDir, settings.DacpacFileName);

      // Create LocalDbInstance and Database
      Helper.LocalDbUtility.CreateInstance(settings.LocalDbInstanceName, settings.LocalDbVersion, true);
      Helper.LocalDbUtility.CreateDatabase(settings.LocalDbInstanceName, settings.DataBaseName);

      // Deploy Database
      var publisher = new Helper.DacpacPublisher(settings.SqlPackagePath);
      publisher.Publish(settings.ConnectionString, settings.AssemblyDir, settings.DacpacFileName);
    }
  }
}
