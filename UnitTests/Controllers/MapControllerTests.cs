using TakeABreak.WebSite.Controllers;
using TakeABreak.WebSite.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static TakeABreak.WebSite.Controllers.MapController;

namespace UnitTests.Controllers
{

    /// <summary>
    /// Class containing unit test cases for MapController
    /// </summary>
    public class MapControllerTests
    {

        //Creating an instance
        public static MapController testMapController;

        /// <summary>
        /// Test initialize
        /// </summary>
        #region TestSetup

        [SetUp]
        public void Testinitialize()
        {
            testMapController = new MapController(TestHelper.MapModelService);
        }

        #endregion

        /// <summary>
        /// Testing if get is valid should return geoJson data
        /// </summary>
        [Test]
        public void Get_Valid_Should_Return_List_Of_Products()
        {
            //Arrange
            var result = testMapController.GetGeoJSON();

            //Act

            //Assert
            Assert.NotNull(result);
        }
    }
}