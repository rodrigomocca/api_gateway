using System.Collections.Generic;
using System.Threading.Tasks;

namespace Launcher
{
    internal class Launcher
    {
        private static async Task Main(string[] args)
        {
            //await ReadConfiguration();

            var tasks = new List<Task>
            {
                Task.Run(() => { CustomersAPIServices.Program.Main(new [] { "9001" }); }),
                Task.Run(() => { CustomersAPIServices.Program.Main(new [] { "9002" }); }),

                //Task.Run(() => { ProductsAPIServices.Program.Main(new string[] { }); }),
                Task.Run(() => { APIGateway.Program.Main(new string[] { }); })
            };
            await Task.WhenAll(tasks);
        }
    }
}