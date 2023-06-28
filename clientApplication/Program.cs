using softaware.Authentication.Hmac.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace clientApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async () => await RunAsync());
            Console.ReadLine();
        }


        static async Task RunAsync()
        {
            try
            {
                Console.WriteLine("Calling the backend API");
                //  string apiBaseAddress = "https://localhost:44391/api/hello";
                string apiBaseAddress = "https://localhost:7172/customer";
                string appId = "4d53bce03ec34c0a911182d4c228ee6c";
                string apiKey = "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=";
                var customDelegateHandler = new ApiKeyDelegatingHandler(appId, apiKey)
                {
                    InnerHandler = new HttpClientHandler()
                };
                //  ApiKeyDelegatingHandler client = new ApiKeyDelegatingHandler();
                HttpClient _client = new HttpClient(customDelegateHandler);
                //HttpClient _client = new HttpClient(new ApiKeyDelegatingHandler(appId, apiKey));
                //   HttpClient _client = new HttpClient();

                HttpResponseMessage response = await _client.GetAsync(apiBaseAddress);
                Console.WriteLine("sdkfghldsfg");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseString);
                }
                else
                {
                    Console.WriteLine("Failed to call the api end point");
                }
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }



    }
}
