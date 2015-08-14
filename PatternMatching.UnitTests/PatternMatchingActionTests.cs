using System;
using PatternMatching;
using PatternMatching.UnitTests.TestClasses;
using PatternMatchingTests.TestClasses;
using Xunit;

namespace PatternMatchingTests
{
    public class PatternMatchingActionTests
    {
        [Fact]
        public void Constant_With_Should_Invoke_Action_If_Pattern_Matches()
        {
            bool patternMatched = false;
            "Apple".Match()
                .With("Apple", s => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void Constant_With_Should_Not_Invoke_Action_If_Pattern_Doesnt_Match()
        {
            bool patternMatched = false;
            "Apple".Match()
                .With("Orange", s => patternMatched = true);

            Assert.False(patternMatched);
        }

        [Fact]
        public void Finally_Invokes_Action_If_No_Prior_Patterns_Matched()
        {
            bool patternMatched = false;
            "Apple".Match()
                .With("Orange", s => { })
                .Finally(s => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void Finally_Doesnt_Invoke_Action_If_Prior_Pattern_Matched_Already()
        {
            bool patternMatched = false;
            "Apple".Match()
                .With("Apple", s => { })
                .Finally(s => patternMatched = true);

            Assert.False(patternMatched);
        }

        [Fact]
        public void WithType_Should_Invoke_Action_If_Pattern_Matches()
        {
            bool patternMatched = false;
            Fruit fruit = new Apple("apple");
            fruit.Match()
                .WithType<Apple>(f => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void WithType_Shouldnt_Invoke_Action_If_Pattern_Doesnt_Match()
        {
            bool patternMatched = false;
            Fruit fruit = new Fruit("fruit");
            fruit.Match()
                .WithType<Apple>(f => patternMatched = true);

            Assert.False(patternMatched);
        }

        [Fact]
        public void Predicate_With_Should_Invoke_Action_If_Pattern_Matches()
        {
            bool patternMatched = false;
            "Apple".Match()
                .With(s => s.Length == 5, f => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void Predicate_With_Should_Not_Invoke_Action_If_Pattern_Doesnt_Match()
        {
            bool patternMatched = false;
            "Apple".Match()
                .With(s => s.Length == 0, f => patternMatched = true);

            Assert.False(patternMatched);
        }

        [Fact]
        public void Collection_With_Should_Invoke_Action_If_Pattern_Matches()
        {
            bool patternMatched = false;
            "Apple".Match()
                .With(new [] { "Orange", "Apple", "Banana" }, f => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void Collection_With_Should_Not_Invoke_Action_If_Pattern_Doesnt_Match()
        {
            bool patternMatched = false;
            "Apple".Match()
                .With(new [] { "Orange", "Kiwi", "Banana" }, f => patternMatched = true);

            Assert.False(patternMatched);
        }

        [Fact]
        public void WithRange_Should_Invoke_Action_If_Expression_Equals_Lowest_Value()
        {
            bool patternMatched = false;
            (0.0).Match()
                .WithRange(0.0, 5.0, d => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void WithRange_Should_Invoke_Action_If_Expression_Is_Within_Range()
        {
            bool patternMatched = false;
            (3.0).Match()
                .WithRange(0.0, 5.0, d => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void WithRange_Should_Invoke_Action_If_Expression_Is_Near_Highest_Value_In_Range()
        {
            bool patternMatched = false;
            (4.999999).Match()
                .WithRange(0.0, 5.0, d => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void WithRange_Should_Not_Invoke_Action_If_Expression_Equals_Highest_Value_Of_Range()
        {
            bool patternMatched = false;
            (5.0).Match()
                .WithRange(0.0, 5.0, d => patternMatched = true);

            Assert.False(patternMatched);
        }

        [Fact]
        public void WithRange_Should_Not_Invoke_Action_If_Expression_Is_Below_Range()
        {
            bool patternMatched = false;
            (-1.0).Match()
                .WithRange(0.0, 5.0, d => patternMatched = true);

            Assert.False(patternMatched);
        }

        [Fact]
        public void WithRange_Should_Not_Invoke_Action_If_Expression_Is_Above_Range()
        {
            bool patternMatched = false;
            (6.0).Match()
                .WithRange(0.0, 5.0, d => patternMatched = true);

            Assert.False(patternMatched);
        }

        [Fact]
        public void Pattern_Matching_Works_When_Matching_Pattern_Isnt_First()
        {
            bool patternMatched = false;
            "Apple".Match()
                .With("Banana", f => { })
                .With("Apple", f => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void Even_If_Multiple_Patterns_Match_Only_One_Action_Should_Be_Invoked()
        {
            int actionsInvoked = 0;

            "Apple".Match()
                .With("Pear", f => actionsInvoked++)
                .With("Apple", f => actionsInvoked++)
                .WithType<string>(f => actionsInvoked++)
                .With(s => s.Length == 5, f => actionsInvoked++)
                .With("Apple", f => actionsInvoked++);

            Assert.Equal(1, actionsInvoked);
        }

        [Fact]
        public void WithNull_Invokes_Action_If_Pattern_Matches()
        {
            bool actionInvoked = false;
            string value = null;
            value.Match()
                .WithNull(() => actionInvoked = true);

            Assert.True(actionInvoked);
        }

        [Fact]
        public void WithNull_Doesnt_Invoke_Action_If_Pattern_Doesnt_Match()
        {
            bool actionInvoked = false;
            const string value = "Not null";
            value.Match()
                .WithNull(() => actionInvoked = true);

            Assert.False(actionInvoked);
        }

        [Fact]
        public void Can_Match_IEquatable_Objects()
        {
            var apple1 = new Fruit("apple");
            var apple2 = new Fruit("apple");
            bool actionInvoked = false;

            apple1.Match()
                .With(apple2, a => actionInvoked = true);

            Assert.True(actionInvoked);
        }
    }
}