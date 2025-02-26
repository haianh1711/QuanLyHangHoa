using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

using SkiaSharp;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Kernel.Sketches;

using DTO;
using LiveChartsCore.Defaults;
using BLL;
using GUI.ViewModels.UserControls;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using DAL;
using GUI.Views.UserControls;

namespace GUI.ViewModels
{
    partial class ThongKeSPNhapViewModel : ObservableObject
    {
        [ObservableProperty]
        ThongBaoViewModel thongBaoVM = new();

        [ObservableProperty]
        public ObservableCollection<string> danhSachLoc = ["Tuần", "Tháng", "Năm"];

        [ObservableProperty]
        private string luaChonLoc;

        [ObservableProperty]
        private string hienThiLuaChonLoc;

        [ObservableProperty]
        private string thoiGianHienThi;

        [ObservableProperty]
        private string tuKhoaTimKiem;

        private ThongKeNhapBLL thongKeBLL = new();

        private const int barHeight = 40;
        private const int barSpacing = 5;


        // biểu đồ cột
        [ObservableProperty]
        private ISeries[] barChartSeries;
        [ObservableProperty]
        private ICartesianAxis[] yAxes;
        [ObservableProperty]
        private ICartesianAxis[] xAxes;

        // biểu đồ line
        [ObservableProperty]
        private ISeries[] lineChartSeries;

        [ObservableProperty]
        private ICartesianAxis[] xAxesLine;

        [ObservableProperty]
        private ObservableCollection<HangHoaDTO> data;

        [ObservableProperty]
        private int chartHeight;

        [ObservableProperty]
        private string thongTinNV;

        [ObservableProperty]
        private NhanVienDTO nhanVienDTO;

        public ThongKeSPNhapViewModel()
        {
            Data = new ObservableCollection<HangHoaDTO>(thongKeBLL.GetHangHoaThongKe());
            LuaChonLoc = "Tuần";
            OnPropertyChanged(nameof(LuaChonLoc));

            LoadLineChartSeries(LayDanhSachThangNam(thongKeBLL.GetThongKePhieuNhapHangTuanData()));
        }

        [RelayCommand]
        private async Task TimKiem()
        {
            TuKhoaTimKiem = string.IsNullOrEmpty(TuKhoaTimKiem) ? "" : TuKhoaTimKiem;

            var danhSach = thongKeBLL
                .SearchHangHoa(TuKhoaTimKiem);

            if (danhSach == null || !danhSach.Any())
            {
                await ThongBaoVM.MessageOK($"Không tìm thấy {TuKhoaTimKiem}");
                Data = new ObservableCollection<HangHoaDTO>(thongKeBLL.GetHangHoaThongKe());
                return;
            }
            Data.Clear();
            Data = new ObservableCollection<HangHoaDTO>(danhSach);


        }

        private void LoadBarChartSeries(List<PhieuNhapDTO> PhieuNhapDTOs)
        {
            string[] dsTenSanPham = PhieuNhapDTOs.Select(PhieuNhap => PhieuNhap.MaHang ?? string.Empty).ToArray();
            double[] dsSoLuongNhap = PhieuNhapDTOs.Select(PhieuNhap => (double?)PhieuNhap.SoLuongNhap ?? 0).ToArray();

            if (dsSoLuongNhap.Length == 0)
            {
                ChartHeight = 0;
                return;
            }
            double maxSoLuongNhap = dsSoLuongNhap.Max();
            double[] dsSoLuongNhapMax = dsSoLuongNhap.Select(PhieuNhap => maxSoLuongNhap).ToArray();

            // biểu đồ cột ngang
            BarChartSeries = [
                new RowSeries<double>
                {
                    IsHoverable = false, // disables the series from the tooltips 
                    Values = dsSoLuongNhapMax,
                    Stroke = null,
                    Fill = new SolidColorPaint(new SKColor(30, 30, 30, 30)),
                    IgnoresBarPosition = true
                },
                new RowSeries<double>
                {
                    IsHoverable = true,
                    Values = dsSoLuongNhap,
                    Stroke = null,
                    Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                    IgnoresBarPosition = true,
                    YToolTipLabelFormatter = point => point.Model.ToString("N2")
                }
            ];

            YAxes = [
                new Axis {
                    Labels = dsTenSanPham,
                }
            ];

            XAxes = [
                new Axis {
                    IsVisible = false,
                }
            ];

            UpdateChartHeight();
        }

