using System.Data.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

using BusinessLogic.Options;
using BusinessLogic.Services;
using Data.Data.EF.Context;
using Data.Data.EF.Output;

namespace BusinessLogicTest
{
    public class DatabaseTest
    {
        private NorthwindContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Northwind;Trusted_Connection=True;Integrated Security=SSPI;TrustServerCertificate=True;").Options;
            var dbContext = new NorthwindContext(options);
            return dbContext;
        }

        [Fact]
        public void It_should_add_a_product_successfully_into_data_store()
        {
            //Arrange
            var dbContext = CreateDbContext();            
            var dati = dbContext
                   .Categories
                   .Take(5)
                   .AsEnumerable();
            //Assert
            Assert.True(dati.Count() == 5);
            //Clean up
            dbContext.Dispose();
        }

        [Fact]
        public async Task It_should_add_a_product_successfully_into_data_store_2()
        {
            var mockSet = new Mock<Microsoft.EntityFrameworkCore.DbSet<Category>>();

            var mockContext = new Mock<NorthwindContext>();
            var options = new Mock<IOptions<BusinessOptions>>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var service = new BusinessServices(mockContext.Object, options.Object);
            var data = await service.GetData();

            Assert.True(data.Count() == 1);
        }
        [Fact]
        public async Task It_should_add_a_product_successfully_into_data_store_3()
        {
            var mockSet = new Mock<Microsoft.EntityFrameworkCore.DbSet<Category>>();

            var mockContext = new Mock<NorthwindContext>();
            var options = new Mock<IOptions<BusinessOptions>>();

            var data = new List<Category>
            {
                (new Mock<Category>()).Object,
                (new Mock<Category>()).Object,
                (new Mock<Category>()).Object,
                (new Mock<Category>()).Object,
                (new Mock<Category>()).Object,
                (new Mock<Category>()).Object
            }.AsQueryable();

            mockSet.As<IEnumerable<Category>>()
                .Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator());

            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var service = new BusinessServices(mockContext.Object, options.Object);
            var result = await service.GetData();

            Assert.True(result.Count() == 6);
        }


        [Fact]
        public async Task DapperTest()
        {
            var mockSet = new Mock<Microsoft.EntityFrameworkCore.DbSet<Category>>();

            var mockContext = new Mock<NorthwindContext>();
            var options = new Mock<IOptions<BusinessOptions>>();
            mockContext.Setup(m => m.Database.GetDbConnection()).Returns((new Mock<DbConnection>()).Object);

            var data = new List<Category>
            {
                (new Mock<Category>()).Object,
                (new Mock<Category>()).Object,
                (new Mock<Category>()).Object,
                (new Mock<Category>()).Object,
                (new Mock<Category>()).Object,
                (new Mock<Category>()).Object
            }.AsQueryable();

            mockSet.As<IEnumerable<Category>>()
                .Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator());

            var service = new BusinessServices(mockContext.Object, options.Object);
            var result = await service.GetCategories();

            Assert.True(result.Count() == 6);
        }

    }
}