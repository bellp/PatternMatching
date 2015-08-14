# PatternMatching

If you're familiar with any funcitonal programming language, you've probably run into pattern matching. Unfortunately C# doesn't support it (yet), but I've been able to write my own API to support it. It's still not as great as a real functional language's implementation, but can still be useful in some cases.

Example:


// Matching a Fruit value to a string value
string description = fruit.Match<Fruit, string>()
    .With(f => f.Description.Length > 20, "That's a long description")
    .With(new Apple("Granny Smith"), "My favorite :)")
    .WithType<Apple>("An apple")
    .WithType<Banana>("A banana")
    .WithNull("Unsure what type of fruit.")
    .Finally("Huh?");
  
  
// Matching a value with an action
42.Match()
    .With(n => n % 2 != 0, s => Console.WriteLine("Odd"))
    .With(42, s => Console.WriteLine("The meaning of life, the universe, and everything."))
    .Finally(s => {/* do something else */});
