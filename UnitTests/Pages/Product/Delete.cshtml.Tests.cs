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
    /// <summary>
    /// Class containing unit test cases for delete page
    /// </summary>
	public class DeleteTests
	{
        /// <summary>
        /// Creating instance to the model
        /// </summary>
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

        #region OnGet

        /// <summary>
        /// Testing OnGet valid should return all products
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {
            //Arrange

            //Act
            pageModel.OnGet("Aquarium");

            //Asset
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual("Seattle Aquarium", pageModel.Product.Title);
        }



        /// <summary>
        /// Testing OnGet for invalid should return false
        /// </summary>
        [Test]
        public void OnGet_Invalid_Should_Return_False()
        {
            //Arrange

            //Act
            pageModel.OnGet("Test");

            //Asset
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
        }

        #endregion OnGet

        #region OnPost

        /// <summary>
        /// Testing OnPost invalid model and not valid return to page
        /// </summary>
        [Test]
        public void OnPost_InValid_Model_NotValid_Return_Page()
        {
            //Arrange

            //Act
            pageModel.ModelState.AddModelError("-light", "");
            var result = pageModel.OnPost() as ActionResult;

            //Assert
            Assert.AreEqual(false, pageModel.ModelState.IsValid);
        }

        /// <summary>
        /// Testing OnPost for valid model return changed page
        /// </summary>
        [Test]
        public void OnPost_Valid_Model_Return_Changed_Page()
        {
            //Arrange

            //Act
            pageModel.ModelState.AddModelError(" ", " ");
            var result = pageModel.OnPost() as ActionResult;

            //Assert
            Assert.AreEqual(false, pageModel.ModelState.IsValid);
        }

        /// <summary>
        /// Testing OnPost for valid should return true
        /// </summary>
        [Test]
        public void OnPost_Valid_Should_Return_True()
        {
            //Arrange
            pageModel.Product = new ProductModel
            {
                Title = "test",
                Image = "test",
                Description = "test",
            };

            //Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            //Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        #endregion OnPost

    }
}




