using System;
using System.IO;
using NUnit.Framework;
using ShowMyGitBranch;

namespace ShowMyGitBranch_UnitTests {
    [TestFixture]
    public class BranchGetterTests {
        readonly string baseTestPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ShowMyGitBranchTests");


        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void IfPathIsNullOrEmptyOrWhiteSpace_MustReturnAnEmptyString(string path) {
            var branchGetter = new BranchGetter();
            string branchName = branchGetter.GetCurrentBranchName(path);

            Assert.AreEqual(string.Empty, branchName);
        }

        [TestCase("123432")]
        [TestCase("C:::zk\\e2")]
        [TestCase("deijij deij")]
        public void IfPathIsInvalid_MustReturnAnEmptyString(string path) {
            var branchGetter = new BranchGetter();
            string branchName = branchGetter.GetCurrentBranchName(path);

            Assert.AreEqual(string.Empty, branchName);
        }

        [Test]
        public void IfPathIsNotAGitRepo_MustReturnAnEmptyString() {
            DirectoryInfo folder = Directory.CreateDirectory(Path.Combine(baseTestPath, "Test1"));

            var branchGetter = new BranchGetter();
            string branchName = branchGetter.GetCurrentBranchName(folder.FullName);

            Assert.AreEqual(string.Empty, branchName);
        }

        [Test]
        public void IfGitFolderDoesNotHaveAHeadFile_MustReturnAnEmptyString() {
            DirectoryInfo gitFolder = Directory.CreateDirectory(Path.Combine(baseTestPath, "Test2", ".git"));

            var branchGetter = new BranchGetter();
            string branchName = branchGetter.GetCurrentBranchName(gitFolder.FullName);

            Assert.AreEqual(string.Empty, branchName);

        }

        [Test]
        public void IfPathIsAGitRepo_MustReturnTheNameOfTheCurrentBranch() {
            DirectoryInfo path = Directory.CreateDirectory(Path.Combine(baseTestPath, "Test3"));
            DirectoryInfo gitFolder = Directory.CreateDirectory(Path.Combine(path.FullName, ".git"));
            using (StreamWriter file = new StreamWriter(Path.Combine(gitFolder.FullName, "HEAD"), false)) {
                file.WriteLine("ref: refs/heads/develop123");
            }

            var branchGetter = new BranchGetter();
            string branchName = branchGetter.GetCurrentBranchName(path.FullName);

            Assert.AreEqual("develop123", branchName);
        }

        [Test]
        public void IfCurrentBranchBelongsToATree_MustReturnTheFullNameOfTheCurrentBranch() {
            DirectoryInfo path = Directory.CreateDirectory(Path.Combine(baseTestPath, "Test4"));
            DirectoryInfo gitFolder = Directory.CreateDirectory(Path.Combine(path.FullName, ".git"));
            using (StreamWriter file = new StreamWriter(Path.Combine(gitFolder.FullName, "HEAD"), false)) {
                file.WriteLine("ref: refs/heads/feature/develop123");
            }

            var branchGetter = new BranchGetter();
            string branchName = branchGetter.GetCurrentBranchName(path.FullName);

            Assert.AreEqual("feature/develop123", branchName);
        }

        [Test]
        public void IfTheGitReferencesFolderIsInAParentDirectory_MustReturnTheFullNameOfTheCurrentBranch() {
            DirectoryInfo path = Directory.CreateDirectory(Path.Combine(baseTestPath, "Test5"));
            DirectoryInfo gitFolder = Directory.CreateDirectory(Path.Combine(path.FullName, ".git"));
            using (StreamWriter file = new StreamWriter(Path.Combine(gitFolder.FullName, "HEAD"), false)) {
                file.WriteLine("ref: refs/heads/feature/develop123");
            }

            DirectoryInfo subFolder = Directory.CreateDirectory(Path.Combine(baseTestPath, "Test5", "Sub1", "Sub2"));

            var branchGetter = new BranchGetter();
            string branchName = branchGetter.GetCurrentBranchName(subFolder.FullName);

            Assert.AreEqual("feature/develop123", branchName);
        }

        [Test]
        public void IfPathIsAGitWorkTree_MustReturnTheNameOfTheCurrentBranch()
        {
            DirectoryInfo repoPath = Directory.CreateDirectory(Path.Combine(baseTestPath, "Test6"));
            DirectoryInfo gitFolder = Directory.CreateDirectory(Path.Combine(repoPath.FullName, ".git"));
            DirectoryInfo workTreeFolder = Directory.CreateDirectory(Path.Combine(gitFolder.FullName, Path.Combine("worktrees", "Test7")));
            using (StreamWriter file = new StreamWriter(Path.Combine(workTreeFolder.FullName, "HEAD"), false))
            {
                file.WriteLine("ref: refs/heads/develop123");
            }

            DirectoryInfo workTreePath = Directory.CreateDirectory(Path.Combine(baseTestPath, "Test7"));
            using (StreamWriter file = new StreamWriter(Path.Combine(workTreePath.FullName, ".git"), false))
            {
                file.WriteLine($"gitdir: {workTreeFolder.FullName.Replace("\\", "/")}");
            }

            var branchGetter = new BranchGetter();
            string branchName = branchGetter.GetCurrentBranchName(workTreePath.FullName);

            Assert.AreEqual("develop123", branchName);
        }
    }
}