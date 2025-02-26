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
    public class DataPoint
    {
        public double Value { get; set; }
        public string? Label { get; set; }
        public int? Tuan { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }
    }

    partial class ThongKeSPXuatViewModel : ObservableObject
    {
        [ObservableProperty]
        ThongBaoViewModel thongBaoVM = new();

        [ObservableProperty]
        public ObservableCollection<string> danhSachLoc = ["Tuần","Tháng", "Năm"];

        [ObservableProperty]
        private string luaChonLoc;

        [ObservableProperty]
        private string hienThiLuaChonLoc;

        [ObservableProperty]
        private string thoiGianHienThi;

        [ObservableProperty]
        private string tuKhoaTimKiem;

        private ThongKeXuatBLL thongKeBLL = new();

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

        public ThongKeSPXuatViewModel()
        {
            Data = new ObservableCollection<HangHoaDTO>(thongKeBLL.GetHangHoaThongKe());
            luaChonLoc = "Lọc the";

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

        private void LoadBarChartSeries(List<PhieuXuatDTO> PhieuXuatDTOs)
        {
            string[] dsTenSanPham = PhieuXuatDTOs.Select(PhieuXuat => PhieuXuat.MaHang ?? string.Empty).ToArray();
            double[] dsSoLuongNhap = PhieuXuatDTOs.Select(PhieuXuat => (double?)PhieuXuat.SoLuongXuat ?? 0).ToArray();

            if (dsSoLuongNhap.Length == 0)
            {
                ChartHeight = 0;
                return;
            }
            double maxSoLuongNhap = dsSoLuongNhap.Max();
            double[] dsSoLuongNhapMax = dsSoLuongNhap.Select(PhieuXuat => maxSoLuongNhap).ToArray();

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

        private List<ThongKePhieuXuatDTO> LayDanhSachNam(List<ThongKePhieuXuatDTO> danhSach)
        {
            int minNam = danhSach.Min(tk => tk.Nam ?? 0);
            int maxNam = danhSach.Max(tk => tk.Nam ?? 0);

            List<ThongKePhieuXuatDTO> danhSachNam = Enumerable.Range(minNam, maxNam - minNam + 1)
                                                    .Select(nam => new ThongKePhieuXuatDTO()
                                                    {
                                                        Nam = nam,
                                                        Thang = 0,
                                                        TongSoLuongXuat = danhSach.FirstOrDefault(tk => tk.Nam == nam)?.TongSoLuongXuat ?? 0,
                                                        HienThi = $"{nam}"
                                                    })
                                                    .ToList();

            return danhSachNam;
        }

        private List<ThongKePhieuXuatDTO> LayDanhSachThangNam(List<ThongKePhieuXuatDTO> danhSach)
        {
            if (danhSach == null || danhSach.Count == 0)
                return new List<ThongKePhieuXuatDTO>();

            int minNam = danhSach.Min(tk => tk.Nam ?? int.MaxValue);
            int maxNam = danhSach.Max(tk => tk.Nam ?? int.MinValue);

            if (minNam == int.MaxValue || maxNam == int.MinValue)
                return new List<ThongKePhieuXuatDTO>();

            var danhSachThangNam = new List<ThongKePhieuXuatDTO>();

            // Lặp qua từng năm
            for (int nam = minNam; nam <= maxNam; nam++)
            {
                for (int thang = 1; thang <= 12; thang++)
                {
                    var data = danhSach.FirstOrDefault(tk => tk.Nam == nam && tk.Thang == thang);

                    danhSachThangNam.Add(new ThongKePhieuXuatDTO
                    {
                        Nam = nam,
                        Thang = thang,
                        TongSoLuongXuat = data?.TongSoLuongXuat ?? 0, // Nếu không có, đặt bằng 0
                        HienThi = $"{thang:D2}/{nam}"
                    });
                }
            }

            return danhSachThangNam;
        }

        private List<ThongKePhieuXuatDTO> LayDanhSachTuanNam(List<ThongKePhieuXuatDTO> danhSach)
        {
            if (danhSach == null || danhSach.Count == 0)
                return new List<ThongKePhieuXuatDTO>();

            // Tìm năm và tuần nhỏ nhất từ dữ liệu
            int minNam = danhSach.Min(tk => tk.Nam ?? int.MaxValue);
            int minTuan = danhSach.Where(tk => tk.Nam == minNam).Min(tk => tk.Tuan ?? int.MaxValue);

            // Tìm năm và tuần lớn nhất từ dữ liệu
            int maxNam = danhSach.Max(tk => tk.Nam ?? int.MinValue);
            int maxTuan = danhSach.Where(tk => tk.Nam == maxNam).Max(tk => tk.Tuan ?? int.MinValue);

            if (minNam == int.MaxValue || maxNam == int.MinValue || minTuan == int.MaxValue || maxTuan == int.MinValue)
                return new List<ThongKePhieuXuatDTO>();

            var danhSachTuanNam = new List<ThongKePhieuXuatDTO>();

            // Lặp từ năm nhỏ nhất đến năm lớn nhất
            for (int nam = minNam; nam <= maxNam; nam++)
            {
                int startTuan = (nam == minNam) ? minTuan : 1; // Bắt đầu từ tuần nhỏ nhất nếu là năm minNam
                int endTuan = (nam == maxNam) ? maxTuan : 52;  // Kết thúc ở tuần lớn nhất nếu là năm maxNam

                for (int tuan = startTuan; tuan <= endTuan; tuan++)
                {
                    var data = danhSach.FirstOrDefault(tk => tk.Nam == nam && tk.Tuan == tuan);

                    danhSachTuanNam.Add(new ThongKePhieuXuatDTO
                    {
                        Nam = nam,
                        Tuan = tuan,
                        Thang = data?.Thang ?? 1, // Giữ mặc định nếu cần
                        TongSoLuongXuat = data?.TongSoLuongXuat ?? 0,
                        HienThi = $"Tuần {tuan}/{nam}"
                    });
                }
            }

            return danhSachTuanNam;
        }




        private void LoadLineChartSeries(List<ThongKePhieuXuatDTO> thongKePhieus)
        {
            DataPoint[] dataPoints = thongKePhieus.Select(tk => new DataPoint()
            {
                Value = tk.TongSoLuongXuat ?? 0,
                Label = tk.HienThi ?? "",
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
            List<PhieuXuatDTO> PhieuXuatDTOs;

            switch (LuaChonLoc)
            {
                case "Tuần":
                    PhieuXuatDTOs = thongKeBLL.GetThongKePhieuXuatHangHoaTheoTuanData(point.Model.Tuan.ToString(), point.Model.Thang.ToString(), point.Model.Nam.ToString());
                    ThoiGianHienThi = $"Sản phẩm xuất trong tuần {point.Model.Tuan.ToString()}/{point.Model.Nam.ToString()}";
                    LoadBarChartSeries(PhieuXuatDTOs);

                    break;

                case "Tháng":
                    PhieuXuatDTOs = thongKeBLL.GetThongKePhieuXuatHangHoaTheoThangData(point.Model.Thang.ToString(), point.Model.Nam.ToString());
                    ThoiGianHienThi = $"Sản phẩm xuất trong tháng {point.Model.Thang.ToString()}/{point.Model.Nam.ToString()}";
                    LoadBarChartSeries(PhieuXuatDTOs);

                    break;
                case "Năm":
                    PhieuXuatDTOs = thongKeBLL.GetThongKePhieuXuatHangHoaTheoNamData(point.Model.Nam.ToString());
                    ThoiGianHienThi = $"Sản phẩm xuất trong năm {point.Model.Thang.ToString()}/{point.Model.Nam.ToString()}";
                    LoadBarChartSeries(PhieuXuatDTOs);
                    break;
                default:
                    break;
            }

        }

        partial void OnLuaChonLocChanged(string value)
        {
            List<ThongKePhieuXuatDTO> thongKePhieus = new();
            switch (value)
            {
                case "Tuần":
                    thongKePhieus = thongKeBLL.GetThongKePhieuXuatHangTuanData();
                    thongKePhieus = LayDanhSachTuanNam(thongKePhieus);
                    break;
                case "Tháng":
                    thongKePhieus = thongKeBLL.GetThongKePhieuXuatHangThangData();
                    thongKePhieus = LayDanhSachThangNam(thongKePhieus);
                    break;
                case "Năm":
                    thongKePhieus = thongKeBLL.GetThongKePhieuXuatHangNamData();
                    thongKePhieus = LayDanhSachNam(thongKePhieus);
                    break;
                default:
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