        private List<ThongKePhieuNhapDTO> LayDanhSachNam(List<ThongKePhieuNhapDTO> danhSach)
        {
            int minNam = danhSach.Min(tk => tk.Nam ?? 0);
            int maxNam = danhSach.Max(tk => tk.Nam ?? 0);

            List<ThongKePhieuNhapDTO> danhSachNam = Enumerable.Range(minNam, maxNam - minNam + 1)
                                                    .Select(nam => new ThongKePhieuNhapDTO()
                                                    {
                                                        Nam = nam,
                                                        Thang = 0,
                                                        TongSoLuongNhap = danhSach.FirstOrDefault(tk => tk.Nam == nam)?.TongSoLuongNhap ?? 0,
                                                        HienThi = $"{nam}"
                                                    })
                                                    .ToList();

            return danhSachNam;
        }

        private List<ThongKePhieuNhapDTO> LayDanhSachThangNam(List<ThongKePhieuNhapDTO> danhSach)
        {
            if (danhSach == null || danhSach.Count == 0)
                return new List<ThongKePhieuNhapDTO>();

            int minNam = danhSach.Min(tk => tk.Nam ?? int.MaxValue);
            int maxNam = danhSach.Max(tk => tk.Nam ?? int.MinValue);

            if (minNam == int.MaxValue || maxNam == int.MinValue)
                return new List<ThongKePhieuNhapDTO>();

            var danhSachThangNam = new List<ThongKePhieuNhapDTO>();

            // Lặp qua từng năm
            for (int nam = minNam; nam <= maxNam; nam++)
            {
                for (int thang = 1; thang <= 12; thang++)
                {
                    var data = danhSach.FirstOrDefault(tk => tk.Nam == nam && tk.Thang == thang);

                    danhSachThangNam.Add(new ThongKePhieuNhapDTO
                    {
                        Nam = nam,
                        Thang = thang,
                        TongSoLuongNhap = data?.TongSoLuongNhap ?? 0, // Nếu không có, đặt bằng 0
                        HienThi = $"{thang:D2}/{nam}"
                    });
                }
            }

            return danhSachThangNam;
        }

        private List<ThongKePhieuNhapDTO> LayDanhSachTuanNam(List<ThongKePhieuNhapDTO> danhSach)
        {
            if (danhSach == null || danhSach.Count == 0)
                return new List<ThongKePhieuNhapDTO>();

            // Tìm năm và tuần nhỏ nhất từ dữ liệu
            int minNam = danhSach.Min(tk => tk.Nam ?? int.MaxValue);
            int minTuan = danhSach.Where(tk => tk.Nam == minNam).Min(tk => tk.Tuan ?? int.MaxValue);

            // Tìm năm và tuần lớn nhất từ dữ liệu
            int maxNam = danhSach.Max(tk => tk.Nam ?? int.MinValue);
            int maxTuan = danhSach.Where(tk => tk.Nam == maxNam).Max(tk => tk.Tuan ?? int.MinValue);

            if (minNam == int.MaxValue || maxNam == int.MinValue || minTuan == int.MaxValue || maxTuan == int.MinValue)
                return new List<ThongKePhieuNhapDTO>();

            var danhSachTuanNam = new List<ThongKePhieuNhapDTO>();

            // Lặp từ năm nhỏ nhất đến năm lớn nhất
            for (int nam = minNam; nam <= maxNam; nam++)
            {
                int startTuan = (nam == minNam) ? minTuan : 1; // Bắt đầu từ tuần nhỏ nhất nếu là năm minNam
                int endTuan = (nam == maxNam) ? maxTuan : 52;  // Kết thúc ở tuần lớn nhất nếu là năm maxNam

                for (int tuan = startTuan; tuan <= endTuan; tuan++)
                {
                    var data = danhSach.FirstOrDefault(tk => tk.Nam == nam && tk.Tuan == tuan);

                    danhSachTuanNam.Add(new ThongKePhieuNhapDTO
                    {
                        Nam = nam,
                        Tuan = tuan,
                        Thang = data?.Thang ?? 1, // Giữ mặc định nếu cần
                        TongSoLuongNhap = data?.TongSoLuongNhap ?? 0,
                        HienThi = $"Tuần {tuan}/{nam}"
                    });
                }
            }

            return danhSachTuanNam;
        }

