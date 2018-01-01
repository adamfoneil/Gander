using OpenQA.Selenium;
using System.Collections.Generic;
using System.Data;
using System;
using System.Linq;
using System.Security.Cryptography;
using AdamOneilSoftware;
using System.Text;

namespace Gander.Unit
{
	public enum InsertedKeySource
	{
		/// <summary>
		/// Inserted key can be found at the very end of URL (i.e. MVC insert /Controller/Action/{key})
		/// </summary>
		UrlEnd,

		/// <summary>
		/// Inserted key can be found in the URL by regular expression
		/// </summary>
		UrlRegex,

		/// <summary>
		/// Key is embedded in the page somewhere within a span element
		/// </summary>
		SpanId,

		/// <summary>
		/// Inserted key must be queried via @@IDENTITY or some such (not necessarily reliable)
		/// </summary>
		Query
	}

	public abstract class SeleniumHelper
	{
		protected static string CombineUrl(params string[] urlParts)
		{
			return string.Join("/", urlParts.Select(s => s.TrimStart('/').TrimEnd('/')));
		}

		protected abstract IEnumerable<Environment> GetEnvironments();

		protected abstract IEnumerable<IWebDriver> GetWebDrivers();

		/// <summary>
		/// Describes the URL, form element ID, and fields encapsulated on the form. When defining the fields, leave the values blank,
		/// since they will be set from a <see cref="Credentials"/> object at runtime
		/// </summary>
		protected abstract Form LoginForm { get; }

		protected abstract string LogoffUrl { get; }

		protected void Login(IWebDriver driver, Environment env, Credentials login)
		{
			env.NavigateTo(driver, LoginForm.Url);
			login.MapToForm(LoginForm);
			LoginForm.Submit(driver);
		}

		protected int SubmitForm(IWebDriver driver, Environment env, Form form, InsertedKeySource keySource = InsertedKeySource.UrlEnd)
		{
			if (form.IsAuthenticationRequired)
			{
				Login(driver, env, env.DefaultCredentials());
			}

			env.NavigateTo(driver, form.Url);
			form.Submit(driver);

			switch (keySource)
			{
				case InsertedKeySource.UrlEnd:
					return int.Parse(driver.Url.Split('/').Last());
			}

			throw new NotImplementedException();
		}		

		protected abstract class Environment
		{
			public string Name { get; protected set; }
			public string Url { get; protected set; }

			public abstract IDbConnection GetConnection();
			public abstract Credentials DefaultCredentials();

			public string GetUrl(string url)
			{
				return CombineUrl(Url, url);
			}

			public INavigation NavigateTo(IWebDriver driver, string url)
			{
				driver.Url = GetUrl(url);
				return driver.Navigate();
			}
		}

		protected class Form : Dictionary<string, string>
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

			public void Submit(IWebDriver driver)
			{
				foreach (var keyPair in this)
				{
					var element = driver.FindElement(By.Id(keyPair.Key));
					element.SendKeys(keyPair.Value);
				}

				var form = driver.FindElement(By.Id(_formElementId));
				form.Submit();
			}
		}

		protected abstract class Credentials
		{
			public string UserName { get; set; }
			public string Password { get; set; }

			public static Credentials FromFile(string fileName, DataProtectionScope? protectionScope)
			{
				Credentials result = XmlSerializerHelper.Load<Credentials>(fileName);
				Decrypt(protectionScope, result);
				return result;
			}

			private static void Decrypt(DataProtectionScope? protectionScope, Credentials result)
			{
				if (protectionScope.HasValue)
				{
					result.UserName = SeleniumHelper.Decrypt(result.UserName, protectionScope.Value);
					result.Password = SeleniumHelper.Decrypt(result.Password, protectionScope.Value);
				}
			}

			public void SaveAs(string fileName, DataProtectionScope? protectionScope)
			{
				if (protectionScope.HasValue)
				{
					UserName = Encrypt(UserName);
					Password = Encrypt(Password);
				}

				XmlSerializerHelper.Save(this, fileName);

				Decrypt(protectionScope, this);
			}

			/// <summary>
			/// Assigns the UserName and Password properties to form field element Ids
			/// </summary>			
			public abstract void MapToForm(Form form);
		}

		protected static string Encrypt(string clearText, DataProtectionScope scope = DataProtectionScope.CurrentUser)
		{
			byte[] clearBytes = Encoding.ASCII.GetBytes(clearText);
			byte[] encryptedBytes = ProtectedData.Protect(clearBytes, null, scope);
			return Convert.ToBase64String(encryptedBytes);
		}

		protected static string Decrypt(string encryptedText, DataProtectionScope scope = DataProtectionScope.CurrentUser)
		{
			byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
			byte[] clearBytes = ProtectedData.Unprotect(encryptedBytes, null, scope);
			return Encoding.ASCII.GetString(clearBytes);
		}
	}
}