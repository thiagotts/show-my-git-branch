using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;

namespace ShowMyGitBranch {
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidShowMyGitBranchPkgString)]
    public sealed class ShowMyGitBranchPackage : Package {
        private DTE2 dte;

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public ShowMyGitBranchPackage() {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize() {
            base.Initialize();
            dte = (DTE2) (GetGlobalService(typeof (DTE)));
            dte.Events.SolutionEvents.Opened += UpdateBranchName;
            dte.Events.WindowEvents.WindowActivated += UpdateBranchName;
            dte.Events.DocumentEvents.DocumentOpened += UpdateBranchName;
        }

        private void UpdateBranchName() {
            var fileName = dte.Solution.FileName;
            var fullName = dte.Solution.FullName;
        }

        private void UpdateBranchName(Window gotFocus, Window lostFocus) {
            UpdateBranchName();
        }

        private void UpdateBranchName(Document document) {
            UpdateBranchName();
        }
    }
}