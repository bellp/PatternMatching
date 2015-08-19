using System;
using PatternMatching;
using Sample.Model;

namespace Sample
{
    static class Program
    {
        static void Main()
        {
            Console.WriteLine(DescribeFruit(new Banana("The Velvet Underground and Nico")));
            Console.WriteLine(DescribeFruit(new Apple("Granny Smith")));
            Console.WriteLine(DescribeFruit(new Apple("Washington")));
            Console.WriteLine(DescribeFruit(new Banana("Chiquita")));
            Console.WriteLine(DescribeFruit(new Banana("Chiquita")));
            Console.WriteLine(DescribeFruit(null));
            Console.WriteLine(DescribeFruit(new Fruit("General")));

            // Matching a value with an action
            42.Match()
                .With(n => n % 2 != 0, n => Console.WriteLine("{0} is Odd", n))
                .With(42, s => Console.WriteLine("The meaning of life, the universe, and everything."))
                .WithRange(1, 11, n => Console.WriteLine("{0} is between 1 and 10", n))
                .Finally(s => {/* do something else */});

            Console.ReadLine();
        }

        private static string DescribeFruit(Fruit fruit)
        {
            // Matching a Fruit value to a string value
            return fruit.Match<Fruit, string>()
                .With(f => f.Description.Length > 20, "That's a long description")
                .With(new Apple("Granny Smith"), "My favorite :)")
                .WithType<Apple>("An apple")
                .WithType<Banana>("A banana")
                .WithNull("No fruit at all.")
                .Finally("Huh?");
        }
    }
}
