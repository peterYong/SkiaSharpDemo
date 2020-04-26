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
    public partial class TapToggleFillPage : ContentPage
    {
        /// <summary>
        /// 在Xamarin.Forms中创建交互式SkiaSharp图形的另一种方法是通过触摸，通过轻按切换两种方式绘制一个简单的圆圈（无填充和有填充）。
        /// TapToggleFillPage类显示如何响应用户输入来更改SkiaSharp图形。
        /// </summary>
        public TapToggleFillPage()
        {
            InitializeComponent();
        }

        private void SKCanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            //SKPaintSurfaceEventArgs 有两个重要的属性：Info和Surface

            //SKImageInfo结构包含有关绘图表面的信息，最重要的是其宽度和高度（以像素为单位）。 

            //SKSurface对象代表绘图表面本身。 在此程序中，绘图表面是视频显示，但在其他程序中，SKSurface对象也可以表示您使用SkiaSharp进行绘图的 Bitmaps位图。
            //SKSurface的最重要属性是SKCanvas类型的Canvas。此类是用于执行实际绘图的图形绘图上下文，SKCanvas对象封装了图形状态，其中包括图形转换和裁剪。

            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
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
            if(showFill)
            {
                paint.Style = SKPaintStyle.Fill; //填充
                paint.Color = SKColors.Blue;
                canvas.DrawCircle(info.Width / 2, info.Height / 2, 170, paint);
            }
        }

        bool showFill = true;

        /// <summary>
        /// 单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            showFill = !showFill;
            //通知画布需要重新绘制自己
            //对InvalidateSurface的调用有效地生成了对PaintSurface处理程序的调用
            (sender as SKCanvasView).InvalidateSurface();
        }
    }
}