        private void LoadLineChartSeries(List<ThongKePhieuNhapDTO> thongKePhieus)
        {
            DataPoint[] dataPoints = thongKePhieus.Select(tk => new DataPoint()
            {
                Value = tk.TongSoLuongNhap ?? 0,
                Label = tk.HienThi ?? "",
                Tuan = tk.Tuan,
                Nam = tk.Nam,
                Thang = tk.Thang,
            }).ToArray();
            string[] dsThoiGian = thongKePhieus.Select(tk => tk.HienThi ?? "").ToArray();

            // biểu đồ đường
            var lineSeries = new LineSeries<DataPoint>
            {
                Values = dataPoints,
                Fill = null, // Không tô nền dưới đường
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
                Mapping = (data, index) => new Coordinate(index, data.Value)

            };

            lineSeries.ChartPointPointerDown += OnPointerDown;

            LineChartSeries = new ISeries[] { lineSeries };


            XAxesLine = new Axis[]
            {
                new Axis
                {
                    Labels = dsThoiGian,
                    LabelsRotation = 0 // Giữ chữ nằm ngang
                }
            };
        }

        private void OnPointerDown(IChartView chart, ChartPoint<DataPoint, CircleGeometry, LabelGeometry> point)
        {

            List<PhieuNhapDTO> PhieuNhapDTOs;

            switch (LuaChonLoc)
            {
                case "Tuần":
                    PhieuNhapDTOs = thongKeBLL.GetThongKePhieuNhapHangHoaTheoTuanData(point.Model.Tuan.ToString(), point.Model.Thang.ToString(), point.Model.Nam.ToString());
                    ThoiGianHienThi = $"Sản phẩm xuất trong tuần {point.Model.Tuan.ToString()}/{point.Model.Nam.ToString()}";
                    LoadBarChartSeries(PhieuNhapDTOs);

                    break;

                case "Tháng":
                    PhieuNhapDTOs = thongKeBLL.GetThongKePhieuNhapHangHoaTheoThangData(point.Model.Thang.ToString(), point.Model.Nam.ToString());
                    ThoiGianHienThi = $"Sản phẩm xuất trong tháng {point.Model.Thang.ToString()}/{point.Model.Nam.ToString()}";
                    LoadBarChartSeries(PhieuNhapDTOs);

                    break;
                case "Năm":
                    PhieuNhapDTOs = thongKeBLL.GetThongKePhieuNhapHangHoaTheoNamData(point.Model.Nam.ToString());
                    ThoiGianHienThi = $"Sản phẩm xuất trong năm {point.Model.Thang.ToString()}/{point.Model.Nam.ToString()}";
                    LoadBarChartSeries(PhieuNhapDTOs);
                    break;
                default:
                    LuaChonLoc = "Tháng";
                    break;
            }

        }

        partial void OnLuaChonLocChanged(string value)
        {
            List<ThongKePhieuNhapDTO> thongKePhieus = new();
            switch (value)
            {
                case "Tuần":
                    thongKePhieus = thongKeBLL.GetThongKePhieuNhapHangTuanData();
                    thongKePhieus = LayDanhSachTuanNam(thongKePhieus);
                    break;
                case "Tháng":
                    thongKePhieus = thongKeBLL.GetThongKePhieuNhapHangThangData();
                    thongKePhieus = LayDanhSachThangNam(thongKePhieus);
                    break;
                case "Năm":
                    thongKePhieus = thongKeBLL.GetThongKePhieuNhapHangNamData();
                    thongKePhieus = LayDanhSachNam(thongKePhieus);
                    break;
                default:
                    LuaChonLoc = "Tháng";
                    break;
            }
            HienThiLuaChonLoc = "Lọc theo " + LuaChonLoc;
            LoadLineChartSeries(thongKePhieus);
        }

        private void UpdateChartHeight()
        {
            if (BarChartSeries != null && BarChartSeries[0].Values != null)
            {
                ChartHeight = (barHeight + barSpacing) * BarChartSeries[0].Values.Cast<double>().Count();
                ChartHeight = ChartHeight > 166 ? ChartHeight : 166;
            }
        }

    }
}
