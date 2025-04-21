using GUI.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Helper
{
	public class ConfigHelper
	{
		public static string GetClientId()
		{
			var encrypted = ConfigurationManager.AppSettings["ClientId"];
			if (encrypted == null)
				throw new InvalidOperationException("ClientId chưa được cấu hình.");

			return EncryptionHelper.Decrypt(encrypted); // ⚠️ Phải giải mã!
		}

		public static string GetClientSecret()
		{
			var encrypted = ConfigurationManager.AppSettings["ClientSecret"];
			if (encrypted == null)
				throw new InvalidOperationException("ClientSecret chưa được cấu hình.");

			return EncryptionHelper.Decrypt(encrypted); // ⚠️ Phải giải mã!
		}



	}
}
