using Microsoft.Extensions.Hosting;
using System.Threading;

namespace ApiLauncher.Models
{
    public class Execution
    {
        public string Host { get; set; }

        public Microservice Microservice { get; set; }

        public string Id { get; set; }

        public int Port { get; set; }

        public IHost AppHost { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }

        public void Cancel()
        {
            CancellationTokenSource.Cancel();
        }

        public void Run()
        {
            var token = CancellationTokenSource.Token;
            AppHost.RunAsync(token);
        }
    }
}