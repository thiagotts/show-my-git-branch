using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Window = EnvDTE.Window;

namespace ShowMyGitBranch {
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidShowMyGitBranchPkgString)]
    public sealed class ShowMyGitBranchPackage : Package {
        private DTE2 dte;
        private const string TitlePattern = @"[{0}] {1} - {2}";

        protected override void Initialize() {
            base.Initialize();
            dte = (DTE2) (GetGlobalService(typeof (DTE)));
            dte.Events.SolutionEvents.Opened += UpdateBranchName;
            dte.Events.WindowEvents.WindowActivated += UpdateBranchName;
            dte.Events.DocumentEvents.DocumentOpened += UpdateBranchName;
        }

        private void UpdateBranchName() {
            var fileName = dte.Solution.FileName;
            if (string.IsNullOrWhiteSpace(fileName)) return;

            var branchGetter = new BranchGetter();
            var branchName = branchGetter.GetCurrentBranchName(new FileInfo(fileName).DirectoryName);

            ChangeWindowTitle(branchName);
        }

        private void UpdateBranchName(Window gotFocus, Window lostFocus) {
            UpdateBranchName();
        }

        private void UpdateBranchName(Document document) {
            UpdateBranchName();
        }

        private void ChangeWindowTitle(string branchName) {
            var decoration = dte.Name;

            var solutionName = string.Empty;
            foreach (Property property in dte.Solution.Properties) {
                if (property.Name.Equals("Name", StringComparison.InvariantCultureIgnoreCase))
                    solutionName = property.Value as string;
            }

            var windowTitle = string.Format(TitlePattern, branchName, solutionName, decoration);
            Application.Current.MainWindow.Title = windowTitle;
        }
    }
}