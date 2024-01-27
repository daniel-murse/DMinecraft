using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Content
{
    internal class FileContentSource : IContentSource
    {
        public FileContentSource(string path) 
        {
            Path = path;
        }

        public string Path { get; set; }

        public Stream OpenRead()
        {
            return File.OpenRead(Path);
        }
    }
}
