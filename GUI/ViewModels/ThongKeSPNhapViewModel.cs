
using BLL;
using CommunityToolkit.Mvvm.ComponentModel;
using DTO;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace GUI.ViewModels
{
    partial class ThongKeSPNhapViewModel : ObservableObject
    {
        private static ThongKeBLL thongKeBLL = new();

        private const int barHeight = 40;
        private const int barSpacing = 5;

        private static string[]  dsTenSanPham = [];
        private static double[] dsSoLuongNhap = [];
        private static double[] dsSoLuongNhapMax = [];
        private double maxSoLuongNhap;

        [ObservableProperty]
        private ObservableCollection<SanPhamDTO> data;

        [ObservableProperty]
        private int chartHeight;

        [ObservableProperty]
        private string thongTinNV;

        public ThongKeSPNhapViewModel()
        {
            Data = new ObservableCollection<SanPhamDTO>(thongKeBLL.GetSanPhamThongKe());

            List<PhieuNhapDTO> phieuNhapDTOs = thongKeBLL.GetThongKePhieuNhapSanPhamData();
            dsTenSanPham = phieuNhapDTOs.Select(phieunhap => phieunhap.MaSanPham ?? String.Empty).ToArray();
            dsSoLuongNhap = phieuNhapDTOs.Select(phieunhap => (double?)phieunhap.SoLuongNhap ?? 0).ToArray();

            maxSoLuongNhap = dsSoLuongNhap.Max();
            dsSoLuongNhapMax = dsSoLuongNhap.Select(phieunhap => maxSoLuongNhap).ToArray();

            // Cập nhật chiều cao ban đầu ban đầu
            UpdateChartHeight();

            thongTinNV = "TD00997 - Nguyễn Hải Anh";
        }

        // biểu đồ cột ngang
        public ISeries[] Series { get; set; } = [
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

        public ICartesianAxis[] YAxes { get; set; } = [
            new Axis { 
                Labels = dsTenSanPham,
            }
        ];

        public ICartesianAxis[] XAxes { get; set; } = [
            new Axis {
                IsVisible = false,
            }
        ];

        private void UpdateChartHeight()
        {
            if (Series != null && Series[0].Values != null)
            {
                ChartHeight = (barHeight + barSpacing) * Series[0].Values.Cast<double>().Count();
            }
        }

        // biểu đồ đường
        public ISeries[] SeriesLine { get; set; } = [
            new LineSeries<ObservablePoint>
            {
                Values = [
                    new ObservablePoint(1, 4),
                    new ObservablePoint(2, 3),
                    new ObservablePoint(3, 8),
                    new ObservablePoint(4, 6),
                    new ObservablePoint(5, 1),
                    new ObservablePoint(6, 1),
                    new ObservablePoint(7, 22),
                    new ObservablePoint(8, 30),
                    new ObservablePoint(9, 1),
                    new ObservablePoint(10, 10),
                    new ObservablePoint(11, 1),
                    new ObservablePoint(12, 35),
                ],
            }
        ];

        

    }
}