using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Airline.AppData.EF;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AirlineDbContext("AirlineDbConnection"))
            {
               foreach(var x in  context.Cities)
                {
                    Console.WriteLine(x.Name);
                }
            }
        }
    }
}
