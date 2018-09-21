using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheburashkaHots
{
    class Program
    {
        static async void Main(string[] args)
        {
            await new Host()
                     .Configure(options => { options.Port = 10000; })
                     .RunAsync();
        }
    }
}
