using System;
using System.Diagnostics;

namespace MyDataBaseTest.Helper
{
  public class DacpacPublisher
  {
    public string SqlPackageExePath { get; set; }

    public DacpacPublisher(string sqlPackageExePath)
    {
      SqlPackageExePath = sqlPackageExePath;
    }

    public void Publish(string targetConnctionString, string targetDacpacDir, string dacpacFileName)
    {
      var sourceDacpacPath = System.IO.Path.Combine(targetDacpacDir, dacpacFileName + ".dacpac");
      var param = $"/Action:publish /SourceFile:\"{sourceDacpacPath}\" /TargetConnectionString:\"{targetConnctionString}\"";
      var psInfo = new ProcessStartInfo()
      {
        FileName = SqlPackageExePath,
        Arguments = param,
        CreateNoWindow = true,
        UseShellExecute = false,
      };
      var p = Process.Start(psInfo);
      p.WaitForExit();
      if (p.ExitCode != 0)
      {
        throw new Exception($"Deployment did not complete successfully. ExitCode :{p.ExitCode} , StdError : {p.StandardError}");
      }
    }
  }
}
