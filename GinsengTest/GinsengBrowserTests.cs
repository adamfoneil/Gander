using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gander.Unit;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Linq;

namespace GinsengTest
{
	[TestClass]
	public class GinsengBrowserTests : SeleniumHelper
	{
		protected override Form LoginForm()
		{
			var form = new Form("/Account/Login", "frmLogin")
			{
				{ "Email", string.Empty },
				{ "Password", string.Empty }
			};
			form.KeySource = InsertedKeySource.None;

			return form;
		}

		protected override string LogoffUrl => "Account/Logoff";

		[TestMethod]
		public void SubmitRequest()
		{			
			EnumDriversAndEnvironments((driver, env) =>
			{
				Login(driver, env);

				int id = new Form("/Request/Create", "frmMain")
				{
					{ "ApplicationId", "Ginseng" },
					{ "TypeId", "Fix" },
					{ "Title", "Sample Request" },
					{ "TextBody", $"This is a sample request created by unit test project in {env.Name} by {driver.GetType().Name}." }
				}.Submit(driver, env);

				Assert.IsTrue(env.AssertExists("SELECT 1 FROM [dbo].[Request] WHERE [Id]=@id", new { id }));
			});
		}

		protected override IEnumerable<Gander.Unit.Abstract.Environment> GetEnvironments()
		{
			yield return new GinsengEnvironment("Prod", "http://ginseng.azurewebsites.net");
		}

		protected override IEnumerable<IWebDriver> GetWebDrivers()
		{
			yield return new ChromeDriver();
		}
	}
}
