using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionParser;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string s;
            int n;
            while ((s = Console.ReadLine()) != "")
            {
                Expression func = new Expression(s);

                Console.WriteLine(func.ToString());

                Console.Write("What doing with expression? (0 - calculate, 1 - simplify): ");
                n = Convert.ToInt32(Console.ReadLine());

                switch (n)
                {
                    case 0:
                        Console.WriteLine("Result: " + Convert.ToString(func.Calculate()));
                        break;
                    case 1:
                        Console.WriteLine("Result: " + Convert.ToString(func.Simplify()));
                        break;
                    default:
                        Console.WriteLine("No acts with this number.");
                        break;
                }

            }
            

            Console.ReadKey();
        }
    }
}
