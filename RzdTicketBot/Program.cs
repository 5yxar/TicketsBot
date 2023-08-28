using System.Net;
using System.Text;
using ConsoleApp3.Shared;
using Newtonsoft.Json;

namespace ConsoleApp3;

public static class Program
{
    private const string _trainName = "120М";
    private const string ProxyUrl = "http://justapi.info/api/proxylist.php?out=js&country=RU&code=340843618381768";

    private const string RequestUrl =
        "https://ticket.rzd.ru/apib2b/p/Railway/V1/Search/TrainPricing?service_provider=B2B_RZD";

    private static async Task Main()
    {
        var proxyList = GetPublicProxies(ProxyUrl).OrderBy(_ => Guid.NewGuid()).ToList();

        // Create the requestData object
        var requestData = new RequestData
        {
            Origin = "2000000",
            Destination = "2024510",
            DepartureDate = DateTime.Parse("2023-08-25T00:00:00"),
            TimeFrom = 0,
            TimeTo = 24,
            CarGrouping = "DontGroup",
            GetByLocalTime = true,
            SpecialPlacesDemand = "StandardPlacesAndForDisabledPersons"
        };
        // Convert requestData to JSON
        var json = JsonConvert.SerializeObject(requestData);
        
        // Create the request content
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        
        try
        {
            foreach (var proxy in proxyList)
            {
                // Создайте экземпляр объекта HttpClientHandler для настройки прокси
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                
                // Настройте прокси в HttpClientHandler
                httpClientHandler.Proxy = proxy;
                httpClientHandler.UseProxy = true;

                // if (!await IsProxyReachable(httpClientHandler))
                // {
                //     Console.WriteLine("Прокси {address} недоступен", proxy.Address);
                //     continue;
                // }

                // Создайте экземпляр объекта HttpClient с HttpClientHandler
                var httpClient = new HttpClient(httpClientHandler);

                try
                {
                    // Send the POST request and get the response
                    var response = await httpClient.PostAsync(RequestUrl, content);
                    
                    // Read the response content as a string
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the response content to a Root object
                    var root = JsonConvert.DeserializeObject<Root>(responseContent);
                    
                    // var a = GetMapTrain(root);
                    
                    // Проверьте статус ответа
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Ответ через прокси {0}: {1}", proxy.Address, responseBody);
                        // Освободите ресурсы HttpClient
                        httpClient.Dispose();
                        // Use the root object as needed
                        Console.WriteLine($"Origin: {root.OriginStationCode}");
                        Console.WriteLine($"Destination: {root.DestinationStationCode}");
                        // Access other properties of the Root object as needed
                        return;
                    }
                }
                catch 
                {
                    httpClient.Dispose();
                }

                // Ensure the request was successful
                // response.EnsureSuccessStatusCode();

               

                // Console.WriteLine("Не удалось выполнить запрос через прокси {0}. Код статуса: {1}", proxy.Address,
                //     response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при выполнении запроса: " + ex.Message);
        }
    }

    // private static object GetMapTrain(Root root)
    // {
    //     var a = root.Trains.FirstOrDefault(x => x.TrainName == _trainName).CarGroups;
    // }

    static async Task<bool> IsProxyReachable(HttpClientHandler httpClientHandler)
    {
        try
        {
            // Создайте экземпляр объекта HttpClient с HttpClientHandler
            using (HttpClient httpClient = new HttpClient(httpClientHandler))
            {
                // Отправьте GET-запрос к проверочному URL
                HttpResponseMessage response = await httpClient.GetAsync("http://www.example.com");

                // Проверьте статус ответа
                return response.IsSuccessStatusCode;
            }
        }
        catch
        {
            return false;
        }
    }

