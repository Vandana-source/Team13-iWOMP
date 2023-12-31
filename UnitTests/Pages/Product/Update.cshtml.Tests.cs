﻿using System.Collections.Generic;
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


namespace UnitTests.Pages.Product.Update
{
    /// <summary>
    /// Unit testing for Update page
    /// </summary>
    public class UpdateTests
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

        public static UpdateModel pageModel;

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

            pageModel = new UpdateModel(productService, mockWebHostEnvironment.Object)
            {
            };
        }

        #endregion TestSetup

        /// <summary>
        /// Tests OnGet with a valid product, should return valid product data
        /// </summary>
        #region OnGet
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange

            // Act
            pageModel.OnGet("SeattleUQuadTable1");

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual("Seattle University Green", pageModel.Product.Title);

            // Reset
            pageModel.ModelState.Clear();
        }

        /// <summary>
        /// Tests OnGet with a Invalid product, should return null
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_InvalidState()
        {
            // Arrange

            // Act
            pageModel.OnGet("bogus");

            // Assert
            Assert.AreEqual(false, pageModel.ModelState.IsValid);

            // Reset
            pageModel.ModelState.Clear();
        }

        #endregion OnGet


        /// <summary>
        /// Tests OnPost with a Valid product, it redirects to the "Index Page"
        /// </summary>
        #region OnPost
        [Test]
        public void OnPost_Valid_Should_Return_Products()
        {
            // Arrange

            // Act
            pageModel.OnGet("Aquarium");
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            var originalTitle = pageModel.Product.Title;

            // change Value to Microsoft  and update
            pageModel.Product.Title = "Aquarium Title test";
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert to see that post succeeeded
            Assert.AreEqual(true, pageModel.ModelState.IsValid);

            // Read it to see if it changed
            pageModel.OnGet("Aquarium");

            // Assertions to verify
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual("Aquarium Title test", pageModel.Product.Title);

            // Reset it back
            pageModel.Product.Title = originalTitle;
            result = pageModel.OnPost() as RedirectToPageResult;

            // Assert to see that post succeeded
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            
            // Reset
            pageModel.ModelState.Clear();
        }

        /// <summary>
        /// Tests OnPost with a Invalid product,return invalid page
        /// </summary>
        [Test]
        public void OnPost_InValid_Model_Not_Valid_Return_Page()
        {
            // Arrange

            // Force an invalid error state
            pageModel.ModelState.AddModelError("bogus", "bogus error");

            // Act
            // Store the ActionResult of the post? TODO: better understand this line of code or ask professor
            var result = pageModel.OnPost() as ActionResult;
            // Store whether the ModelState is valid for later assert
            var stateIsValid = pageModel.ModelState.IsValid;

            // Assert
            Assert.AreEqual(false, stateIsValid);

            // Reset
            // This should remove the error we added
            pageModel.ModelState.Clear();
        }

        /// <summary>
        /// Testing valid image file and location type should store image in correct directory and redirect to Index page
        /// </summary>
        [Test]
        public void OnPost_Valid_File_And_LocationType_Should_Save_In_Correct_SubDirectory_And_Redirect_To_Index()
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
                var fileName = "test.jpg";
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
                    Id = "cal-anderson-park",
                    LocationType = testCase.LocationType,
                    Image = "/SiteImages/Restrooms/CalAnderson.jpg"
                };

                // Act
                var result = pageModel.OnPost() as RedirectToPageResult;

                // Assert on correct sub-directory
                string expectedPath = Path.Combine("/SiteImages", testCase.ExpectedSubDirectory);
                bool isExpectedSubDirectory = pageModel.Product.Image.Contains(expectedPath);
                Assert.AreEqual(true, isExpectedSubDirectory, $"Failed for LocationType: {testCase.LocationType}");

                // Assert on correct redirection
                Assert.AreEqual("./Index", result.PageName);

                // Reset
                pageModel.ModelState.Clear();
            }
        }

        /// <summary>
        /// Testing valid image file and dummy image redirect to Index page
        /// </summary>
        [Test]
        public void OnPost_Valid_File_And_LocationType_Should_Save_In_Correct_SubDirectory_And_Redirect_To_Index_Page()
        {
            // Arrange

            // Mock the IFormFile
            var mockFormFile = new Mock<IFormFile>();
            var content = "Dummy file content";
            var fileName = "test.jpg";
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
                Id = "cal-anderson-park",
                LocationType = "Restroom",
                Image = "dummy"
            };

            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert on correct sub-directory
            string expectedPath = Path.Combine("/SiteImages", "Restroom");
            bool isExpectedSubDirectory = pageModel.Product.Image.Contains(expectedPath);
            Assert.AreEqual(true, isExpectedSubDirectory, $"Failed for LocationType: Restroom");

            // Assert on correct redirection
            Assert.AreEqual("./Index", result.PageName);

            // Reset
            pageModel.ModelState.Clear();

        }

        /// <summary>
        /// Testing valid image file and image null should save existing image back in JSON and redirect to Index page
        /// </summary>
        [Test]
        public void OnPost_Valid_File_And_Image_Null_Should_Save_Existing_Image_And_Redirect_To_Index()
        {
            // Arrange

            // Mock the IFormFile
            var mockFormFile = new Mock<IFormFile>();
            var content = "Dummy file content";
            var fileName = "test.jpg";
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
                Id = "cal-anderson-park",
                LocationType = "Restroom",
                Image = null
            };

            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert on correct sub-directory
            string expectedPath = Path.Combine("/SiteImages", "Restrooms");
            bool isExpectedSubDirectory = pageModel.Product.Image.Contains(expectedPath);
            Assert.AreEqual(true, isExpectedSubDirectory, $"Failed for LocationType: Restroom");

            // Assert on correct redirection
            Assert.AreEqual("./Index", result.PageName);

            // Reset
            pageModel.ModelState.Clear();
        }

        /// <summary>
        /// Testing Invalid file extension should stay on the same page
        /// </summary>
        [Test]
        public void OnPost_Invalid_File_Should_Stay_On_Page()
        {
            // Arrange

            // Mock the IFormFile
            var mockFormFile = new Mock<IFormFile>();
            var content = "Dummy file content";
            var fileName = "test.pdf";
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
                Id = "cal-anderson-park",
                LocationType = "Restroom",
                Image = null
            };

            // Act
            var result = pageModel.OnPost() as PageResult;

            // Assert on correct sub-directory
            string expectedPath = Path.Combine("/SiteImages", "Restrooms");
            bool isExpectedSubDirectory = pageModel.Product.Image.Contains(expectedPath);
            Assert.AreEqual(true, isExpectedSubDirectory, $"Failed for LocationType: Restroom");

            // Assert
            Assert.AreEqual(false, pageModel.ModelState.IsValid);

            var isPageResultType = result is PageResult;
            Assert.AreEqual(true, isPageResultType);

            // Reset
            pageModel.ModelState.Clear();
        }

        /// <summary>
        /// Testing Invalid Map URL should stay on the same page
        /// </summary>
        [Test]
        public void OnPost_Invalid_MapURL_Should_Stay_On_Page()
        {
            // Arrange            

            // Set up product
            pageModel.Product = new ProductModel
            {
                Id = "cal-anderson-park",
                LocationType = "Restroom",
                MapURL = "dummy"
            };

            // Act
            var result = pageModel.OnPost() as PageResult;

            // Assert
            Assert.AreEqual(false, pageModel.ModelState.IsValid);

            var isPageResultType = result is PageResult;
            Assert.AreEqual(true, isPageResultType);

            // Reset
            pageModel.ModelState.Clear();
        }

        #endregion OnPost

    }
}