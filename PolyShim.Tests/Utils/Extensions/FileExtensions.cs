using System.IO;

namespace PolyShim.Tests.Utils.Extensions;

internal static class FileExtensions
{
    extension(File)
    {
        public static bool TryDelete(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
