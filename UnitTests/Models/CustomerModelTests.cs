using NUnit.Framework;
using TakeABreak.WebSite.Models;

namespace UnitTests.Models.ToString
{

    /// <summary>
    /// Class containing unit test cases to ProductModel
    /// </summary>
    public class CustomerModelTests
    {
        /// <summary>
        /// Testing Json to String serializer
        /// </summary>
        [Test]
        public void Validate_Json_ToString()
        {
            //Arrange
            CustomerModel customerModel = new CustomerModel();

            //Act
            customerModel.NominatedTitle = "Seattle Aquarium ";
            string data = customerModel.ToString();

            //Assert
            Assert.AreEqual(true, data.Contains("Seattle"));
        }
    }
}
