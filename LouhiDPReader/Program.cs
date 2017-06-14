using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouhiDPReader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("LouhiDPReader accepts (and needs) only one argument, the path to the csv file containing Deal data.");
                Console.WriteLine("Please try again.");
                return;
            }

            if (!DealProcessor.FilePathIsOk(args[0]))
            {
                Console.WriteLine(String.Format("The given file path ({0}) is not working.", args[0]));
                Console.WriteLine("Please try again.");
            }

            DealProcessor dp = new DealProcessor();
            dp.ProcessDeals(args[0]);
        }
    }
}
