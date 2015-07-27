using NUnit.Framework;
using ShowMyGitBranch;

namespace ShowMyGitBranch_UnitTests {
    [TestFixture]
    public class BranchGetterTests {

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void IfPathIsNullOrEmptyOrWhiteSpace_MustReturnEmptyString() {
            Assert.Inconclusive("Write test");
        }
    }
}