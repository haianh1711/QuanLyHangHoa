using System;
using System.Security.Cryptography;
using System.Text;

namespace GUI.Helpers
{
	public static class EncryptionHelper
	{
		public static string Encrypt(string plainText)
		{
			byte[] data = Encoding.UTF8.GetBytes(plainText);
			byte[] encryptedData = ProtectedData.Protect(
				data, null, DataProtectionScope.CurrentUser);
			return Convert.ToBase64String(encryptedData);
		}

		public static string Decrypt(string encryptedText)
		{
			byte[] data = Convert.FromBase64String(encryptedText);
			byte[] decryptedData = ProtectedData.Unprotect(
				data, null, DataProtectionScope.CurrentUser);
			return Encoding.UTF8.GetString(decryptedData);
		}
	}
}
