namespace ApiLauncher.Models
{
    public class DownstreamHostAndPort
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public DownstreamHostAndPort(string host, int port)
        {
            Host = host;
            Port = port;
        }
    }
}