namespace Util.Common
{
    public class Endpoint
    {
        public Endpoint(string hostName)
        {
            var parts = hostName.Split(':');
            Name = parts[0];
            Port = int.Parse(parts[1]);
        }

        public string Name { get; set; }
        public int Port { get; set; }
    }
}
