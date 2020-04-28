using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SkiaSharpDemo.WoSign
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DemoPage : ContentPage
    {
        public DemoPage()
        {
            InitializeComponent();

            
            SKCanvasView canvasView = new SKCanvasView();
            //canvasView.BackgroundColor = Color.Gray;
            canvasView.PaintSurface += CanvasView_PaintSurface; ;
            Content = canvasView;
        }

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            using (var surface = SKSurface.Create(info))
            {
                SKCanvas canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                // set up drawing tools
                var paint = new SKPaint
                {
                    IsAntialias = true,
                    Color = new SKColor(0x2c, 0x3e, 0x50),
                    StrokeCap = SKStrokeCap.Round
                };

                // create the Xamagon path
                var path = new SKPath();
                path.MoveTo(71.4311121f, 56f);
                path.CubicTo(68.6763107f, 56.0058575f, 65.9796704f, 57.5737917f, 64.5928855f, 59.965729f);
                path.LineTo(43.0238921f, 97.5342563f);
                path.CubicTo(41.6587026f, 99.9325978f, 41.6587026f, 103.067402f, 43.0238921f, 105.465744f);
                path.LineTo(64.5928855f, 143.034271f);
                path.CubicTo(65.9798162f, 145.426228f, 68.6763107f, 146.994582f, 71.4311121f, 147f);
                path.LineTo(114.568946f, 147f);
                path.CubicTo(117.323748f, 146.994143f, 120.020241f, 145.426228f, 121.407172f, 143.034271f);
                path.LineTo(142.976161f, 105.465744f);
                path.CubicTo(144.34135f, 103.067402f, 144.341209f, 99.9325978f, 142.976161f, 97.5342563f);
                path.LineTo(121.407172f, 59.965729f);
                path.CubicTo(120.020241f, 57.5737917f, 117.323748f, 56.0054182f, 114.568946f, 56f);
                path.LineTo(71.4311121f, 56f);
                path.Close();

                // draw the Xamagon path
                canvas.DrawPath(path, paint);
            }
        }


        private void Init()
        {
            var info = new SKImageInfo(640, 480);
            using (var surface = SKSurface.Create(info))
            {
                SKCanvas canvas = surface.Canvas;

                canvas.Clear(SKColors.White);

                // set up drawing tools
                var paint = new SKPaint
                {
                    IsAntialias = true,
                    Color = new SKColor(0x2c, 0x3e, 0x50),
                    StrokeCap = SKStrokeCap.Round
                };

                // create the Xamagon path
                var path = new SKPath();
                path.MoveTo(71.4311121f, 56f);
                path.CubicTo(68.6763107f, 56.0058575f, 65.9796704f, 57.5737917f, 64.5928855f, 59.965729f);
                path.LineTo(43.0238921f, 97.5342563f);
                path.CubicTo(41.6587026f, 99.9325978f, 41.6587026f, 103.067402f, 43.0238921f, 105.465744f);
                path.LineTo(64.5928855f, 143.034271f);
                path.CubicTo(65.9798162f, 145.426228f, 68.6763107f, 146.994582f, 71.4311121f, 147f);
                path.LineTo(114.568946f, 147f);
                path.CubicTo(117.323748f, 146.994143f, 120.020241f, 145.426228f, 121.407172f, 143.034271f);
                path.LineTo(142.976161f, 105.465744f);
                path.CubicTo(144.34135f, 103.067402f, 144.341209f, 99.9325978f, 142.976161f, 97.5342563f);
                path.LineTo(121.407172f, 59.965729f);
                path.CubicTo(120.020241f, 57.5737917f, 117.323748f, 56.0054182f, 114.568946f, 56f);
                path.LineTo(71.4311121f, 56f);
                path.Close();

                // draw the Xamagon path
                canvas.DrawPath(path, paint);
            }
        }
    }
}