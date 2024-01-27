using DMinecraft.PhysicalClient.Scheduling.Coroutines.Util;
using Serilog;
using System.Collections;
using System.Reflection;

namespace DMinecraft.PhysicalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Assembly.GetAssembly(typeof(Program)));
            using (var app = new App(args))
                app.Run();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
