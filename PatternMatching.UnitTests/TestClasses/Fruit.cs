using System;

namespace PatternMatching.UnitTests.TestClasses
{
    public class Fruit : IEquatable<Fruit>
    {
        private readonly string _name;

        public Fruit(string name)
        {
            _name = name;
        }

        public bool Equals(Fruit other)
        {
            return Equals(_name, other._name);
        }
    }
}
