using ContosoCrafts.WebSite.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Models.ToString
{

    /// <summary>
    /// Class containing unit test cases to ProductModel
    /// </summary>
    public class ProductModelTests
    {
        /// <summary>
        /// Testing Json to String serializer
        /// </summary>
        [Test]
        public void Validate_Json_ToString()
        {
            //Arrange
            ProductModel productModel = new ProductModel();

            //Act
            productModel.Title = "Seattle Aquarium ";
            string data = productModel.ToString();

            //Assert
            Assert.AreEqual(true, data.Contains("Seattle"));
        }
    }
}