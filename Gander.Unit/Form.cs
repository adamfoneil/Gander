using Gander.Unit.Abstract;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gander.Unit
{
	public class Form : Dictionary<string, string>
	{
		private readonly string _formElementId;
		private readonly string _url;

		public Form(string url, string formElementId)
		{
			_url = url;
			_formElementId = formElementId;
		}

		public bool IsAuthenticationRequired { get; set; }

		public string FormElementId { get { return _formElementId; } }

		public string Url { get { return _url; } }

		public InsertedKeySource KeySource { get; set; } = InsertedKeySource.UrlEnd;

		public int Submit(IWebDriver driver, Abstract.Environment environment)
		{
			environment.NavigateTo(driver, Url);

			foreach (var keyPair in this)
			{
				var element = driver.FindElement(By.Id(keyPair.Key));
				element.SendKeys(keyPair.Value);
			}
			
			var form = driver.FindElement(By.Id(_formElementId));
			form.Submit();

			switch (KeySource)
			{
				case InsertedKeySource.None:
					return 0;

				case InsertedKeySource.UrlEnd:
					return int.Parse(driver.Url.Split('/').Last());
			}

			throw new NotImplementedException();
		}
	}
}