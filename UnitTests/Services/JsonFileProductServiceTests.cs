using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Models;
using System.Linq;

namespace UnitTests.Pages.Product.AddRating
{

    /// <summary>
    /// Class containing unit test cases to JsonFileProductService file
    /// </summary>
    public class JsonFileProductServiceTests
    {
        #region TestSetup

        /// <summary>
        /// Test initialize
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup


        #region AddRating
        /// <summary>
        /// Testing to check if a invalid product null will return false
        /// </summary>
        [Test]
        public void AddRating_Invalid_Product_Null_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating(null, 1);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        ///Testing AddRating for invalid product empty will return false
        /// </summary>
        [Test]
        public void AddRating_Invalid_Product_Empty_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("", 1);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        ///Testing AddRating for invalid null data should return false
        /// </summary>
        [Test]
        public void AddRating_Invalid_Data_Null_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("Not exists", 3);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Testing AddRating for invalid negative rating should return false
        /// </summary>        
        [Test]
        public void AddRating_Invalid_Negative_Rating_Should_Return_False()
        {
            // Arrange
            var productId = TestHelper.ProductService.GetProducts().First().Id;

            // Act
            var result = TestHelper.ProductService.AddRating(productId, -3);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Testing AddRating for Invalid rating greater then 5 should return false
        /// </summary>
        [Test]
        public void AddRating_Invalid_Rating_Greater_Than_Five_Should_Return_False()
        {
            // Arrange
            var productId = TestHelper.ProductService.GetProducts().First().Id;

            // Act
            var result = TestHelper.ProductService.AddRating(productId, 8);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Testing AddRating for valid product and rating array null should create new array and return true
        /// </summary>
        [Test]
        public void AddRating_Valid_Rating_Array_Is_Null_Should_Create_New_Array_Return_True()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("starbucks-reserve", 5);

            // Assert
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Testing AddRating for valid product and rating should return true
        /// </summary>
        [Test]
        public void AddRating_Valid_Product_And_Rating_Should_Return_True()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().FirstOrDefault(x => x.Id.Equals("u-village"));
            var count = data.Ratings.Length;

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 5);
            var newData = TestHelper.ProductService.GetProducts().FirstOrDefault(x => x.Id.Equals("u-village"));

            // Assert
            Assert.AreEqual(true, result);
            Assert.AreEqual(count + 1, newData.Ratings.Length);
            Assert.AreEqual(5, newData.Ratings.Last());
        }

        #endregion AddRating

        #region UpdateData

        /// <summary>
        /// Testing valid usage of UpdateData method
        /// </summary>
        [Test]
        public void UpdateData_Valid_Updated_Value_Matches_Should_Return_true()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().FirstOrDefault();
            var data1 = data;
            data1.Title = "Test";

            // Act
            var result = TestHelper.ProductService.UpdateData(data1);

            // Assert
            Assert.AreEqual(data1.Title, result.Title);
        }

        #endregion UpdateData
    }
}