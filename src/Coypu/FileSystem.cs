using System.IO;

namespace Coypu
{
    internal interface FileSystem
    {
        void SaveAs(Stream stream, string path);
    }
}