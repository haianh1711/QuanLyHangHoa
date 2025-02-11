using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GUI.ViewModels.UserControls
{
    public partial class ThongBaoViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private string? messageText;

        [ObservableProperty]
        private bool isMessageVisible = false;

        [ObservableProperty]
        private bool isYesNoMessage;

        [ObservableProperty]
        private bool isOKMessage;

        private TaskCompletionSource<bool>? taskCompletionSource;

        public ThongBaoViewModel()
        {
            IsMessageVisible = false;
            IsYesNoMessage = false;  // Mặc định không phải hộp thoại Yes/No
            IsOKMessage = false;
        }

        public void MessageTest(string message)
        {
            IsMessageVisible = true;
            MessageText = message;
            IsYesNoMessage = true;
            IsOKMessage = false;
            Debug.WriteLine($"[ThongBaoViewModel] MessageText: {MessageText}"); // Kiểm tra giá trị
            Debug.WriteLine($"[ThongBaoViewModel] IsMessageVisible: {IsMessageVisible}");

        }

        // Hiển thị hộp thoại Yes/No và đợi phản hồi
        public async Task<bool> MessageYesNo(string message)
        {
            IsMessageVisible = true;
            MessageText = message;
            IsYesNoMessage = true;
            IsOKMessage = false;
            Debug.WriteLine($"[ThongBaoViewModel] MessageText: {MessageText}"); // Kiểm tra giá trị
            Debug.WriteLine($"[ThongBaoViewModel] IsMessageVisible: {IsMessageVisible}");

            taskCompletionSource = new();
            return await taskCompletionSource.Task;
        }

        // Hiển thị hộp thoại OK
        public async Task<bool> MessageOK(string message)
        {
            IsMessageVisible = true;
            MessageText = message;
            IsOKMessage = true;
            IsYesNoMessage = false;

            Debug.WriteLine($"[ThongBaoViewModel] MessageText: {MessageText}"); // Kiểm tra giá trị
            Debug.WriteLine($"[ThongBaoViewModel] IsMessageVisible: {IsMessageVisible}");

            taskCompletionSource = new();
            return await taskCompletionSource.Task;
        }


        [RelayCommand]
        private void OK()
        {
            IsMessageVisible = false;
            taskCompletionSource?.SetResult(true);
        }

        [RelayCommand]
        private void Yes()
        {
            IsMessageVisible = false;
            taskCompletionSource?.SetResult(true);
        }

        [RelayCommand]
        private void No()
        {
            IsMessageVisible = false;
            taskCompletionSource?.SetResult(false);
        }
    }
}
