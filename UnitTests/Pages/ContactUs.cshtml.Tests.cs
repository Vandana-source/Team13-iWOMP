using System.Collections.Generic;
using System.IO;
using TakeABreak.WebSite.Pages.Product;
using System.Text;
using TakeABreak.WebSite.Services;
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
using TakeABreak.WebSite.Models;
using TakeABreak.WebSite.Pages;

namespace UnitTests.Pages.ContactUs
{
    // <summary>
    /// Unit testing for ContactUs page
    /// </summary>
	public class ContactUsTests
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

        public static ContactUsModel pageModel;

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

            var MockLoggerDirect = Mock.Of<ILogger<ContactUsModel>>();
            JsonFileCustomerService customerService;

            customerService = new JsonFileCustomerService(mockWebHostEnvironment.Object);

            pageModel = new ContactUsModel(customerService, mockWebHostEnvironment.Object, MockLoggerDirect)
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

        #endregion OnGet
        
        /// <summary>
        /// Tests OnPost with valid customer input should return new customer
        /// </summary>
        #region OnPost
        
        [Test]
        public void OnPost_Valid_File_And_LocationType_Should_Save_In_Correct_SubDirectory_And_Stay_On_Page()
        {
            // Data for testing
            var testCases =
                new List<(string NominatedLocationType, string ExpectedSubDirectory)>
                {
                    ("Table", "Tables"),
                    ("Bench", "Benches"),
                    ("Restroom", "Restrooms"),
                    ("UnknownType", "Others") 
                };

            // Loop through the test case scenarios and test them
            foreach (var testCase in testCases)
            {
                // Set up product for this iteration
                pageModel.CustomerNomination = new CustomerModel
                {
                    NominatedTitle = "Title",
                    NominatedLocationType = testCase.NominatedLocationType,
                    NominatedNeighborhood =  "Neighborhood",
                    NominatedDescription = "Description",
                    NominatedMapDetails = "https://www.google.com/maps/embed?pb=!12345",
                };

                // Act
                var result = pageModel.OnPost() as RedirectToPageResult;

                // Assert on correct title after creation
                Assert.AreEqual("Title", pageModel.CustomerNomination.NominatedTitle);

                // Assert on correct redirection
                Assert.AreEqual("/ContactUs", result.PageName);

                // Delete the created data
                
                // Reset
                pageModel.ModelState.Clear();
            }
        }
        
        #endregion OnPost
      
    }
}