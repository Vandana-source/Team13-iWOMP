using NUnit.Framework;
using TakeABreak.WebSite.Enums;
using YourNamespace;

namespace UnitTests.Enums
{
    /// <summary>
    /// Class to provide unit testing of NeighborhoodEnums.cs
    /// </summary>
    public class NeighborhoodEnumsTests
    {
        #region TestSetup
        /// <summary>
        /// Test Setup
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region StaticMethodsTests
        [Test]
        /// <summary>
        /// Test that checks functionality of GetName
        /// </summary>
        public void Valid_Enum_Should_Return_Correct_Name()
        {
            // Arrange
            SeattleNeighborhoods testEnum = SeattleNeighborhoods.Ballard;

            // Act

            // Assert
            Assert.AreEqual("Ballard", testEnum.ToString());

        }
        #endregion StaticMethodTests
    }
}