using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.IO
{
    internal class FilePathEnumerable : IEnumerable<string>
    {
        public FilePathEnumerable(string path, string pattern) 
        {
            Path = path;
            Pattern = pattern;
        }

        public required string Path { get; init; }
        public required string Pattern { get; init; }

        public IEnumerator<string> GetEnumerator()
        {
            var queue = new Queue<string>();
            queue.Enqueue(Path);
            IEnumerator<string> subDirectories = Directory.EnumerateDirectories(Path).GetEnumerator();

            do 
            {
                var directory = queue.Dequeue();
                
                foreach (var item in Directory.EnumerateFiles(directory))
                {
                    yield return item;
                }

                if (!subDirectories.MoveNext())
                {

                }
                else
                {
                    queue.Enqueue(subDirectories.Current);
                }

            } while (queue.Count > 0);
            while (queue.TryDequeue(out string? directory))
            {
                Directory.EnumerateDirectories(directory);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
