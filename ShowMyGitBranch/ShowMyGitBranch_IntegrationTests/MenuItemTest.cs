using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.IntegrationTestLibrary;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using ThiagoSa.ShowMyGitBranch;

namespace ShowMyGitBranch_IntegrationTests {
    [TestClass()]
    public class MenuItemTest {
        private delegate void ThreadInvoker();

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        /// <summary>
        ///A test for lauching the command and closing the associated dialogbox
        ///</summary>
        [TestMethod()]
        [HostType("VS IDE")]
        public void LaunchCommand() {
            UIThreadInvoker.Invoke((ThreadInvoker) delegate() {
                var menuItemCmd = new CommandID(GuidList.guidShowMyGitBranchCmdSet, (int) PkgCmdIDList.cmdidFirstCommand);

                // Create the DialogBoxListener Thread.
                var expectedDialogBoxText = string.Format(CultureInfo.CurrentCulture, "{0}\n\nInside {1}.MenuItemCallback()", "ShowMyGitBranch", "ThiagoSa.ShowMyGitBranch.ShowMyGitBranchPackage");
                var purger = new DialogBoxPurger(NativeMethods.IDOK, expectedDialogBoxText);

                try {
                    purger.Start();

                    var testUtils = new TestUtils();
                    testUtils.ExecuteCommand(menuItemCmd);
                }
                finally {
                    Assert.IsTrue(purger.WaitForDialogThreadToTerminate(), "The dialog box has not shown");
                }
            });
        }
    }
}