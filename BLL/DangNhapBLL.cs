using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.DangNhapBLL;
using DAL;
using DTO;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Util.Store;


namespace BLL
{
    public class DangNhapBLL
    {
        private DangNhapDAL dangNhapDAL = new DangNhapDAL();

        private NhanVienDAL nhanVienDAL = new NhanVienDAL(); // Thêm DAL để lấy dữ liệu nhân viên

        public async Task<NhanVienDTO?> TimNhanVienTheoGmail(string email)
        {
            return await Task.Run(() => nhanVienDAL.TimNhanVienTheoGmail(email));
        }

        // Hàm đăng nhập Gmail bất đồng bộ
        public async Task<TaiKhoanDTO?> DangNhapGmail(string GoogleClientId, string GoogleClientSecret)
        {
            
            ClientSecrets clientSecrets = new ClientSecrets
            {
				ClientId = GoogleClientId,
				ClientSecret = GoogleClientSecret
			};

            string[] Scopes = { GmailService.Scope.GmailReadonly };
            string ApplicationName = "WPF Gmail Login Example";
            UserCredential credential;

            try
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new NullDataStore()

                );

                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                var profile = await service.Users.GetProfile("me").ExecuteAsync();

                // Kiểm tra Gmail có tồn tại trong hệ thống không
                return dangNhapDAL.DangNhapGmail(profile.EmailAddress);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi đăng nhập Gmail: " + ex.Message);
                return null;
            }
        }
    }
}
