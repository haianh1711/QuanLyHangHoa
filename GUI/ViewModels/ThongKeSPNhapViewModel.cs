
using BLL;
using CommunityToolkit.Mvvm.ComponentModel;
using DTO;
using GUI.ViewModels.UserControls;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace GUI.ViewModels
{

    public class DataPoint
    {
        public double Value { get; set; } 
        public string? Label { get; set; } 
    }

    partial class ThongKeSPNhapViewModel : ObservableObject
    {
        [ObservableProperty]
        ThongBaoViewModel thongBaoVM = new();

        [ObservableProperty]
        public ObservableCollection<string> danhSachLoc = [ "Tuần", "Tháng", "Năm" ];

        [ObservableProperty]
        private string luaChonLoc;

        [ObservableProperty]
        private string hienThiLuaChonLoc;

        private static ThongKeNhapBLL thongKeBLL = new();

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

        public ThongKeSPNhapViewModel()
        {
            Data = new ObservableCollection<HangHoaDTO>(thongKeBLL.GetHangHoaThongKe());
            luaChonLoc = "Tuần";
            LoadLineChartSeries(thongKeBLL.GetThongKePhieuNhapHangTuanData());
            LoadBarChartSeries();
        }

        private void LoadBarChartSeries()
        {
            List<PhieuNhapDTO> phieuNhapDTOs = new();
            string[] dsTenSanPham = phieuNhapDTOs.Select(phieunhap => phieunhap.MaHang ?? string.Empty).ToArray();
            double[]  dsSoLuongNhap = phieuNhapDTOs.Select(phieunhap => (double?)phieunhap.SoLuongNhap ?? 0).ToArray();

            if(dsSoLuongNhap.Length == 0)
            {
                return;
            }
            double maxSoLuongNhap = dsSoLuongNhap.Max();
            double[] dsSoLuongNhapMax = dsSoLuongNhap.Select(phieunhap => maxSoLuongNhap).ToArray();

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

        private void LoadLineChartSeries(List<ThongKePhieuNhapDTO> thongKePhieus)
        {
            DataPoint[] dataPoints = thongKePhieus.Select(tk => new DataPoint() {
                Value = tk.TongSoLuongNhap ?? 0,
                Label = tk.ThangNam ?? ""
            } ).ToArray();
            string[] dsThoiGian = thongKePhieus.Select(tk => tk.ThangNam ?? "").ToArray();

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
            string label = point.Model.Label;  // Lấy Label đúng cách
            double value = point.Model.Value;  // Lấy giá trị Y

            MessageBox.Show($"Label: {label}\nValue: {value}");
        }

        partial void OnLuaChonLocChanged(string value)
        {
            List<ThongKePhieuNhapDTO> thongKePhieus = new();
            switch (value)
            {
                case "Tuần":
                    thongKePhieus = thongKeBLL.GetThongKePhieuNhapHangTuanData();

                    break;
                case "Tháng":
                    thongKePhieus = thongKeBLL.GetThongKePhieuNhapHangThangData();

                    break;
                case "Năm":
                    thongKePhieus = thongKeBLL.GetThongKePhieuNhapHangNamData();

                    break;
                default:
                    LuaChonLoc = "Tuần";
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
            }
        }

    }
}