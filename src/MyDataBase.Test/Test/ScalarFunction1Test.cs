using Dapper;
using NUnit.Framework;

namespace MyDataBaseTest.Test
{
  [TestFixture]
  public class ScalarFunction1Test
  {
    private GlobalSettings Settings = GlobalSettings.Instance;

    [Test]
    public void TestMethod1()
    {
      var expected = 2;
      using(var cn = new System.Data.SqlClient.SqlConnection(Settings.ConnectionString))
      {
        cn.Open();
        int actual = cn.ExecuteScalar<int>("SELECT dbo.ScalarFunction1(1,1) ", cn);
        Assert.AreEqual(expected, actual);
      }
    }
  }
}
