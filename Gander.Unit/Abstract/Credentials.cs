using AdamOneilSoftware;
using System.Security.Cryptography;

namespace Gander.Unit
{
	public abstract class Credentials
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
				UserName = SeleniumHelper.Encrypt(UserName);
				Password = SeleniumHelper.Encrypt(Password);
			}

			XmlSerializerHelper.Save(this, fileName);

			Decrypt(protectionScope, this);
		}

		/// <summary>
		/// Assigns the UserName and Password properties to form field element Ids
		/// </summary>
		public abstract Form MapToForm(Form loginForm);
	}
}