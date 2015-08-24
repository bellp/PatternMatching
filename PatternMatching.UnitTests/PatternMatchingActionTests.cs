using PatternMatching.UnitTests.TestClasses;
using PatternMatchingTests.TestClasses;
using Xunit;

namespace PatternMatching.UnitTests
{
    public class PatternMatchingActionTests
    {
        [Fact]
        public void No_Matching_Actions_Invoked_After_Deferred_Action_Invoked()
        {
            bool patternMatched = false;
            int matchCount = 0;
            "Apple".Match()
                .With("Apple")
                .With("Orange")
                .With("Apple", f =>
                {
                    ++matchCount;
                    patternMatched = true;
                })
                .With("Apple", f => ++matchCount)
                .Finally(s => ++matchCount);

            Assert.Equal(1, matchCount);
            Assert.True(patternMatched);
        }

        [Fact]
        public void Finally_Action_Invoked_If_Previous_Match_Occurred_With_No_Action()
        {
            bool patternMatched = false;
            "Apple".Match()
                .With("Apple")
                .Finally(x => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void NonMatching_WithRange_Can_Chain_To_Matching_With()
        {
            bool patternMatched = false;
            3.Match()
                .WithRange(5, 10)
                .With(3, n => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void Matching_With_Can_Chain_To_NonMatching_WithRange()
        {
            bool patternMatched = false;
            3.Match()
                .With(5)
                .WithRange(1, 4, n => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void NonMatching_With_Can_Chain_To_Matching_With()
        {
            bool patternMatched = false;
            "Red Delicious".Match()
                .With("Granny Smith")
                .With("Red Delicious", a => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void Matching_With_Can_Chain_To_NonMatching_With()
        {
            bool patternMatched = false;
            "Granny Smith".Match()
                .With("Granny Smith")
                .With("Red Delicious", a => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void WithNull_Can_Chain_To_Matching_With()
        {
            string value = null;
            bool patternMatched = false;
            value.Match()
                .WithNull()
                .With("Red Delicious", a => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void Matching_With_Can_Chain_To_NonMatching_WithNull()
        {
            const string value = "Red Delicious";
            bool patternMatched = false;
            value.Match()
                .With("Red Delicious")
                .WithNull(() => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void Matching_With_Can_Chain_To_WithNull()
        {
            bool patternMatched = false;
            "Granny Smith".Match()
                .With("Granny Smith")
                .With("Red Delicious", a => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void With_Can_Chain_To_WithType()
        {
            bool patternMatched = false;
            "Red Delicious".Match()
                .With("Granny Smith")
                .WithType<string>(a => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void WithType_Can_Chain_To_With()
        {
            bool patternMatched = false;
            "Red Delicious".Match()
                .WithType<string>()
                .With("Granny Smith", s => patternMatched = true);

            Assert.True(patternMatched);
        }

        [Fact]
        public void Chained_Withs_Return_True_If_Middle_Matches()
        {
            bool patternMatched = false;
            "Apple".Match()
                .With("Banana")
                .With("Apple")
                .With("Orange")
                .WithNull(() => patternMatched = true);

            Assert.True(patternMatched);
        }

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

            Assert.True(patternMatched);
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