    static List<WebProxy> GetPublicProxies(string url)
    {
        var proxyStrings = new List<Proxy>();

        try
        {
            // Загрузите JSON-строку с указанного URL
            using (var client = new WebClient())
            {
                // string json = client.DownloadString(url);

                string json =
                    @"[{""host"":""45.8.211.90"",""ip"":""45.8.211.90"",""port"":""80"",""lastseen"":511,""delay"":100,""cid"":""524925"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":""Moscow
Oblast"",""checks_up"":""988"",""checks_down"":""7"",""anon"":""1"",""http"":""1"",""ssl"":""0"",""socks4"":""0"",""socks5"":""0""},{""host"":""158.160.56.149"",""ip"":""158.160.56.149"",""port"":""8080"",""lastseen"":2580,""delay"":220,""cid"":""524901"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":""Moscow"",""checks_up"":""1986"",""checks_down"":""1101"",""anon"":""4"",""http"":""1"",""ssl"":""1"",""socks4"":""0"",""socks5"":""0""},{""host"":""45.136.244.245"",""ip"":""45.136.244.245"",""port"":""44531"",""lastseen"":3168,""delay"":1520,""cid"":""524901"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":""Moscow"",""checks_up"":""90"",""checks_down"":""82"",""anon"":""4"",""http"":""0"",""ssl"":""0"",""socks4"":""1"",""socks5"":""0""},{""host"":""45.8.211.110"",""ip"":""45.8.211.110"",""port"":""80"",""lastseen"":4654,""delay"":100,""cid"":""524925"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":""Moscow
Oblast"",""checks_up"":""975"",""checks_down"":""13"",""anon"":""1"",""http"":""1"",""ssl"":""0"",""socks4"":""0"",""socks5"":""0""},{""host"":""45.8.211.64"",""ip"":""45.8.211.64"",""port"":""80"",""lastseen"":4656,""delay"":100,""cid"":""524925"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":""Moscow
Oblast"",""checks_up"":""1005"",""checks_down"":""6"",""anon"":""1"",""http"":""1"",""ssl"":""0"",""socks4"":""0"",""socks5"":""0""},{""host"":""185.174.138.19"",""ip"":""185.174.138.19"",""port"":""80"",""lastseen"":4657,""delay"":120,""cid"":""524925"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":""Moscow
Oblast"",""checks_up"":""7747"",""checks_down"":""121"",""anon"":""1"",""http"":""1"",""ssl"":""0"",""socks4"":""0"",""socks5"":""0""},{""host"":""185.15.172.212"",""ip"":""185.15.172.212"",""port"":""3128"",""lastseen"":4677,""delay"":4220,""cid"":""2017370"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":"""",""checks_up"":""6082"",""checks_down"":""3614"",""anon"":""4"",""http"":""0"",""ssl"":""1"",""socks4"":""0"",""socks5"":""0""},{""host"":""82.146.39.160"",""ip"":""82.146.39.160"",""port"":""3128"",""lastseen"":4764,""delay"":740,""cid"":""524901"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":""Moscow"",""checks_up"":""155"",""checks_down"":""268"",""anon"":""4"",""http"":""0"",""ssl"":""1"",""socks4"":""0"",""socks5"":""0""},{""host"":""185.221.160.0"",""ip"":""185.221.160.0"",""port"":""80"",""lastseen"":4776,""delay"":240,""cid"":""524925"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":""Moscow
Oblast"",""checks_up"":""704"",""checks_down"":""5"",""anon"":""1"",""http"":""1"",""ssl"":""0"",""socks4"":""0"",""socks5"":""0""},{""host"":""31.12.75.145"",""ip"":""31.12.75.145"",""port"":""80"",""lastseen"":4777,""delay"":160,""cid"":""2017370"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":"""",""checks_up"":""138"",""checks_down"":""0"",""anon"":""1"",""http"":""1"",""ssl"":""0"",""socks4"":""0"",""socks5"":""0""},{""host"":""185.221.160.176"",""ip"":""185.221.160.176"",""port"":""80"",""lastseen"":4895,""delay"":140,""cid"":""524925"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":""Moscow
Oblast"",""checks_up"":""10058"",""checks_down"":""128"",""anon"":""1"",""http"":""1"",""ssl"":""0"",""socks4"":""0"",""socks5"":""0""},{""host"":""31.12.75.183"",""ip"":""31.12.75.183"",""port"":""80"",""lastseen"":4895,""delay"":100,""cid"":""2017370"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":"""",""checks_up"":""345"",""checks_down"":""2"",""anon"":""1"",""http"":""1"",""ssl"":""0"",""socks4"":""0"",""socks5"":""0""},{""host"":""185.221.160.60"",""ip"":""185.221.160.60"",""port"":""80"",""lastseen"":4897,""delay"":480,""cid"":""524925"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":""Moscow
Oblast"",""checks_up"":""4260"",""checks_down"":""69"",""anon"":""1"",""http"":""1"",""ssl"":""0"",""socks4"":""0"",""socks5"":""0""},{""host"":""91.238.211.110"",""ip"":""91.238.211.110"",""port"":""8080"",""lastseen"":5116,""delay"":900,""cid"":""2017370"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":"""",""checks_up"":""242"",""checks_down"":""170"",""anon"":""1"",""http"":""1"",""ssl"":""0"",""socks4"":""0"",""socks5"":""0""},{""host"":""212.41.1.163"",""ip"":""212.41.1.163"",""port"":""1080"",""lastseen"":6311,""delay"":1420,""cid"":""524901"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":""Moscow"",""checks_up"":""113"",""checks_down"":""234"",""anon"":""4"",""http"":""0"",""ssl"":""0"",""socks4"":""0"",""socks5"":""1""},{""host"":""45.8.211.113"",""ip"":""45.8.211.113"",""port"":""80"",""lastseen"":10533,""delay"":100,""cid"":""524925"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":""Moscow
Oblast"",""checks_up"":""958"",""checks_down"":""10"",""anon"":""1"",""http"":""1"",""ssl"":""0"",""socks4"":""0"",""socks5"":""0""},{""host"":""62.33.207.202"",""ip"":""62.33.207.202"",""port"":""80"",""lastseen"":10797,""delay"":780,""cid"":""493702"",""country_code"":""RU"",""country_name"":""Russian
Federation"",""city"":""Mikhaylovsk"",""checks_up"":""8975"",""checks_down"":""7885"",""anon"":""1"",""http"":""1"",""ssl"":""0"",""socks4"":""0"",""socks5"":""0""}]";
                // Десериализуйте JSON-строку в список прокси-серверов
                proxyStrings = JsonConvert.DeserializeObject<List<Proxy>>(json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при загрузке списка прокси: " + ex.Message);
        }

        var proxyList2 = proxyStrings?.Select(proxyString => new WebProxy(proxyString.Host, proxyString.Port)).ToList();

        return proxyList2;
    }
}