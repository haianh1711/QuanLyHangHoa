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

namespace GUI.ViewModels
{
    partial class ThongKeSPTonKhoViewModel :ObservableObject
    {

        private const int barHeight = 40;
        private const int barSpacing = 5;

        [ObservableProperty]
        private ObservableCollection<SanPhamDTO> data;

        [ObservableProperty]
        private int chartHeight;

        public ThongKeSPTonKhoViewModel()
        {
            Data = new ObservableCollection<SanPhamDTO>
            {
                new SanPhamDTO("SP001", "Sản phẩm 1", 10000.50, 50, "Mô tả sản phẩm 1"),
                new SanPhamDTO("SP002", "Sản phẩm 2", 20000.75, 30, "Mô tả sản phẩm 2"),
                new SanPhamDTO("SP003", "Sản phẩm 3", 15000.00, 20, "Mô tả sản phẩm 3"),
                new SanPhamDTO("SP004", "Sản phẩm 4", 25000.60, 40, "Mô tả sản phẩm 4")
            };


            // Cập nhật dữ liệu ban đầu
            UpdateChartHeight();
        }

        public ISeries[] Series { get; set; } = [
            new RowSeries<double>
            {
                IsHoverable = false, // disables the series from the tooltips 
                Values = [10, 10, 10, 10, 10, 10, 10],
                Stroke = null,
                Fill = new SolidColorPaint(new SKColor(30, 30, 30, 30)),
                IgnoresBarPosition = true
            },
            new RowSeries<double>
            {
                IsHoverable = true,
                Values = [3, 10, 5, 3, 7, 3, 8],
                Stroke = null,
                Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                IgnoresBarPosition = true,
                YToolTipLabelFormatter = point => point.Model.ToString("N2")
            }
        ];

        public ICartesianAxis[] YAxes { get; set; } = [
            new Axis {
                Labels = new[]{"HH001", "HH002", "HH003", "HH004", "HH005", "HH006", "HH007"},
            }
        ];

        public ISeries[] SeriesLine { get; set; } = [
        new LineSeries<ObservablePoint>
        {
            Values = [
                new ObservablePoint(0, 4),
                new ObservablePoint(1, 3),
                new ObservablePoint(3, 8),
                new ObservablePoint(18, 6),
                new ObservablePoint(20, 12)
                ]
            }
        ];

        private void UpdateChartHeight()
        {
            if (Series != null && Series[0].Values != null)
            {
                ChartHeight = (barHeight + barSpacing) * Series[0].Values.Cast<double>().Count();
            }
        }

    }
}
