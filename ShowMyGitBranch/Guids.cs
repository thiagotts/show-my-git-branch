// Guids.cs
// MUST match guids.h

using System;

namespace ShowMyGitBranch {
    internal static class GuidList {
        public const string guidShowMyGitBranchPkgString = "aafd1434-3bc5-43bd-bec9-9b51837136d7";
        public const string guidShowMyGitBranchCmdSetString = "d2da42c2-8daa-4f75-a49b-8546eb7c5856";

        public static readonly Guid guidShowMyGitBranchCmdSet = new Guid(guidShowMyGitBranchCmdSetString);
    };
}