using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Tranzact.Search.Managers;
using Tranzact.Search.Models;

namespace Tranzact.Search
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Startup.Start();
            Console.WriteLine("*******************************************************");
            Console.WriteLine("Batalla de popularidad de dos lenguajes de programación");
            Console.WriteLine("*******************************************************");

            if (args.Length == 0)
            {
                Console.WriteLine("Ingrese los dos lenguajes para la batalla.");
                string input = Console.ReadLine();
             
                args = Utility.FixQuote(input);
            }
            using (var serviceScope = Startup.Host.Services.CreateScope())
            {
                var service = serviceScope.ServiceProvider;
                var figth = service.GetRequiredService<FigthManager<FigthResponse>>();
                var response = await figth.FigthResultAsync(args);
                Console.WriteLine(response.Mensaje);
            }

            Console.ReadKey();

        }
    }
}
