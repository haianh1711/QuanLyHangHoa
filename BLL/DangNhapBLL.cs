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

        public TaiKhoanDTO? DangNhap(string tenTaiKhoan, string matKhau)
        {

            var taiKhoan = dangNhapDAL.DangNhap(tenTaiKhoan, matKhau);
            return taiKhoan;
        }

        // hàm bất đồng bộ, khi gán biến nhớ dùng await
        public async Task<TaiKhoanDTO?> DangNhapGmail()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ClientId = "561350745742-iugftt8jlt86vnn2at518cbqq1f7tov3.apps.googleusercontent.com",
                ClientSecret = "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"
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

                return dangNhapDAL.DangNhapGmail(profile.EmailAddress);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi đăng nhập: " + ex.Message);
                return null;
            }

        }
    }
}
