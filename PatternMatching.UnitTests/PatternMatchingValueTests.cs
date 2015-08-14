using System;
using PatternMatching.UnitTests.TestClasses;
using PatternMatchingTests.TestClasses;
using Xunit;

namespace PatternMatching.UnitTests
{
    public class PatternMatchingValueTests
    {
        private class Vehicle { }
        private class Car : Vehicle { }

        [Fact]
        public void With_Returns_Value_When_Value_Condition_Is_Met()
        {
            bool result = "Test".Match<string, bool>()
                .With("Test", true)
                .Finally(false);

            Assert.True(result);
        }

        [Fact]
        public void Finally_Value_Returns_If_No_Condition_Met()
        {
            int result = "Test".Match<string, int>()
                .With("Other Value", 1)
                .Finally(2);

            Assert.Equal(2, result);
        }

        [Fact]
        public void Finally_Returns_Alternative_If_Candidate_Is_Null()
        {
            string str = null;
            string result = str.Match<string, string>()
                .Finally("Test");

            Assert.Equal("Test", result);
        }

        [Fact]
        public void Finally_Returns_Alternative_If_No_With_Statements_Used()
        {
            const string str = "Valid Value";
            string result = str.Match<string, string>()
                .Finally("Test");

            Assert.Equal("Test", result);
        }

        [Fact]
        public void With_Ignores_Later_Conditions_Once_A_Condition_Is_Met()
        {
            bool result = "Test".Match<string, bool>()
                .With("Test", true)
                .With("Test", false)
                .Finally(false);

            Assert.True(result);
        }

        [Fact]
        public void WithType_Value_Used_If_Condition_Met()
        {
            Vehicle vehicle = new Car();
            int result = vehicle.Match<Vehicle, int>()
                .WithType<Car>(2)
                .Finally(0);

            Assert.Equal(2, result);
        }

        [Fact]
        public void WithType_Value_Not_Used_If_Condition_Not_Met()
        {
            Vehicle vehicle = new Vehicle();
            int result = vehicle.Match<Vehicle, int>()
                .WithType<Car>(2)
                .Finally(0);

            Assert.Equal(0, result);
        }

        [Fact]
        public void WithRange_Value_Used_If_Condition_Met()
        {
            string result = (3.5).Match<double, string>()
                .WithRange(0.0, 5.0, "In Range")
                .Finally("Not in range");

            Assert.Equal("In Range", result);
        }

        [Fact]
        public void WithRange_Value_Used_When_Value_Equals_LowInclusive()
        {
            string result = (0.0).Match<double, string>()
                .WithRange(0.0, 5.0, "In Range")
                .Finally("Not in range");

            Assert.Equal("In Range", result);
        }

        [Fact]
        public void WithRange_Value_Used_When_Value_Equals_HighExclusive()
        {
            string result = (5.0).Match<double, string>()
                .WithRange(0.0, 5.0, "In Range")
                .Finally("Not in Range");

            Assert.Equal("Not in Range", result);
        }

        [Fact]
        public void WithRange_Value_Used_When_Value_Near_HighExclusive()
        {
            string result = (4.999).Match<double, string>()
                .WithRange(0.0, 5.0, "In Range")
                .Finally("Not in Range");

            Assert.Equal("In Range", result);
        }


        [Fact]
        public void WithRange_Value_Not_Used_If_Condition_Met()
        {
            string result = (3.5).Match<double, string>()
                .WithRange(5.0, 7.5, "In Range")
                .Finally("Not in Range");

            Assert.Equal("Not in Range", result);
        }

        [Fact]
        public void With_Value_Used_If_Condition_Met()
        {
            int result = 'A'.Match<char, int>()
                .With(new [] { 'A', 'B', 'C' }, 1)
                .Finally(2);

            Assert.Equal(1, result);
        }

        [Fact]
        public void With_Value_Not_Used_If_Condition_Not_Met()
        {
            int result = 'D'.Match<char, int>()
                .With(new [] { 'A', 'B', 'C' }, 1)
                .Finally(2);

            Assert.Equal(2, result);
        }

        [Fact]
        public void With_Value_Used_If_Predicate_Condition_Met()
        {
            bool isEven = 2.Match<int, bool>()
                .With(n => n % 2 == 0, true)
                .Finally(false);

            Assert.True(isEven);
        }

        [Fact]
        public void With_Value_Not_Used_If_Predicate_Condition_Not_Met()
        {
            bool isEven = 3.Match<int, bool>()
                .With(n => n % 2 == 0, true)
                .Finally(false);

            Assert.False(isEven);
        }

        [Fact]
        public void With_Predicate_Is_Ignored_If_Input_Is_Null()
        {
            string animal = null;

            string noise = animal.Match<string, string>()
                .With("Fish", "Splash")
                .With(a => a.StartsWith("Walru"), "Arf!")
                .Finally("Unknown");

            Assert.Equal("Unknown", noise);
        }

        [Fact]
        public void With_Mapping_Used_If_Condition_Met()
        {
            double value = "Pi".Match<string, double>()
                .With("Pi", s => Math.PI)
                .Finally(0.0);

            Assert.Equal(Math.PI, value);
        }

        [Fact]
        public void With_Mapping_Not_Used_If_Condition_Not_Met()
        {
            double value = "Pi".Match<string, double>()
                .With("e", s => Math.E)
                .Finally(0.0);

            Assert.Equal(0.0, value);
        }

        [Fact]
        public void ArgumentNullException_Thrown_If_Map_Function_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
                "Pi".Match<string, double>()
                    .With("e", null)
                    .Finally(0.0));
        }

        [Fact]
        public void WithNull_Returns_Value_If_Pattern_Matches()
        {
            string value = null;
            bool isNull = value.Match<string, bool>()
                .WithNull(true)
                .Finally(false);

            Assert.True(isNull);
        }

        [Fact]
        public void WithNull_Doesnt_Return_Value_If_Pattern_Doesnt_Match()
        {
            string value = "Test";
            bool isNull = value.Match<string, bool>()
                .WithNull(true)
                .Finally(false);

            Assert.False(isNull);
        }

        [Fact]
        public void With_Can_Match_Two_IEquatable_Objects()
        {
            var apple1 = new Fruit("apple");
            var apple2 = new Fruit("apple");

            string result = apple1.Match<Fruit, string>()
                .With(apple2, "Match found")
                .Finally("No Match");

            Assert.Equal("Match found", result);
        }
    }
}
