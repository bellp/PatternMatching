# Pattern Matching

If you're familiar with any funcitonal programming language, you've probably run into pattern matching. Unfortunately C# doesn't support it (yet), but I've been able to write my own API to support it. It's still not as great as a real functional language's implementation, but can still be useful in some cases.

Example:

private static string DescribeFruit(Fruit fruit)
{
    // Matching a Fruit value to a string value
    return fruit.Match<Fruit, string>()
        .With(f => f.Description.Length > 20, "That's a long description")
        .With(new Apple("Granny Smith"), "My favorite :)")
        .WithType<Apple>("An apple")
        .WithType<Banana>("A banana")
        .WithNull("Not a fruit")
        .Finally("Huh?");
}
  
  
// Matching a value with an action
42.Match()
    .With(n => n % 2 != 0, n => Console.WriteLine("{0} is Odd", n))
    .With(42, s => Console.WriteLine("The meaning of life, the universe, and everything"))
    .WithRange(1, 10, n => Console.WriteLine("{0} is even and between 1 and 10", n))
    .With(64)
    .With(128)
    .With(256, n => Console.WriteLine("{0} is 64, 128, or 256", n))
    .Finally(s => { throw new Exception("Oh no!"); });
