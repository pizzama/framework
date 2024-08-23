using System.Collections.Generic;
using System.IO;

namespace SFramework.Extension
{
    public static class SystemIOExtension
    {
        public static string CreateDirIfNotExists(this string dirFullPath)
        {
            if (!Directory.Exists(dirFullPath))
            {
                Directory.CreateDirectory(dirFullPath);
            }

            return dirFullPath;
        }

        public static void DeleteDirIfExists(this string dirFullPath)
        {
            if (Directory.Exists(dirFullPath))
            {
                Directory.Delete(dirFullPath, true);
            }
        }

        public static void EmptyDirIfExists(this string dirFullPath)
        {
            if (Directory.Exists(dirFullPath))
            {
                Directory.Delete(dirFullPath, true);
            }

            Directory.CreateDirectory(dirFullPath);
        }

        public static bool DeleteFileIfExists(this string fileFullPath)
        {
            if (File.Exists(fileFullPath))
            {
                File.Delete(fileFullPath);
                return true;
            }

            return false;
        }

        public static string CombinePath(this string selfPath, string toCombinePath)
        {
            return Path.Combine(selfPath, toCombinePath);
        }

        public static string GetFileName(this string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public static string GetFileNameWithoutExtend(this string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        public static string GetFileExtendName(this string filePath)
        {
            return Path.GetExtension(filePath);
        }

        public static string GetFolderPath(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Copy Streaming Assets all files to release file.
        /// </summary>
        /// <param name="fromDir"></param>
        /// <param name="toDir"></param>
        public static void DirectoryCopy(
            this string fromDir,
            string toDir,
            List<string> ignoreFiles,
            params string[] ignoreExtensions
        )
        {
            if (!Directory.Exists(fromDir))
            {
                return;
            }

            bool ExistFiles(string filename)
            {
                if (ignoreFiles == null)
                {
                    return false;
                }
                foreach (var ie in ignoreFiles)
                {
                    if (filename.Contains(ie))
                    {
                        return true;
                    }
                }
                return false;
            }

            bool ExistExtensions(string extension)
            {
                if (ignoreExtensions == null)
                {
                    return false;
                }
                foreach (var ie in ignoreExtensions)
                {
                    if (extension.Contains(ie))
                    {
                        return true;
                    }
                }
                return false;
            }

            var directoryInfo = new DirectoryInfo(fromDir);
            foreach (var dir in directoryInfo.GetDirectories())
            {
                dir.Name.DirectoryCopy(
                    $"{toDir}/{dir.Name}",
                    ignoreFiles,
                    ignoreExtensions
                );
            }

            foreach (var file in directoryInfo.GetFiles())
            {
                if (ExistFiles(file.FullName) || ExistExtensions(file.Extension))
                {
                    continue;
                }

                File.Copy($"{fromDir}/{file.Name}", $"{toDir}/{file.Name}", true);
            }
        }

    }
}