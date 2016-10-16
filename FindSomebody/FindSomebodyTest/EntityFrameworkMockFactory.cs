using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FindSomebodyTest
{
    /// <summary>
    /// Creates Mocked DbSets to enable mocking away DbContexts.
    /// </summary>
    public static class EntityFrameworkMockFactory
    {
        /// <summary>
        /// Creates a mock for a dbset, allowing queries against the dbSet.
        /// </summary>
        /// <typeparam name="T">Type of the DbSet members.</typeparam>
        /// <param name="data">Data returned by the DbSet.</param>
        /// <returns>Mock of a dbset ready for general operations.</returns>
        public static Mock<DbSet<T>> CreateMockDbSet<T>(IQueryable<T> data)
            where T : class
        {
            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockDbSet.As<IEnumerable<T>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            return mockDbSet;
        }
    }
}