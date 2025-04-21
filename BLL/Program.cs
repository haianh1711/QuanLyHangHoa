using System.Security.Cryptography;
using System.Text;


	byte[] data = Encoding.UTF8.GetBytes("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");
	byte[] encryptedData = ProtectedData.Protect(
		data, null, DataProtectionScope.CurrentUser);
	Console.WriteLine(Convert.ToBase64String(encryptedData));
