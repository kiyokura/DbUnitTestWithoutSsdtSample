using System;
using System.Diagnostics;
using System.IO;

namespace MyDataBaseTest.Helper
{
  public class DacpacBuilder
  {
    public string MsBuildPath { get; set; }

    public DacpacBuilder(string msBuildPath)
    {
      MsBuildPath = msBuildPath;
    }

    public void BuildDacpac(string targetProjcetPath, string outputDirectory, string outputDacpacName)
    {
      ValidateParameter(targetProjcetPath, outputDirectory);
      ClearOutputDir(outputDirectory, outputDacpacName);
      var arg = CreateMsBuildArgument(targetProjcetPath, outputDirectory, outputDacpacName);

      var psInfo = new ProcessStartInfo()
      {
        FileName = MsBuildPath,
        Arguments = arg,
        CreateNoWindow = true,
        UseShellExecute = false,
      };

      var p = Process.Start(psInfo);
      p.WaitForExit();
      if (p.ExitCode != 0)
      {
        throw new Exception($"Build process did not complete successfully. ExitCode :{p.ExitCode}, StdError : {p.StandardError} ");
      }
    }

    private string CreateMsBuildArgument(string targetProjcetPath, string outputDirectory, string outputDacpacName)
    {
      return $"\"{targetProjcetPath}\" /t:Rebuild /p:OutputPath={outputDirectory};SqlTargetName={outputDacpacName}";
    }

    private void ClearOutputDir(string outputDirectory,string outputDacpacName)
    {
      var dacpacPath = Path.Combine(outputDirectory, outputDacpacName + "dacpac");
      if (File.Exists(dacpacPath))
      {
        File.Delete(dacpacPath);
      }
    }

    private void ValidateParameter(string targetProjcetPath, string outputDirectory)
    {
      if (!File.Exists(MsBuildPath))
      {
        throw new FileNotFoundException($"MsBuild.exe dose not found : {MsBuildPath}");
      }
      if (!File.Exists(targetProjcetPath))
      {
        throw new FileNotFoundException($"Target project file dose not found : {targetProjcetPath}");
      }
      if (!Directory.Exists(outputDirectory))
      {
        throw new DirectoryNotFoundException($"Output directory dose not found : {outputDirectory}");
      }
    }
  }
}
