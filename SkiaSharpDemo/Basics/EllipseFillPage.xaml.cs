using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SkiaSharpDemo.Basics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EllipseFillPage : ContentPage
    {
        /// <summary>
        /// 椭圆
        /// </summary>
        public EllipseFillPage()
        {
            InitializeComponent();
        }

        private void OnCanvasViewPaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            float strokeWidth = 50;
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Blue,
                StrokeWidth = strokeWidth
            };

            //float xRadius = (info.Width - strokeWidth) / 2;
            //float yRadius = (info.Height - strokeWidth) / 2;
            //canvas.DrawOval(info.Width / 2, info.Height / 2, xRadius, yRadius, paint);


            //另一种方式
            //SKRect rect = new SKRect(0, 0, info.Width, info.Height);
            SKRect rect = new SKRect(strokeWidth/2, strokeWidth/2, info.Width-strokeWidth/2, info.Height-strokeWidth/2);
            canvas.DrawOval(rect, paint);

        }
    }
}