using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;

namespace UnitTests.Pages.Startup
{
    /// <summary>
    /// Unit tests for the startup process of the ContosoCrafts.WebSite 
    /// (TakeABreak) application.
    /// </summary>
    public class StartupTests
    {
        #region TestSetup
        
        /// <summary>
        /// Initializes necessary resources before each test run.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
        }
        
        /// <summary>
        /// A derived Startup class for testing purposes, inheriting from the
        /// web application's original Startup class.
        /// </summary>
        public class Startup : ContosoCrafts.WebSite.Startup
        {
            /// <summary>
            /// Constructor that initializes the derived startup class with the
            /// provided configuration.
            /// </summary>
            /// <param name="config">The application's configuration.</param>
            public Startup(IConfiguration config) : base(config) { }
        }
        #endregion TestSetup
 
        #region ConfigureServices
        
        /// <summary>
        /// Tests if the Startup's ConfigureServices method executes without any
        /// errors and the resultant web host is valid.
        /// </summary>
        [Test]
        public void Startup_ConfigureServices_Valid_Defaut_Should_Pass()
        {
            // Create a default web host builder and set it to use the derived
            // Startup class for testing.
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            
            Assert.IsNotNull(webHost);
        }
        #endregion ConfigureServices

        #region Configure
        
        /// <summary>
        /// Tests if the Startup's Configure method executes without any errors 
        /// and the resultant web host is valid.
        /// </summary>
        [Test]
        public void Startup_Configure_Valid_Defaut_Should_Pass()
        {
            // Create a default web host builder and set it to use the derived
            // Startup class for testing.
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            
            Assert.IsNotNull(webHost);
        }

        #endregion Configure
    }
}
