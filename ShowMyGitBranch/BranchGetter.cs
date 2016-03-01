using System;
using System.IO;

namespace ShowMyGitBranch {
	internal class BranchGetter
	{

		internal string GetCurrentBranchName(string path)
		{
			var gitFolder = GitHelper.GetGitFolder(path);
			if (!string.IsNullOrEmpty(gitFolder))
			{
				return GitHelper.GetCurrentBranchName(gitFolder);
			}
			var hgFolder = HgHelper.GetHgFolder(path);
			if (!string.IsNullOrEmpty(hgFolder))
			{
				return HgHelper.GetCurrentBranchName(hgFolder);
			}
			return string.Empty;
		}


		private static class HgHelper
		{
			internal static string GetHgFolder(string path)
			{
				if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path)) return string.Empty;

				var currentPath = new DirectoryInfo(path);
				bool isAHgRepo;
				string hgFolder;
				do
				{
					hgFolder = Path.Combine(currentPath.FullName, ".hg");
					isAHgRepo = Directory.Exists(hgFolder);
					if (isAHgRepo) break;

					currentPath = currentPath.Parent;
				} while (currentPath != null && currentPath.Exists);


				return isAHgRepo ? hgFolder : string.Empty;
			}

			private static string getBranchFile(string gitFolder)
			{
				var branchFile = Path.Combine(gitFolder, "branch");
				var headFileExists = File.Exists(branchFile);

				return headFileExists ? branchFile : string.Empty;
			}

			public static string GetCurrentBranchName(string path)
			{
				var headFile = getBranchFile(path);
				if (string.IsNullOrWhiteSpace(headFile)) return string.Empty;

				return File.ReadAllText(headFile).Trim();
			}
		}


		private static class GitHelper
		{
			internal static string GetGitFolder(string path)
			{
				if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path)) return string.Empty;

				var currentPath = new DirectoryInfo(path);
				bool isAGitRepo;
				string gitFolder;
				do
				{
					gitFolder = Path.Combine(currentPath.FullName, ".git");
					isAGitRepo = Directory.Exists(gitFolder);
					if (isAGitRepo) break;

					currentPath = currentPath.Parent;
				} while (currentPath != null && currentPath.Exists);


				return isAGitRepo ? gitFolder : string.Empty;
			}

			private static string getHeadFile(string gitFolder)
			{
				var headFile = Path.Combine(gitFolder, "HEAD");
				var headFileExists = File.Exists(headFile);

				return headFileExists ? headFile : string.Empty;
			}

			private static string extractBranchNameFromHeadFile(string headFileContent)
			{
				return headFileContent.Replace(@"ref: refs/heads/", string.Empty)
					.Replace(Environment.NewLine, string.Empty)
					.Trim();
			}
			internal static string GetCurrentBranchName(string path)
			{
				var headFile = getHeadFile(path);
				if (string.IsNullOrWhiteSpace(headFile)) return string.Empty;

				var headFileContent = File.ReadAllText(headFile);
				return extractBranchNameFromHeadFile(headFileContent);
			}

		}
	}
}