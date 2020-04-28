using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharpDemo.Font;
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
    public partial class CreateStampPage : ContentPage
    {
        public CreateStampPage()
        {
            InitializeComponent();

            BindingContext = this;

            Title = "生成签章";
            Company = "沃通电子认证服务";
            // Create bitmap and draw on it
            //CreateBitmap();
        }

        private string _company;
        public string Company
        {
            get { return _company; }
            set { _company = value; OnPropertyChanged(); }
        }

        SKBitmap helloBitmap;
        //string text = "Hello123" + Environment.NewLine + "xx技术有限公司";
        string stampStyle = "电子公章";
        SKFontManager fontManager = SKFontManager.Default;
        SKTypeface commonTypeface;
        int width = 400, height = 400;  //位图的宽和高


        /// <summary>
        /// 创建一个位图 并在它上面绘制一个圆圈、五角星、文本
        /// </summary>
        private void CreateBitmap()
        {
            commonTypeface = fontManager.MatchCharacter('时'); //兼容中文
            SKPaint skTextPaint = new SKPaint() { TextSize = 40, Color = SKColors.Red, Typeface = commonTypeface };
            SKPaint paintCircle = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 10
            };
            helloBitmap = new SKBitmap(width, height); 
            using (SKCanvas bitmapCanvas = new SKCanvas(helloBitmap))
            {
                //bitmapCanvas.Clear(SKColors.White); //位图的底部颜色
                bitmapCanvas.Clear(SKColors.Transparent); //位图的底部颜色
                bitmapCanvas.DrawCircle(width / 2, height / 2, (width - 20) / 2, paintCircle);

                PaintFiveStar(bitmapCanvas, width, height);
                PaintStampType(bitmapCanvas, stampStyle, width, height, skTextPaint);

                PaintStampText(bitmapCanvas, Company, width, height, skTextPaint);
            }
            Common.ImageBytes = GetBytes(helloBitmap); //将位图转为字节数组（以后 重新生成时用）


            // Create SKCanvasView to view result。【在设备的屏幕上 显示位图】
            //SKCanvasView canvasView = new SKCanvasView();
            //canvasView.BackgroundColor = Color.Gray;
            //canvasView.PaintSurface += skia_PaintSurface;
            //Content = canvasView;
        }

        /// <summary>
        /// 画 签章文本（公司名）
        /// </summary>
        /// <param name="bitmapCanvas"></param>
        /// <param name="stampText"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="paint"></param>
        private void PaintStampText(SKCanvas bitmapCanvas, string stampText, int width, int height, SKPaint paint)
        {
            bitmapCanvas.Translate(width / 2f, height / 2f);
            float textRadius = (width - 120) / 2;

            //测量文本的宽和高
            SKRect rect = new SKRect();
            paint.MeasureText(stampText, ref rect);
            float textWidth = rect.Width;
            float textHeight = rect.Height;

            int textLength = stampText.Length;
            //一个字符的宽度
            float charWidth = textWidth / textLength;

            if (textLength == 1)
            {
                bitmapCanvas.DrawText(stampText, -(charWidth / 2), -textRadius, paint);
                return;
            }

            //字符串 在圆弧上对应的总角度
            float totalAngle = (int)(360 * textWidth / (2 * Math.PI * textRadius));
            //totalAngle =  totalAngle; //增加一些空隙的长度
            float angleStep = totalAngle / (textLength - 1);
            int index = 0;
            float xStart = 0;
            float yStart = 0;

            for (float i = -totalAngle / 2; i <= totalAngle / 2 || index < textLength; i += angleStep) //兼容最后一个字，角度float计算 不精确
            //for (index = 0; index < textLength;)
            {
                string word = stampText[index].ToString();

                //这里不用旋转角度的方式，因为旋转后 本文也会跟随旋转，后续角度也调整，比较麻烦。。用下面的定位到角度点方式
                //if (index != 0)
                //{
                //    bitmapCanvas.RotateDegrees(angleStep);//旋转到下一个角度

                //}

                //定位到下一个角度点。。
                xStart = (float)Math.Sin(Math.PI * (totalAngle / 2 - angleStep * index) / 180) * textRadius;
                yStart = (float)Math.Cos(Math.PI * (totalAngle / 2 - angleStep * index) / 180) * textRadius;
                if (i < 0)
                {
                    xStart = -Math.Abs(xStart);
                }
                else
                {
                    xStart = Math.Abs(xStart);
                }

                if (Math.Abs(i) < 90)
                {
                    yStart = -Math.Abs(yStart);
                }
                else
                {
                    yStart = Math.Abs(yStart);
                }

                bitmapCanvas.Translate(xStart, yStart); //移到圆上 某个点 去操作（在这个点上 画文本）
                bitmapCanvas.RotateDegrees(i);
                //bitmapCanvas.DrawText(word, 0, 0, paint);
                bitmapCanvas.DrawText(word, -charWidth / 4, 0, paint);
                bitmapCanvas.RotateDegrees(-i);
                bitmapCanvas.Translate(-xStart, -yStart);  //移到中心点

                index++;
            }
        }

        /// <summary>
        /// 画五角星
        /// </summary>
        /// <param name="bitmapCanvas"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void PaintFiveStar(SKCanvas bitmapCanvas, int width, int height)
        {
            SKPoint center = new SKPoint(width / 2, height / 2);
            float radius = 80;  //控制五角星大小

            SKPath path = new SKPath
            {
                FillType = SKPathFillType.Winding
            };
            path.MoveTo(width / 2, height / 2 - radius);

            for (int i = 1; i < 5; i++)
            {
                // angle from vertical
                //如果五角星的中心是点（0，0），则五角星所有点都在围绕该点的圆上（半径设置为radius）。每个点都是一个正弦和余弦值的组合，其角度增加了360度的2/5倍。
                double angle = i * 4 * Math.PI / 5;
                path.LineTo(center + new SKPoint(radius * (float)Math.Sin(angle),
                                                -radius * (float)Math.Cos(angle)));
            }
            path.Close();

            SKPaint fillPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Red
            };
            bitmapCanvas.DrawPath(path, fillPaint);
        }

        /// <summary>
        /// 画 签章的类型
        /// </summary>
        /// <param name="bitmapCanvas"></param>
        /// <param name="stampType"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="skTextPaint"></param>
        private void PaintStampType(SKCanvas bitmapCanvas, string stampType, int width, int height, SKPaint skTextPaint)
        {
            byte[] texts = Encoding.UTF8.GetBytes(stampType);
            float textWidth = skTextPaint.MeasureText(stampType);
            bitmapCanvas.DrawText(texts, (width - textWidth) / 2, 85 * height / 100, skTextPaint);
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

        private void Button_Clicked(object sender, EventArgs e)
        {
            CreateBitmap();
            if (helloBitmap != null)
            {
                canvasView.InvalidateSurface();
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

            if (helloBitmap != null)
            {
                canvas.Clear();
                canvas.DrawBitmap(helloBitmap, (info.Width - width) / 2, (info.Height - height) / 2); //坐标
            }
        }

    }
}