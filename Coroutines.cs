using System;

namespace Coroutines
{
    public class Coroutines
    {
        public static async void FirstCoroutine(Coordinator coordinator)
        {
            Console.WriteLine("Starting FirstCoroutine");
            Console.WriteLine("Yielding from FirstCoroutine…");

            await coordinator;

            Console.WriteLine("Returned to FirstCoroutine");
            Console.WriteLine("Yielding from FirstCoroutine again…");

            await coordinator;

            Console.WriteLine("Returned to FirstCoroutine again");
            Console.WriteLine("Finished FirstCoroutine");
        } 

        public static async void SecondCoroutine(Coordinator coordinator)
        {
            Console.WriteLine("Starting SecondCoroutine");
            Console.WriteLine("Yielding from SecondCoroutine…");

            await coordinator;

            Console.WriteLine("Returned to SecondCoroutine");
            Console.WriteLine("Yielding from SecondCoroutine again…");

            await coordinator;

            Console.WriteLine("Returned to SecondCoroutine again");
            Console.WriteLine("Finished SecondCoroutine");
        } 

        public static async void ThirdCoroutine(Coordinator coordinator)
        {
            Console.WriteLine("Starting ThirdCoroutine");
            Console.WriteLine("Yielding from ThirdCoroutine…");

            await coordinator;

            Console.WriteLine("Returned to ThirdCoroutine again");
            Console.WriteLine("Finished ThirdCoroutine");
        } 
    }
}