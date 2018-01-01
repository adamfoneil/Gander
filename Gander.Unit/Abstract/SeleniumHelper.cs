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
		Query,
		/// <summary>
		/// Placeholder value 0 is returned
		/// </summary>
		None
	}

	public abstract class SeleniumHelper
	{
		public static string CombineUrl(params string[] urlParts)
		{
			return string.Join("/", urlParts.Select(s => s.TrimStart('/').TrimEnd('/')));
		}

		protected abstract IEnumerable<Abstract.Environment> GetEnvironments();

		protected abstract IEnumerable<IWebDriver> GetWebDrivers();

		/// <summary>
		/// Describes the URL, form element ID, and fields encapsulated on the form. When defining the fields, leave the values blank,
		/// since they will be set from a <see cref="Credentials"/> object at runtime
		/// </summary>
		protected abstract Form LoginForm();

		protected abstract string LogoffUrl { get; }

		protected void Login(IWebDriver driver, Abstract.Environment env)
		{
			Login(driver, env, env.DefaultCredentials());
		}

		protected void Login(IWebDriver driver, Abstract.Environment env, Credentials login)
		{			
			var form = login.MapToForm(LoginForm());			
			form.Submit(driver, env);
		}

		protected void EnumDriversAndEnvironments(Action<IWebDriver, Abstract.Environment> action)
		{
			foreach (var driver in GetWebDrivers())
			{
				foreach (var env in GetEnvironments())
				{
					try
					{
						action.Invoke(driver, env);
					}
					catch (Exception exc)
					{
						throw new Exception($"Failure with {driver.GetType().Name} in {env.Name} environment: {exc.Message}", exc);
					}					
				}
			}
		}

		public static string Encrypt(string clearText, DataProtectionScope scope = DataProtectionScope.CurrentUser)
		{
			byte[] clearBytes = Encoding.ASCII.GetBytes(clearText);
			byte[] encryptedBytes = ProtectedData.Protect(clearBytes, null, scope);
			return Convert.ToBase64String(encryptedBytes);
		}

		public static string Decrypt(string encryptedText, DataProtectionScope scope = DataProtectionScope.CurrentUser)
		{
			byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
			byte[] clearBytes = ProtectedData.Unprotect(encryptedBytes, null, scope);
			return Encoding.ASCII.GetString(clearBytes);
		}

		public static void EncryptStringToFile(string fileName, string content, DataProtectionScope protectionScope)
		{
			fileName = ResolveFilename(fileName);
			content = Encrypt(content, protectionScope);			
			XmlSerializerHelper.Save(content, fileName);
		}

		private static string ResolveFilename(string fileName)
		{
			Dictionary<string, string> replace = new Dictionary<string, string>()
			{
				{ "mydocs", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) }
			};

			string result = fileName;

			foreach (var keyPair in replace) result = result.Replace($"%{keyPair.Key}%", keyPair.Value);

			return result;
		}

		public static string DecryptStringFromFile(string fileName, DataProtectionScope protectionScope)
		{
			fileName = ResolveFilename(fileName);
			string content = XmlSerializerHelper.Load<string>(fileName);			
			return Decrypt(content, protectionScope);			
		}
	}
}