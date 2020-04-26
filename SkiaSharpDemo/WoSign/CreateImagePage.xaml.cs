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
    public partial class CreateImagePage : ContentPage
    {
        public CreateImagePage()
        {
            InitializeComponent();
            Title = "生成图像";

            // Create bitmap and draw on it
            CreateBitmap();
        }

        SKBitmap helloBitmap;


        /// <summary>
        /// 创建一个位图 并在它上面绘制一个圆圈
        /// </summary>
        private void CreateBitmap()
        {
            using (SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 5
            })
            {
                int width = 400, height = 400;
                helloBitmap = new SKBitmap(width, height); //位图的宽和高
                using (SKCanvas bitmapCanvas = new SKCanvas(helloBitmap))
                {
                    bitmapCanvas.Clear(SKColors.Blue); //位图的底部颜色
                    bitmapCanvas.DrawCircle(width / 2, height / 2, (width-20) / 2, paint);
                }
                Common.ImageBytes = GetBytes(helloBitmap); //将位图转为字节数组（以后 重新生成时用）
            }

            // Create SKCanvasView to view result。【在设备的屏幕上 显示位图】
            SKCanvasView canvasView = new SKCanvasView();
            canvasView.BackgroundColor = Color.Gray;
            canvasView.PaintSurface += skia_PaintSurface;
            Content = canvasView;
        }

        public byte[] GetBytes(SKBitmap saveBitmap)
        {
            using (SKImage image = SKImage.FromBitmap(saveBitmap))
            {
                SKData data = image.Encode();
                byte[] bytes = data.ToArray();
                return bytes;
            }
        }

        private void skia_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            //SKImageInfo结构包含有关绘图表面的信息，最重要的是其宽度和高度（以像素为单位）。 
            //SKSurface对象代表绘图表面本身。 在此程序中，绘图表面是视频显示，但在其他程序中，SKSurface对象也可以表示您使用SkiaSharp进行绘图的 Bitmaps位图。
            //SKSurface的最重要属性是SKCanvas类型的Canvas。此类是用于执行实际绘图的图形绘图上下文，SKCanvas对象封装了图形状态，其中包括图形转换和裁剪。

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
            canvas.DrawBitmap(helloBitmap, info.Width / 2, info.Height / 2); //坐标
        }
    }
}