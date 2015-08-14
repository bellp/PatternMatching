# Pattern Matching

If you're familiar with any funcitonal programming language, you've probably run into pattern matching. Unfortunately C# doesn't support it (yet), but I've been able to write my own API to support it. It's still not as great as a real functional language's implementation, but can still be useful in some cases.

Example:


<!-- code formatted by http://manoli.net/csharpformat/ -->
<pre class="csharpcode">
<span class="rem">// Matching a Fruit value to a string value</span>
<span class="kwrd">string</span> description = fruit.Match&lt;Fruit, <span class="kwrd">string</span>&gt;()
    .With(f =&gt; f.Description.Length &gt; 20, <span class="str">"That's a long description"</span>)
    .With(<span class="kwrd">new</span> Apple(<span class="str">"Granny Smith"</span>), <span class="str">"My favorite :)"</span>)
    .WithType&lt;Apple&gt;(<span class="str">"An apple"</span>)
    .WithType&lt;Banana&gt;(<span class="str">"A banana"</span>)
    .WithNull(<span class="str">"Unsure what type of fruit."</span>)
    .Finally(<span class="str">"Huh?"</span>);
  
  
<span class="rem">// Matching a value with an action</span>
42.Match()
    .With(n =&gt; n % 2 != 0, s =&gt; Console.WriteLine(<span class="str">"Odd"</span>))
    .With(42, s =&gt; Console.WriteLine(<span class="str">"The meaning of life, the universe, and everything."</span>))
    .Finally(s =&gt; {<span class="rem">/* do something else */</span>});</pre>
