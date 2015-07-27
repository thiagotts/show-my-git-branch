using Microsoft.VisualStudio.Shell.Interop;
using NUnit.Framework;
using ShowMyGitBranch;

namespace ShowMyGitBranch_UnitTests {
    [TestFixture]
    public class PackageTest {
        [Test]
        public void CreateInstance() {
            var package = new ShowMyGitBranchPackage();
        }

        [Test]
        public void IsIVsPackage() {
            var package = new ShowMyGitBranchPackage();
            Assert.IsNotNull(package as IVsPackage, "The object does not implement IVsPackage");
        }
    }
}