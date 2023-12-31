﻿using TakeABreak.WebSite.Services;
using NUnit.Framework;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using TakeABreak.WebSite.Components;
using Bunit;

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

        #region ProductList

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

        #endregion ProductList

        #region AddRating

        /// <summary>
        /// Test for testing the AddRating button.
        /// </summary>
        [Test]
        public void AddRating_Valid_RatingClick_Should_ReturnNewRating()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            const string TestButtonId = "MoreInfo_columbia-tower-club";
            const string VoteButtonId = "vote_button";

            const string PreVoteString = "Be the first to vote!";
            const string PostVoteString = "1 Vote";

            // Arrange: Built and find the More Info button
            var cut = RenderComponent<ProductList>();
            var moreInfoButton = cut.FindAll("Button").First(element => element.OuterHtml.Contains(TestButtonId));

            // Arrange: Click button and save markup
            moreInfoButton.Click();
            var preVoteMarkup = cut.Markup;

            // Act

            // Act: Find voting button
            var voteButton = cut.FindAll("span").First(element => element.OuterHtml.Contains(VoteButtonId));

            // Act: Click button and save markup
            voteButton.Click();
            var postVoteMarkup = cut.Markup;


            // Reset

            // Assert
            Assert.AreEqual(true, preVoteMarkup.Contains(PreVoteString));
            Assert.AreEqual(true, postVoteMarkup.Contains(PostVoteString));
            Assert.AreNotEqual(preVoteMarkup, postVoteMarkup);
        }

        #endregion AddRating

        #region AddComments

        /// <summary>
        /// Test a valid new comment should add it as a new comment
        /// </summary>
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

        /// <summary>
        /// Test adding a null comment should not add the comment
        /// </summary>
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

        #region TextFilter

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

            page.Instance._filterDataString = "Seattle";

            //Find the Buttons
            var buttonList = page.FindAll("button");

            //Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains("filter-button"));

            button.Click();

            Assert.True(page.Instance._filterData);

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
            var buttonList = page.FindAll("button");

            //Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains("clear-button"));

            button.Click();

            //Get the markup to use for the assert
            var pageMarkup = page.Markup;

            // Assert - Verify that the filter function is called
            Assert.AreEqual(true, pageMarkup.Contains("Seattle"));
        }

        #endregion TextFilter

        #region Dropdown filters

        /// <summary>
        /// Test for Neighborhood filter "All" should return all products content
        /// </summary>
        [Test]
        public void Filters_Products_By_Null_Neighborhood_Should_Return_Matching_Content()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            //Find the input
            var buttonList = page.FindAll("button");


            //Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains("dropdown1"));

            // Act - Simulate changing the filter text
            button.Click();

            var pageMarkup = page.Markup;
            // Assert
            // Ensure the method OnNeighborhoodChanged was called with the correct value
            Assert.AreEqual(true, pageMarkup.Contains("Lemieux Library"));

        }

        /// <summary>
        /// Test for Neighborhood filter "Ballard" should return matching products
        /// </summary>
        [Test]
        public void Filters_Products_By_Neighborhood_Should_Return_Matching_Content()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            const string neighborhood = "Ballard";

            var button = page.Find($"button[test-id=\"dropdown2\"]:contains('{neighborhood}')");

            // Act - Simulate changing the filter text
            button.Click();

            var pageMarkup = page.Markup;
            // Assert
            // Ensure the method OnNeighborhoodChanged was called with the correct value
            Assert.AreEqual(true, pageMarkup.Contains("Ballard"));

        }

        /// <summary>
        /// Test for LocationType filter "All" should return all products content
        /// </summary>
        [Test]
        public void Filters_Products_By_LocationType_Should_Return_Matching_Content()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            //Find the input
            var buttonList = page.FindAll("button");


            //Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains("dropdown3"));

            // Act - Simulate changing the filter text
            button.Click();

            var pageMarkup = page.Markup;
            // Assert
            // Ensure the method OnNeighborhoodChanged was called with the correct value
            Assert.AreEqual(true, pageMarkup.Contains("Arboretum Marsh Walk"));

        }

        /// <summary>
        /// Test for LocationType filter "Bench" should return matching content
        /// </summary>
        [Test]
        public void Filters_Products_By_LocationType_Bench_Should_Return_Matching_Content()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            //Find the input
            var buttonList = page.FindAll("button");

            //Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains("dropdown4"));

            // Act - Simulate changing the filter text
            button.Click();

            var pageMarkup = page.Markup;
            // Assert
            // Ensure the method OnNeighborhoodChanged was called with the correct value
            Assert.AreEqual(true, pageMarkup.Contains("Arboretum Marsh Walk"));

        }

        /// <summary>
        /// Test for LocationType filter "Restroom" should return matching content
        /// </summary
        [Test]
        public void Filters_Products_By_LocationType_Restroom_Should_Return_Matching_Content()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            //Find the input
            var buttonList = page.FindAll("button");

            //Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains("dropdown5"));

            // Act - Simulate changing the filter text
            button.Click();

            var pageMarkup = page.Markup;
            // Assert
            // Ensure the method OnNeighborhoodChanged was called with the correct value
            Assert.AreEqual(true, pageMarkup.Contains("LA Dive Bar"));

        }

        /// <summary>
        /// Test for LocationType filter "Table" should return matching content
        /// </summary
        [Test]
        public void Filters_Products_By_LocationType_Table_Should_Return_Matching_Content()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            //Find the input
            var buttonList = page.FindAll("button");


            //Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains("dropdown6"));

            // Act - Simulate changing the filter text
            button.Click();

            var pageMarkup = page.Markup;
            // Assert
            // Ensure the method OnNeighborhoodChanged was called with the correct value
            Assert.AreEqual(true, pageMarkup.Contains("Golden Gardens"));

        }

        /// <summary>
        /// Test for LocationType filter "Other" should return matching content
        /// </summary
        [Test]
        public void Filters_Products_By_LocationType_Other_Should_Return_Matching_Content()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            //Find the input
            var buttonList = page.FindAll("button");


            //Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains("dropdown7"));

            // Act - Simulate changing the filter text
            button.Click();

            var pageMarkup = page.Markup;
            // Assert
            // Ensure the method OnNeighborhoodChanged was called with the correct value
            Assert.AreEqual(true, pageMarkup.Contains("Lemieux Library"));

        }

        /// <summary>
        /// Test for Noise Level filter "All" should return all products content
        /// </summary
        [Test]
        public void Filters_Products_By_Null_NoiseLevel_Should_Return_Matching_Content()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            //Find the input
            var buttonList = page.FindAll("button");

            //Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains("dropdown8"));

            // Act - Simulate changing the filter text
            button.Click(null);

            var pageMarkup = page.Markup;
            // Assert
            // Ensure the method OnNeighborhoodChanged was called with the correct value
            Assert.AreEqual(true, pageMarkup.Contains("Seattle Aquarium"));

        }

        /// <summary>
        /// Test for Noise Level filter should return matching content
        /// </summary
        [Test]
        public void Filters_Products_By_NoiseLevel_Should_Return_Matching_Content()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            //Find the input
            var buttonList = page.FindAll("button");

            //Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains("dropdown9"));

            // Act - Simulate changing the filter text
            button.Click();

            var pageMarkup = page.Markup;
            // Assert
            // Ensure the method OnNeighborhoodChanged was called with the correct value
            Assert.AreEqual(true, pageMarkup.Contains("Seattle Aquarium"));

        }

        #endregion Dropdown filters

        #region SurpriseMe

        ///<summary>
        /// Test surprise me code should return a random place from the products
        /// </summary>
        [Test]
        public void SurpriseMe_Should_Return_Random_Product()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>(parameters => parameters
            .Add(p => p.SelectedLocationType, "Surprise"));

            // Assert
            Assert.AreEqual("Surprise", page.Instance.SelectedLocationType);
        }

        #endregion SurpriseMe

        #region SocialMedia

        ///<summary>
        /// Test social media code should return social media icons
        /// </summary>
        [Test]
        public void SocialMedia_Should_Return_Social_Media_Icons()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            //Find the input
            var buttonList = page.FindAll("button");


            //Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains("ToggleSocialButtons"));

            // Act - Simulate changing the filter text
            button.Click();

            var pageMarkup = page.Markup;
            // Assert
            // Ensure the method OnNeighborhoodChanged was called with the correct value
            Assert.AreEqual(true, pageMarkup.Contains("Lemieux Library"));
        }

        #endregion SocialMedia
    }
}