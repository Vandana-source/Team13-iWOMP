using System.Diagnostics;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using TakeABreak.WebSite.Models;


using NUnit.Framework;

using Moq;

using TakeABreak.WebSite.Pages;
using TakeABreak.WebSite.Services;
using System.Collections.Generic;
using TakeABreak.WebSite.Controllers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UnitTests.Pages.Spotlight
{
    // <summary>
    /// Unit testing for Spotlight page
    /// </summary>
    public class SpotlightTests
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

        public static SpotlightModel pageModel;

        public static Mock<JsonFileProductService> mockProductService;

        /// <summary>
        /// Set up test intialize
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            httpContextDefault = new DefaultHttpContext()
            {
                TraceIdentifier = "trace",
                //RequestServices = serviceProviderMock.Object,
            };
            httpContextDefault.HttpContext.TraceIdentifier = "trace";

            modelState = new ModelStateDictionary();

            actionContext = new ActionContext(httpContextDefault, httpContextDefault.GetRouteData(), new PageActionDescriptor(), modelState);

            modelMetadataProvider = new EmptyModelMetadataProvider();
            viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            tempData = new TempDataDictionary(httpContextDefault, Mock.Of<ITempDataProvider>());

            pageContext = new PageContext(actionContext)
            {
                ViewData = viewData,
                HttpContext = httpContextDefault
            };

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            var MockLoggerDirect = Mock.Of<ILogger<SpotlightModel>>();
            JsonFileProductService productService;

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);
            mockProductService = new Mock<JsonFileProductService>(mockWebHostEnvironment.Object);

            pageModel = new SpotlightModel(productService)
            {
                PageContext = pageContext,
                TempData = tempData,
            };
        }

        #endregion TestSetup

        /// <summary>
        /// Tests OnGet with a valid product, should return valid product data
        /// </summary>
        #region OnGet
        [Test]
        public void OnGet_Valid_Activity_Set_Should_Return_RequestId()
        {
            // Arrange

            // Act
            pageModel.OnGet();

            // Reset

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
        }

        /// <summary>
        /// Tests OnGet with a null product, should redirect to index page
        /// </summary>
        [Test]
        public void OnGet_InValid_ProductId_Should_Redirect_To_Index()
        {
            // Arrange
            mockProductService.Setup(p => p.GetProducts()).Returns(new List<ProductModel>());

            var pageModel = new SpotlightModel(mockProductService.Object);

            // Act
            var result = pageModel.OnGet() as RedirectToPageResult;

            Assert.AreEqual(false, pageModel.ModelState.IsValid);
            Assert.AreEqual("./Index", result.PageName);

        }

        #endregion OnGet
    }
}