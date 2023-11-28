using NUnit.Framework;
using TakeABreak.WebSite.Enums;

namespace UnitTests.Enums
{
    /// <summary>
    /// Class to provide unit testing of NoiseLevelEnum.cs
    /// </summary>
    public class NoiseLevelEnumTests
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
            NoiseLevelEnum testEnum = NoiseLevelEnum.VeryLoud;

            // Act

            // Assert
            Assert.AreEqual("VeryLoud", testEnum.ToString());
        }
        #endregion StaticMethodTests
    }
}