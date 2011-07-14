using System;
using System.Collections.Generic;
using System.IO;

namespace Coypu.Tests.TestDoubles
{
    internal class SpyFileSystem : FileSystem
    {
        private readonly IList<SavedStream> savedStreams = new List<SavedStream>();

        internal IEnumerable<SavedStream> SavedStreams
        {
            get { return savedStreams; }
        }

        public void SaveAs(Stream stream, string path)
        {
            savedStreams.Add(new SavedStream(stream,path));
        }
    }

    internal class SavedStream
    {
        public Stream Stream { get; private set; }
        public string Path { get; private set; }

        public SavedStream(Stream stream, string path)
        {
            Stream = stream;
            Path = path;
        }
    }
}