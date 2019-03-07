using System;

namespace Coroutines
{
    // Following this posts series:
    // https://codeblog.jonskeet.uk/2011/06/22/eduasync-part-13-first-look-at-coroutines-with-async/
    class Program
    {
        static void Main(string[] args)
        {
            var coordinator = new Coordinator<string>(
                Coroutines.FirstCoroutine,
                Coroutines.SecondCoroutine,
                Coroutines.ThirdCoroutine);
            
            string finalResult = coordinator.Start("m1");
            Console.WriteLine("Final result: {0}", finalResult); 

            Console.WriteLine("Press Enter to finish");
            Console.ReadLine();
        }
    }
}
