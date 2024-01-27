using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Content
{
    //encoding concerns, but thats for interfaces or classes extending this one
    internal interface IContentSource
    {
        public Stream OpenRead();
    }
}
