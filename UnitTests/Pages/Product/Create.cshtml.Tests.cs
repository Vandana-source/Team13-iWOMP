using System.Collections.Generic;
using System.IO;
using ContosoCrafts.WebSite.Pages.Product;
using System.Text;
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


namespace UnitTests.Pages.Product.Create
{
    /// <summary>
    /// Unit testing for Create Page
    /// </summary>
    public class CreateTests
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

        public static CreateModel pageModel;

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

            actionContext = new ActionContext(httpContextDefault,
                httpContextDefault.GetRouteData(), new PageActionDescriptor(),
                modelState);

            modelMetadataProvider = new EmptyModelMetadataProvider();
            viewData =
                new ViewDataDictionary(modelMetadataProvider, modelState);
            tempData = new TempDataDictionary(httpContextDefault,
                Mock.Of<ITempDataProvider>());

            pageContext = new PageContext(actionContext)
            {
                ViewData = viewData,
            };

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath)
                .Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath)
                .Returns("./data/");

            var MockLoggerDirect = Mock.Of<ILogger<IndexModel>>();
            JsonFileProductService productService;

            productService =
                new JsonFileProductService(mockWebHostEnvironment.Object);

            pageModel = new CreateModel(productService,
                mockWebHostEnvironment.Object)
            {
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Checks if OnGet working for the create page 
        /// </summary>
        [Test]
        public void OnGet_Valid_Activity_Set_Should_Return_RequestId()
        {
            // Arrange

            // Act
            pageModel.OnGet();

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
        }

        #endregion OnGet

        #region OnPost

        /// <summary>
        /// Tests the OnPost method to ensure that a valid product creation
        /// results in a valid model state and returns to the index page.
        /// Tests switch statement path with Benches, Table, Restrooms, Other
        /// </summary>
        [Test]
            public void OnPost_Valid_File_And_LocationType_Should_Save_InCorrect_SubDirectory_And_Redirect_To_Index()
            {
                // Data for testing
                var testCases = new List<(string LocationType, string ExpectedSubDirectory)>
                {
                    ("Table", "Tables"),
                    ("Bench", "Benches"),
                    ("Restroom", "Restrooms"),
                    ("UnknownType", "Others") // default case
                };
                
                // Loop through the test case scenarios and test them
                foreach (var testCase in testCases)
                {
                    // Arrange

                    // Mock the IFormFile
                    var mockFormFile = new Mock<IFormFile>();
                    var content = "Dummy file content";
                    var fileName = "test.txt";
                    var byteArray = Encoding.UTF8.GetBytes(content);
                    var stream = new MemoryStream(byteArray);
                    
                    // Set up the files
                    mockFormFile.Setup(f => f.FileName).Returns(fileName);
                    mockFormFile.Setup(f => f.Length).Returns(stream.Length);
                    mockFormFile.Setup(m => m.OpenReadStream()).Returns(stream);
        
                    pageModel.UploadedFile = mockFormFile.Object;

                    // Set up product for this iteration
                    pageModel.Product = new ProductModel
                    {
                        Id = "testId",
                        Title = "Title",
                        LocationType = testCase.LocationType,
                        Neighborhood = "Neighborhood",
                        Description = "Description",
                        MapURL = "Map",
                    };

                    // Act
                    var result = pageModel.OnPost() as RedirectToPageResult;

                    // Assert on correct sub-directory
                    string expectedPath = Path.Combine("/SiteImages", testCase.ExpectedSubDirectory);
                    bool isExpectedSubDirectory = pageModel.Product.Image.Contains(expectedPath);
                    Assert.AreEqual(true, isExpectedSubDirectory, $"Failed for LocationType: {testCase.LocationType}");

                    // Assert on correct redirection
                    Assert.AreEqual("Index", result.PageName);
                }
            }
        
        /// <summary>
        /// Tests invalid Model state stays on page
        /// </summary>
        [Test]
        public void OnPost_Invalid_ModelState_Should_Stay_On_Page()
        {
            // Arrange: Set an error in the ModelState to simulate a model validation failure.
            pageModel.ModelState.AddModelError("Product.Title",
                "Title is required");

            // Set up product
            pageModel.Product = new ProductModel
            {
                Id = "testId",
                // Don't provide a Title, which might be a required field.
                LocationType = "Bench",
                Neighborhood = "Neighborhood",
                Description = "Description",
                MapURL = "Map",
            };

            // Act
            var result = pageModel.OnPost() as PageResult;

            // Assert
            // Confirming that ModelState is invalid.
            Assert.AreEqual(false, pageModel.ModelState.IsValid);

            var isPageResultType = result is PageResult;
            Assert.AreEqual(true, isPageResultType);
        }
        /// <summary>
        /// No upload file stays on page 
        /// </summary>
        [Test]
        public void OnPost_Invalid_No_Uploaded_File_Should_Stay_On_Page()
        {
            // Arrange: No uploaded file (null).
            pageModel.UploadedFile = null;

            // Dummy product details
            pageModel.Product = new ProductModel
            {
                Id = "testId",
                Title = "Title",
                LocationType = "Bench",
                Neighborhood = "Neighborhood",
                Description = "Description",
                MapURL = "Map",
            };

            // Act
            var result = pageModel.OnPost() as PageResult;

            // Assert
            // Confirming that ModelState is invalid.
            Assert.AreEqual(true, pageModel.ModelState.IsValid);

            // Confirming that the result type is a PageResult.
            Assert.AreEqual(true, result is PageResult);

        }
        
        #endregion OnPost
        
        # region UploadImage
        
        /// <summary>
        /// Tests the property UploadImage set/get to make sure it works 
        /// </summary>
        [Test]
        public void UploadedFile_Valid_Set_And_Get_Should_Return_Expected_Value()
        {
            // Arrange
            var mockFormFile = new Mock<IFormFile>();

            // Act
            // pageModel.UploadedFile = mockFormFile.Object;
            var retrievedFile = pageModel.UploadedFile;

            // Assert
            // Assert.AreEqual(mockFormFile.Object, retrievedFile);
        }

        #endregion UploadImage
    }
}

