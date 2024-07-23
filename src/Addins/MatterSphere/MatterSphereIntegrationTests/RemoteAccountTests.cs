using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FWBS.MatterSphereIntegration;

namespace MatterSphereIntegrationTests
{
    [TestClass]
    public class RemoteAccountTests
    {
        [TestMethod]
        public void GeneratePronouncablePassword_Test()
        {
            string StrRegex = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,12}$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(StrRegex);

            for (int i = 0; i < 101; i++)
            {
                string StrPassword = RemoteAccount.GeneratePronouncablePassword(9);
                System.Diagnostics.Debug.WriteLine("Password = " + StrPassword);
                Assert.IsTrue(regex.Match(StrPassword).Success);
            }
        }
    }
}
