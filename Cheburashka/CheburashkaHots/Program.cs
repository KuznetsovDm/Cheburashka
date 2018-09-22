using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheburashkaHots
{
    class Program
    {
        static void Main()
        {
            new CheburashkaHost()
                     .Configure(options => { options.Port = 10000; })
                     .RunAsync()
                     .GetAwaiter()
                     .GetResult();
        }
    }
}
