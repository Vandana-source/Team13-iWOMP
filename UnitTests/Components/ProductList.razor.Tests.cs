using TakeABreak.WebSite.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TakeABreak.WebSite.Components;
using System.Net.WebSockets;
using Bunit;
using TakeABreak.WebSite.Components;
using TakeABreak.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UnitTests.Components
{

    /// <summary>
    /// Class containing unit test cases to ProductList
    /// </summary>
    public class ProductListTests : BunitTestContext
    {
        #region TestSetup

        /// <summary>
        /// Test initialize
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        /// <summary>
        /// Test for ProductList Default Should Return Content
        /// </summary>
        [Test]
        public void ProductList_Default_Should_Return_Content()
        {
            //Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            //Act
            var page = RenderComponent<ProductList>();

            //Get the cards returned
            var result = page.Markup;

            //Assert
            Assert.AreEqual(true, result.Contains("Seattle Aquarium"));
        }

        /// <summary>
        /// Test for filter text "Seattle" should return matching products
        /// </summary>
        [Test]
        public void Filters_Products_By_Title_Should_Return_Matching_Content()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            //Find the input
            var inputList = page.FindAll("input");

            //Find the one that matches the ID looking for and click it
            var input = inputList.First(m => m.OuterHtml.Contains("filter-input"));

            // Act - Simulate changing the filter text
            input.Change("Seattle");

            //Get the markup to use for the assert
            var pageMarkup = page.Markup;

            // Assert - Verify that the filter function is called
            Assert.AreEqual(true, pageMarkup.Contains("Seattle"));  
        }

        /// <summary>
        /// Test for filter button should return all products
        /// </summary>
        [Test]
        public void Filters_Products_By_Title_And_Filter_Button_Should_Return_Products()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            //Find the Buttons (more info)
            var buttonList = page.FindAll("Button");

            //Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains("filter-button"));

            button.Click();

            //Get the markup to use for the assert
            var pageMarkup = page.Markup;

            // Assert - Verify that the filter function is called
            Assert.AreEqual(true, pageMarkup.Contains("Seattle"));
        }

        /// <summary>
        /// Test for clear button should return all products
        /// </summary>
        [Test]
        public void Filters_Products_By_Title_And_Clear_Button_Should_Return_All_Products()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            //Find the Buttons (more info)
            var buttonList = page.FindAll("Button");

            //Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains("clear-button"));

            button.Click();

            //Get the markup to use for the assert
            var pageMarkup = page.Markup;

            // Assert - Verify that the filter function is called
            Assert.AreEqual(true, pageMarkup.Contains("Seattle"));
        }

    }
}