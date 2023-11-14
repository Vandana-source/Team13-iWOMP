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
using System.ComponentModel;
using Microsoft.AspNetCore.Components;

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

        [Test]
        public void SubmitRating_Valid_ID_Click_Unstared_Should_Increment_Count_And_Check_Chair()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            var moreInfoButton = page.Find($"button[data-target='#productModal-arboretum-bench']");

            var buttonMarkup = page.Markup;

            //Get the star Buttons
            var chairButtonList = page.FindAll("span");

            //Get the vote count
            //Get the vote count, the list should have 7 elements, element 2 is the string for the count
            var preVoteCountSpan = chairButtonList[1];
            var preVoteCountString = preVoteCountSpan.OuterHtml;

            //Get the first star item from the list, it should not be checked
            var starButton = chairButtonList.First(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fas fa-chair"));

            //Save the html for it to compare after the click
            var preStarChange = starButton.OuterHtml;

            //Act

            //Click the star button
            starButton.Click();

            //Get the markup to use for the assert
            buttonMarkup = page.Markup;

            //Get the star Buttons
            chairButtonList = page.FindAll("span");

            //Get the vote count
            //Get the vote count, the list should have 7 elements, element 2 is the string for the count
            var postVoteCountSpan = chairButtonList[1];
            var postVoteCountString = postVoteCountSpan.OuterHtml;

            //Get the first star item from the list, it should not be checked
            starButton = chairButtonList.First(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fas fa-chair checked"));

            //Save the html for it to compare after the click
            var postStarChange = starButton.OuterHtml;

            //Click the star button
            starButton.Click();

            //Get the markup to use for the assert
            buttonMarkup = page.Markup;

            //Get the star Buttons
            chairButtonList = page.FindAll("span");

            //Get the vote count
            //Get the vote count, the list should have 7 elements, element 2 is the string for the count
            var postVoteCountSpan2 = chairButtonList[1];
            var postVoteCountString2 = postVoteCountSpan2.OuterHtml;

            //Get the first star item from the list, it should not be checked
            starButton = chairButtonList.First(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fas fa-chair checked"));

            //Save the html for it to compare after the click
            var postStarChange2 = starButton.OuterHtml;

            //Assert
            System.Console.WriteLine($"Rating received before and after {preVoteCountString}:{postVoteCountString}:{postVoteCountString2}");

            //Confirm that the record has no votes to start, and 1 vote after
            Assert.AreEqual(true, preVoteCountString.Contains("Be the first to vote!"));
            Assert.AreEqual(true, postVoteCountString.Contains("1 Vote"));
            Assert.AreEqual(true, postVoteCountString2.Contains("2 Votes"));
            Assert.AreEqual(false, preVoteCountString.Equals(postVoteCountString));
        }

        #region AddComments
        [Test]
        public void AddComment_Valid_NewComment_Should_AddComment()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            const string TestButtonId = "MoreInfo_columbia-tower-club";
            const string NewCommentButtonId = "new_comment_button";
            const string NewCommentInputId = "new_comment_input";
            const string AddCommentButtonId = "add_comment_button";

            const string TestComment = "This is a test comment.";

            // Arrange: Built and find the More Info button
            var cut = RenderComponent<ProductList>();
            var moreInfoButton = cut.FindAll("Button").First(element => element.OuterHtml.Contains(TestButtonId));

            // Arrange: Click button and save markup
            moreInfoButton.Click();
            var preCommentMarkup = cut.Markup;

            // Arrange: Find comment button and click
            var newCommentButton = cut.FindAll("Button").First(element => element.OuterHtml.Contains(NewCommentButtonId));
            newCommentButton.Click();


            // Act

            // Act: Find input box, add text
            var newCommentInput = cut.FindAll("Input").First(element => element.OuterHtml.Contains(NewCommentInputId));
            newCommentInput.Change(TestComment);

            // Act: Find add button and save comment
            var addCommentButton = cut.FindAll("Button").First(element => element.OuterHtml.Contains(AddCommentButtonId));
            addCommentButton.Click();

            var postCommentMarkup = cut.Markup;

            // Reset

            // Assert
            Assert.AreEqual(false, preCommentMarkup.Contains(TestComment));
            Assert.AreEqual(true, postCommentMarkup.Contains(TestComment));

        }

        [Test]
        public void AddComment_Invalid_Null_NewComment_Should_Not_AddComment()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            const string TestButtonId = "MoreInfo_columbia-tower-club";
            const string NewCommentButtonId = "new_comment_button";
            const string NewCommentInputId = "new_comment_input";
            const string AddCommentButtonId = "add_comment_button";

            const string TestComment = "";

            // Arrange: Built and find the More Info button
            var cut = RenderComponent<ProductList>();
            var moreInfoButton = cut.FindAll("Button").First(element => element.OuterHtml.Contains(TestButtonId));

            // Arrange: Click button and save markup
            moreInfoButton.Click();
            var preCommentMarkup = cut.Markup;

            // Arrange: Find comment button and click
            var newCommentButton = cut.FindAll("Button").First(element => element.OuterHtml.Contains(NewCommentButtonId));
            newCommentButton.Click();


            // Act

            // Act: Find input box, add text
            var newCommentInput = cut.FindAll("Input").First(element => element.OuterHtml.Contains(NewCommentInputId));
            newCommentInput.Change(TestComment);

            // Act: Find add button and save comment
            var addCommentButton = cut.FindAll("Button").First(element => element.OuterHtml.Contains(AddCommentButtonId));
            addCommentButton.Click();

            var postCommentMarkup = cut.Markup;

            // Reset

            // Assert
            Assert.AreEqual(true, postCommentMarkup.Contains(TestComment));

        }

        #endregion AddComments
    }
}