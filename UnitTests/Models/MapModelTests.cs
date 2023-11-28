using NUnit.Framework;
using System.Collections.Generic;
using TakeABreak.WebSite.Models;

namespace UnitTests.Models.ToString
{

    /// <summary>
    /// Class containing unit test cases to MapModel, MapModelFeature, MapModelProperties, MapModelGeometry
    /// </summary>
    public class MapModelTests
    {
        /// <summary>
        /// Testing Json to String serializer
        /// </summary>
        [Test]
        public void MapModel_Validate_Json_ToString()
        {
            //Arrange
            MapModel mapModel = new MapModel();

            //Act
            mapModel.Type = "FeatureCollection";
            string data = mapModel.ToString();

            //Assert
            Assert.AreEqual(true, data.Contains("Feature"));
        }

        /// <summary>
        /// Testing Json to String serializer
        /// </summary>
        [Test]
        public void MapModelFeature_Validate_Json_ToString()
        {
            //Arrange
            MapModelFeature mapModelFeature = new MapModelFeature();

            //Act
            mapModelFeature.Type = "Feature";
            string data = mapModelFeature.ToString();

            //Assert
            Assert.AreEqual(true, data.Contains("Feat"));
        }

        /// <summary>
        /// Testing Json to String serializer
        /// </summary>
        [Test]
        public void MapModelProperties_Validate_Json_ToString()
        {
            //Arrange
            MapModelProperties mapModelProperties = new MapModelProperties();

            //Act
            mapModelProperties.LocationType = "Restroom";
            mapModelProperties.Id = "columbia-tower-club";
            string data = mapModelProperties.ToString();

            //Assert
            Assert.AreEqual(true, data.Contains("Restroom"));
        }

        /// <summary>
        /// Testing Json to String serializer
        /// </summary>
        [Test]
        public void MapModelGeometry_Validate_Json_ToString()
        {
            //Arrange
            MapModelGeometry mapModelGeometry = new MapModelGeometry();

            //Act
            mapModelGeometry.Type = "Point";
            mapModelGeometry.Coordinates = new double[] { -122.32246138226097, 47.61394972733521 };
            string data = mapModelGeometry.ToString();

            //Assert
            Assert.AreEqual(true, data.Contains("Point"));
        }

        /// <summary>
        /// Test to check if Properties and Geometry are set correctly in MapModelFeature
        /// </summary>
        [Test]
        public void MapModelFeature_Properties_Geometry_Should_Be_Set_Correctly()
        {
            // Arrange
            var properties = new MapModelProperties
            {
                LocationType = "Restroom",
                Id = "life-on-mars"
            };
            var geometry = new MapModelGeometry
            {
                Coordinates = new double[] { -122.32246138226097, 47.61394972733521 },
                Type = "Point"
            };

            // Act
            var feature = new MapModelFeature
            {
                Type = "Feature",
                Properties = properties,
                Geometry = geometry
            };

            // Assert
            Assert.AreEqual("Feature", feature.Type);
            Assert.AreEqual(properties, feature.Properties);
            Assert.AreEqual(geometry, feature.Geometry);
        }

        /// <summary>
        /// Test to check if Features list is set correctly in MapModel
        /// </summary>
        [Test]
        public void MapModel_Features_Should_Be_Set_Correctly()
        {
            // Arrange
            var properties = new MapModelProperties
            {
                LocationType = "Restroom",
                Id = "life-on-mars"
            };
            var geometry = new MapModelGeometry
            {
                Coordinates = new double[] { -122.32246138226097, 47.61394972733521 },
                Type = "Point"
            };

            // Act
            var featuresMock = new List<MapModelFeature>
            {
                new MapModelFeature { Type = "Feature1" },
                new MapModelFeature { Type = "Feature2" },
            };

            var mapModel = new MapModel
            {
                Type = "FeatureCollection",
                Features = featuresMock
            };

            // Assert
            Assert.AreEqual("FeatureCollection", mapModel.Type);
            Assert.AreEqual(featuresMock, mapModel.Features);
        }
    }
}