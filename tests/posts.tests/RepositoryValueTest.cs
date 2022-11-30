using posts.Services;

namespace posts.tests
{
    public class RepositoryValueTests
    {
        [Fact]
        public void Can_Store_ValueTypes()
        {
            Optional<int> v = 123;

            Assert.True(v.HasValue);
            Assert.Equal(123, v.Value);
        }
        
        [Fact]
        public void NonNullable_ValueTypes_Always_Have_Value()
        {
            var v = Optional<int>.Empty;

            Assert.True(v.HasValue);
        }

        [Fact]
        public void Nullable_ValueTypes_Can_Be_Empty()
        {
            var v = Optional<int?>.Empty;

            Assert.False(v.HasValue);
        }
        
        [Fact]
        public void ReferenceTypes_Can_Be_Empty()
        {
            var v = Optional<int?>.Empty;

            Assert.False(v.HasValue);
        }
        
        [Fact]
        public void ReferenceTypes_Can_Be_NonEmpty()
        {
            Optional<object> v = new object();

            Assert.True(v.HasValue);
        }
    }
}