using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace XmlParser.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var filterElements = new List<string>();
            string filePath = String.Empty;
            var clientId = Guid.NewGuid().ToString();

            if (args.Length > 1)
            {
                filePath = args[0];
                if (args.Length > 1)
                {
                    for (int i = 1; i < args.Length; i++)
                    {
                        filterElements.Add(args[i]);
                    }
                }
            } else
            {
                Console.WriteLine(@"Please provide file path and filter elements arguments (Ex. dotnet run -- C:\Users\Adis\Desktop\XMLPlay\congree.xml p li)\n");
                return;
            }

            Console.WriteLine($"\nProcessing: ${args[0]} Filter Elements: {String.Join(';', filterElements)}");

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddTransient<IXmlParserService, XmlParserService>();
                }).UseConsoleLifetime();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var myService = services.GetRequiredService<IXmlParserService>();
                    Console.WriteLine($"\nProcessing finished!\n {myService.ProcessXML(args[0], clientId, String.Join(';', filterElements)).Result}");
                    Console.WriteLine("*********************************************\n");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred.");
                    throw;
                }
               
            }

            host.RunAsync();
        }

        public interface IXmlParserService
        {
            Task<string> ProcessXML(string xmlFilePath, string clientId, string filterElements);
        }

        // Need this in order to use  IHttpClientFactory in a console app
        public class XmlParserService : IXmlParserService
        {
            private readonly IHttpClientFactory _clientFactory;
            public XmlParserService(IHttpClientFactory clientFactory)
            {
                _clientFactory = clientFactory;
            }

            public async Task<string> ProcessXML(string xmlFilePath, string clientId, string filterElements)
            {
                var connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:44394/notificationHub")
                    .Build();

                connection.On<string>(clientId, (message) =>
                {
                    Console.WriteLine($"{message}");
                });

                await connection.StartAsync();

                var fileName = System.IO.Path.GetFileName(xmlFilePath);
                var requestUri = $"https://localhost:44394/XmlProcessor/process?fileName={fileName}&clientId={clientId}&filterElements={filterElements}";
                var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

                StreamReader stream = null;
                try
                {
                    stream = new StreamReader(xmlFilePath);

                    request.Content = new StreamContent(stream.BaseStream);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    var client = _clientFactory.CreateClient();
                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync(); ;
                    }
                    else
                    {
                        return $"StatusCode: {response.StatusCode}";
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    stream.Dispose();
                    await connection.DisposeAsync();
                }
            
            }
        }
    }
}
