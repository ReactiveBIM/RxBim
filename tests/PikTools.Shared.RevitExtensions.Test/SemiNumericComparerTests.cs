namespace PikTools.Shared.RevitExtensions.Test
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using PikTools.Shared.RevitExtensions.Helpers;
    using Xunit;

    public class SemiNumericComparerTests
    {
        private SemiNumericComparer _comparer = new SemiNumericComparer();

        [Fact]
        public void CompareTwoEqualStringWithNumber()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("num 1", "num 1");

            act.Should().NotThrow();
            result.Should().Equals(0);
        }

        [Fact]
        public void CompareTwoNotEqualStringWithNumberButNumbersAreEqual()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("num 1", "1 num");

            act.Should().NotThrow();
            result.Should().Equals(0);
        }

        [Fact]
        public void CompareTwoStringsWithDifferentNumberFirstAreGreater()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("num 2", "1 num");

            act.Should().NotThrow();
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public void CompareTwoStringsWithDifferentNumberSecondAreGreater()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("num 1", "2 num");

            act.Should().NotThrow();
            result.Should().BeNegative();
        }

        [Fact]
        public void CompareTwoEqualStringWithoutNumber()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("num", "num");

            act.Should().NotThrow();
            result.Should().Equals(0);
        }

        [Fact]
        public void CompareTwoStringsFirstAreGreater()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("cba", "abc");

            act.Should().NotThrow();
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public void CompareTwoStringsSecondAreGreater()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("abc", "cba");

            act.Should().NotThrow();
            result.Should().BeNegative();
        }

        [Fact]
        public void CompareTwoEqualStringWithFloatNumber()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("num 1.0", "num 1.0");

            act.Should().NotThrow();
            result.Should().Equals(0);
        }

        [Fact]
        public void CompareTwoNotEqualStringWithFloatNumberButNumbersAreEqual()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("num 1.0", "1.0 num");

            act.Should().NotThrow();
            result.Should().Equals(0);
        }

        [Fact]
        public void CompareTwoStringsWithDifferentFloatNumberFirstAreGreater()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("num 1.2", "1.0 num");

            act.Should().NotThrow();
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public void CompareTwoStringsWithDifferentFloatNumberSecondAreGreater()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("num 1.0", "1.2 num");

            act.Should().NotThrow();
            result.Should().BeNegative();
        }

        [Fact]
        public void CompareTwoEmptyString()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare(string.Empty, string.Empty);

            act.Should().NotThrow();
            result.Should().Equals(0);
        }

        [Fact]
        public void CompareEmptyStringAndNotEmptyStringSecondAreGreater()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare(string.Empty, "num");

            act.Should().NotThrow();
            result.Should().BeNegative();
        }

        [Fact]
        public void CompareStringWithNumberAndWithoutNumberFirstAreGreater()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("1 num", "z");

            act.Should().NotThrow();
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public void CompareStringWithFloatNumberAndWithGreaterNumberSecondAreGreater()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("num 1.1", "num 10");

            act.Should().NotThrow();
            result.Should().BeNegative();
        }

        [Fact]
        public void CompareTwoDifferentStringsWithZeroNumbersLessLengthAreGreater()
        {
            int result = 0;
            Action act = () => result = _comparer.Compare("000", "0000");

            act.Should().NotThrow();
            result.Should().BeNegative();
        }

        [Fact]
        public void CompareStringList()
        {
            var inList = new List<string>
            {
                "num 11",
                "num 1.1",
                "num",
                string.Empty,
                "num 11",
                "0000",
                "num 1.2",
                "num 12",
                "z",
                "1 num",
                "000"
            };

            var expectList = new List<string>
            {
                string.Empty,
                "num",
                "z",
                "000",
                "0000",
                "1 num",
                "num 1.1",
                "num 1.2",
                "num 11",
                "num 11",
                "num 12"
            };

            Action act = () => inList.Sort(_comparer);

            act.Should().NotThrow();
            inList.Should().ContainInOrder(expectList);
        }
    }
}
