using System;
using System.Threading.Tasks;

namespace Coroutines
{
    public class Coroutines
    {
        public static async Task<string> FirstCoroutine(Coordinator<string> coordinator, string initialValue)
        {
            Console.WriteLine("Starting FirstCoroutine with initial value {0}", initialValue);
            Console.WriteLine("Yielding from FirstCoroutine…");

            string received = await coordinator.Yield("x1");
            Console.WriteLine("Returned to FirstCoroutine with value {0}", received);
            Console.WriteLine("Yielding from FirstCoroutine again…");

            received = await coordinator.Yield("x2");;

            Console.WriteLine("Returned to FirstCoroutine again with value {0}", received);
            Console.WriteLine("Finished FirstCoroutine");

            return "x3";
        } 

        public static async Task<string> SecondCoroutine(Coordinator<string> coordinator, string initialValue)
        {
            Console.WriteLine("Starting SecondCoroutine with initial value {0}", initialValue);
            Console.WriteLine("Yielding from SecondCoroutine…");

            string received = await coordinator.Yield("y1");
            Console.WriteLine("Returned to SecondCoroutine with value {0}", received);
            Console.WriteLine("Yielding from SecondCoroutine again…");

            received = await coordinator.Yield("y2");

            Console.WriteLine("Returned to SecondCoroutine again with value {0}", received);
            Console.WriteLine("Finished SecondCoroutine");

            return "y3";
        } 

        public static async Task<string> ThirdCoroutine(Coordinator<string> coordinator, string initialValue)
        {
            Console.WriteLine("Starting ThirdCoroutine with initial value {0}", initialValue);
            Console.WriteLine("Yielding from ThirdCoroutine…");

            string received = await coordinator.Yield("z1");

            Console.WriteLine("Returned to ThirdCoroutine again with value {0}", received);
            Console.WriteLine("Finished ThirdCoroutine");

            return "z2";
        } 
    }
}