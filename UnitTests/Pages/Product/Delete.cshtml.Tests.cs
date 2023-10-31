using System.Linq;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;

using NUnit.Framework;

using ContosoCrafts.WebSite.Models;

namespace UnitTests.Pages.Product.Delete
{
	public class Delete
	{
        #region TestSetup
        public static IUrlHelperFactory urlHelperFactory;
        public static DefaultHttpContext httpContextDefault;
        public static IWebHostEnvironment webHostEnvironment;
        public static ModelStateDictionary modelState;
        public static ActionContext actionContext;
        public static EmptyModelMetadataProvider modelMetadataProvider;
        public static ViewDataDictionary viewData;
        public static TempDataDictionary tempData;
        public static PageContext pageContext;

        public static DeleteModel pageModel;

        /// <summary>
        /// Set up test intialize
        /// </summary>

        [SetUp]
        public void TestInitialize()
        {
            httpContextDefault = new DefaultHttpContext()
            {
                //RequestServices = serviceProviderMock.Object,
            };

            modelState = new ModelStateDictionary();

            actionContext = new ActionContext(httpContextDefault, httpContextDefault.GetRouteData(), new PageActionDescriptor(), modelState);

            modelMetadataProvider = new EmptyModelMetadataProvider();
            viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            tempData = new TempDataDictionary(httpContextDefault, Mock.Of<ITempDataProvider>());

            pageContext = new PageContext(actionContext)
            {
                ViewData = viewData,
            };

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            var MockLoggerDirect = Mock.Of<ILogger<IndexModel>>();
            JsonFileProductService productService;

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            pageModel = new DeleteModel(productService)
            {
            };
        }

        #endregion TestSetup

        #region DeleteData
        [Test]
        public void DeleteData_Valid_Delete_Product_Should_Return_True()
        {
            // Arrange
            var product = new ProductModel
            {
                Id = "Aquarium",
                Title = "Seattle Aquariu",
                LocationType = "Other",
                Neighborhood = "Downtown",
                Description = "Meet a few adorable sea otters, and greet the various sea creatures of the Pacific Ocean, from puffers to giant clams.",
                MapURL= "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2689.9634603446284!2d-122.34553072360696!3d47.60740017118983!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x54906aad8804fa43%3A0x6b99e65de2d47683!2sSeattle%20Aquarium!5e0!3m2!1sen!2sus!4v1697526521423!5m2!1sen!2sus"
            };

            //Act
                var result = TestHelper.ProductService.DeleteData(product);

            //Assert 
                Assert.AreEqual(true, result);
        }



       [Test]
       public void DeleteData_Invalid_Delete_Product_Should_Return_False()
       {
            // Arrange
            var product = new ProductModel
            {
                Id = "testId",
                Title = "Title",
                LocationType = "Location",
                Neighborhood = "Neighborhood",
                Description = "Description",
                MapURL = "Map",
            };

            // Act
            var result = TestHelper.ProductService.DeleteData(product);

            // Assert
            Assert.AreEqual(false, result);
    }

        #endregion DeleteData

	}
}




