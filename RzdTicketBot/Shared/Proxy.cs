namespace ConsoleApp3.Shared;

class Proxy
{
    public string Host { get; set; }
    public string IP { get; set; }
    public int Port { get; set; }
    public int LastSeen { get; set; }
    public int Delay { get; set; }
    public string CID { get; set; }
    public string CountryCode { get; set; }
    public string CountryName { get; set; }
    public string City { get; set; }
    public string ChecksUp { get; set; }
    public string ChecksDown { get; set; }
    public string Anon { get; set; }
    public string HTTP { get; set; }
    public string SSL { get; set; }
    public string Socks4 { get; set; }
    public string Socks5 { get; set; }
}