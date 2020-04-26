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
    public partial class FramedTextPage : ContentPage
    {
        /// <summary>
        /// 在页面上居中放置一个短文本字符串，并用由一对圆角矩形组成的框架包围它
        /// </summary>
        public FramedTextPage()
        {
            InitializeComponent();

            Title = "Framed Text";

            SKCanvasView canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            string str = "Hello SkiaSharp!";

            // Create an SKPaint object to display the text
            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.Chocolate
            };

            //第一个MeasureText调用具有一个简单的字符串参数，并根据当前字体属性返回文本的像素宽度。 
            //然后，程序将根据渲染的宽度，当前的TextSize属性和显示区域的宽度，计算SKPaint对象的新TextSize属性。 此计算旨在设置TextSize，以使文本字符串以屏幕宽度的90％呈现：
            float textWidth = textPaint.MeasureText(str); //测量
            textPaint.TextSize = 0.9f * info.Width * textPaint.TextSize / textWidth;

            //如果只需要在屏幕上居中放置一些文本，则可以在不测量文本的情况下大致完成。 而是将SKPaint的TextAlign属性设置为枚举成员SKTextAlign.Center。 
            //然后，您在DrawText方法中指定的X坐标指示文本水平中心的位置。 如果将屏幕的中点传递给DrawText方法，则文本将水平居中且几乎垂直居中，因为基线将垂直居中。


            //第二个MeasureText调用具有SKRect参数，因此它同时获得了呈现文本的宽度和高度。 此SKRect值的Height属性取决于文本字符串中是否存在大写字母，升序和降序。
            //例如，对于文本字符串“ mom”，“ cat”和“ dog”，报告了不同的Height值。

            //如果通过DrawText调用以X和Y位置为0显示文本，则SKRect结构的Left和Top属性指示所渲染文本的左上角的坐标。例如，当此程序在iPhone上运行时 在7个模拟器中，作为对MeasureText的第一次调用之后的计算结果，TextSize被分配了值90.6254。 从第二次调用MeasureText获得的SKRect值具有以下属性值：
            SKRect textBounds = new SKRect(); //存储一组四个浮点数，这些浮点数代表矩形的左上角和右下角。
            textPaint.MeasureText(str, ref textBounds);

            // Calculate offsets to center the text on the screen[以使文本居中显示在屏幕上]
            float xText = info.Width / 2 - textBounds.MidX;  //MidX和MidY值指示矩形中心的坐标。
            float yText = info.Height / 2 - textBounds.MidY;
            //或者
            //float xText = info.Rect.MidX - textBounds.MidX;
            //float yText = info.Rect.MidY - textBounds.MidY;

            // And draw the text..
            canvas.DrawText(str, xText, yText, textPaint);


            //对DrawRoundRect的两个调用，这两个调用都需要SKRect的参数。 此SKRect值基于从MeasureText方法获得的SKRect值，但不能相同。 
            //首先，它需要更大一些，以使圆角矩形不会在文本的边缘上绘制。 
            //其次，它需要在空间上移动，以使“左”和“上”值与要放置矩形的左上角相对应。 这两个作业是通过SKRect定义的Offset和Inflate方法完成的：

            // Create a new SKRect object for the frame around the text
            SKRect frameRect = textBounds;
            frameRect.Offset(xText, yText);
            frameRect.Inflate(10, 10);  //扩大

            //为边框创建另一个SKPaint对象，并两次调用DrawRoundRect。 
            //第二个调用使用一个矩形，该矩形再膨胀10个像素。 第一次调用将角半径指定为20个像素。 第二个的角半径为30像素，因此它们似乎是平行的：
            SKPaint framePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5,
                Color = SKColors.Blue
            };
            // Draw one frame。
            canvas.DrawRoundRect(frameRect, 20, 20, framePaint);

            // Inflate the frameRect and draw another
            frameRect.Inflate(10, 10);
            framePaint.Color = SKColors.DarkBlue;
            canvas.DrawRoundRect(frameRect, 30, 30, framePaint);
        }
    }
}