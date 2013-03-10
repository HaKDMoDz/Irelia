using System;
using System.IO;

namespace Demacia.Services
{
    public static class FileService
    {
        public static string GetRelativePath(string pathTo, string targetPath)
        {
            Uri pathToUri;
            if (!Uri.TryCreate(pathTo, UriKind.Absolute, out pathToUri))
                return targetPath;

            Uri targetPathUri;
            if (!Uri.TryCreate(targetPath, UriKind.Absolute, out targetPathUri))
                return targetPath;

            string relativePath = pathToUri.MakeRelativeUri(targetPathUri).ToString();
            return relativePath.Replace('/', Path.DirectorySeparatorChar);
        }
    }
}
