using SkiaSharp;
using SkiaSharp.Views.Forms;
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
    public partial class SimpleCirclePage : ContentPage
    {
        /// <summary>
        /// 绘制一个圆圈，
        /// 创建一个SKCanvasView对象来承载图形，处理PaintSurface事件以及使用SKPaint对象来指定颜色和其他图形属性。
        /// </summary>
        public SimpleCirclePage()
        {
            InitializeComponent();

            Title = "Simple Circle";

            //SKCanvasView类似StackLayout等view。 可以将SKCanvasView与其他Xamarin.Forms View派生词组合
            SKCanvasView canvasView = new SKCanvasView();
            canvasView.WidthRequest = 100;
            canvasView.HeightRequest = 100;
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;
        }

        /// <summary>
        /// PaintSurface事件处理程序是您绘制所有图形的地方。 程序运行时，可以多次调用此方法【例如旋转屏幕】，因此它应保留重新创建图形显示所需的所有信息：
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            //SKPaintSurfaceEventArgs 有两个重要的属性：Info和Surface
           
            //SKImageInfo结构包含有关绘图表面的信息，最重要的是其宽度和高度（以像素为单位）。 
            
            //SKSurface对象代表绘图表面本身。 在此程序中，绘图表面是视频显示，但在其他程序中，SKSurface对象也可以表示您使用SkiaSharp进行绘图的 Bitmaps位图。
            //SKSurface的最重要属性是SKCanvas类型的Canvas。此类是用于执行实际绘图的图形绘图上下文，SKCanvas对象封装了图形状态，其中包括图形转换和裁剪。

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            //Clear方法使用透明颜色清除画布。 重载使您可以为画布指定背景色。
            canvas.Clear();

            //绘制一个半径为100像素的圆。 圆的轮廓为红色，圆的内部为蓝色。
            //因为此特定的图形图像包含两种不同的颜色，所以该工作需要分两个步骤完成。 第一步是绘制圆的轮廓，要指定线条的颜色和其他特征，请创建并初始化SKPaint对象：
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke, //划 线
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 50   //线的粗细，50像素
            };
            //相对于显示表面的左上角指定坐标。 X坐标向右增加，Y坐标向下增加。 在讨论图形时，通常使用数学符号（x，y）表示一个点。 点（0，0）是显示表面的左上角，通常称为原点。
            //DrawCircle的前两个参数指示圆心的X和Y坐标。 将它们分配给显示表面宽度和高度的一半，以将圆心置于显示表面的中心。 第三个参数指定圆的半径，最后一个参数是SKPaint对象。
            canvas.DrawCircle(info.Width / 2, info.Height / 2, 200, paint);

            //要填充圆的内部，可以更改SKPaint对象的两个属性，然后再次调用DrawCircle。
            paint.Style = SKPaintStyle.Fill; //填充
            paint.Color = SKColors.Blue;
            canvas.DrawCircle(info.Width / 2, info.Height / 2, 170, paint);
        }

    }
}