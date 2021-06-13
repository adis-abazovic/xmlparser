using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace XmlParser.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("START");
            var filterElements = new List<string>();
            string filePath = String.Empty;
            if (args.Length > 0)
            {
                filePath = args[0];

                if (args.Length > 1)
                {
                    for (int i = 1; i < args.Length; i++)
                    {
                        filterElements.Add(args[0]);
                    }
                }
            }

            // Need this in order to use  IHttpClientFactory in a console app
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
                    Console.WriteLine(myService.ProcessXML(args[0], String.Join(';', filterElements)).Result);
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
        Task<string> ProcessXML(string xmlFilePath, string filterElements);
    }

    // Need this in order to use  IHttpClientFactory in a console app
    public class XmlParserService : IXmlParserService
    {
        private readonly IHttpClientFactory _clientFactory;
        public XmlParserService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<string> ProcessXML(string xmlFilePath, string filterElements)
        {
            var fileName = System.IO.Path.GetFileName(xmlFilePath);
            var requestUri = $"https://localhost:44394/XmlProcessor?fileName={fileName}&filterElements={filterElements}";
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
                    return await response.Content.ReadAsStringAsync();
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
            }
            
        }
    }
}
}
