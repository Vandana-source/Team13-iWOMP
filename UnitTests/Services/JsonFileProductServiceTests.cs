using System.Linq;

using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;
using Moq; 

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Collections.Generic;

namespace UnitTests.Pages.Product.AddRating
{
    public class JsonFileProductServiceTests
    {

        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region AddRating

        /// <summary>
        /// For a valid product and ID:
        /// - Tests if ratings array is null, one is created.
        /// - Sees if rating is added correctly  
        /// </summary>
        [Test]
        public void AddRating_Valid_Product_If_Ratings_Null_Should_Create_Ratings_Array_And_Add_Rating_Successfully()
        {
            // Arrange
            // Temp. product with null ratings 
            var product = new ProductModel()
            {
                Id = "validProductId",
                
            };

            // Act
            TestHelper.ProductService.AddRating("validProductId", 3);

            // Assert
            Assert.AreEqual(new List<int> { 3 }, product.Ratings.ToList());


        }


        // <summary>
        // If ratings is not null, checks if rating was added to the array successfully 
        // </summary>     
        [Test]
        public void AddRating_Valid_Product_If_Ratings_Not_Null_Should_Add_Rating_To_Array_Successfully()
        {
            // Arrange
            // Temp. product with populated ratings 
            var product = new ProductModel()
            {
                Id = "validProductId",
                Ratings = new int[] { 2, 3}

            };

            // Act
            TestHelper.ProductService.AddRating("validProductId", 3);


            // Assert
            Assert.AreEqual(new int[] { 2, 3, 3 }, product.Ratings.ToList());

        }

        #endregion 

    }

}
