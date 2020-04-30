using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharpDemo.Helper;
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
    public partial class CreateENStampPage : ContentPage
    {
        private string _company;
        public string Company
        {
            get { return _company; }
            set { _company = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 印章位图
        /// </summary>
        SKBitmap stampBitmap;

        //位图的宽和高
        int width = 0;
        int height = 0;

        string stampType = "电子公章";

        /// <summary>
        /// 用于在显示时的大小
        /// </summary>
        int skiaBitmapsRadius = 200;

        /// <summary>
        /// 像素与Xamarin.Forms之间的比例。差不多是2.5倍
        /// </summary>
        float scale = 2.5f;

        /// <summary>
        /// 文本画刷
        /// </summary>
        SKPaint skTextPaint;

        /// <summary>
        /// 圆圈画刷
        /// </summary>
        SKPaint paintCircle;

        SKColor color;

        /// <summary>
        /// 创建英文单位图章
        /// </summary>
        public CreateENStampPage()
        {
            InitializeComponent();

            BindingContext = this;

            Title = "生成英文签章";
            Company = "WODEYONG HA";
            Init();
        }

        private void Init()
        {
            width = Convert.ToInt32(skiaBitmapsRadius * scale);
            height = width;

            SKFontManager fontManager = SKFontManager.Default;
            color = Color.FromRgb(68, 114, 196).ToSKColor();
            skTextPaint = new SKPaint() { TextSize = 50, Color = color };
            skTextPaint.IsAntialias = true;
            //兼容中文
            //skTextPaint.Typeface = fontManager.MatchCharacter("时");

            //字体粗细、字体宽度（压缩/扩展）、字体倾斜
            skTextPaint.Typeface = fontManager.MatchCharacter("", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright, new string[] { "zh" }, '时');
            //skTextPaint.Typeface = fontManager.MatchCharacter("sans-serif-black", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright, null, '时');
            paintCircle = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = color,
                StrokeWidth = 10
            };
        }


        /// <summary>
        /// 创建一个位图 并在它上面绘制一个圆圈、文本、圆圈、文本、五角星
        /// </summary>
        private void CreateBitmap()
        {
            stampBitmap = new SKBitmap(width, height);
            using (SKCanvas bitmapCanvas = new SKCanvas(stampBitmap))
            {
                bitmapCanvas.Clear(SKColors.Transparent); //位图的底部颜色
                bitmapCanvas.DrawCircle(width / 2, height / 2, (width - 20) / 2, paintCircle);

                paintCircle.StrokeWidth = 6;
                bitmapCanvas.DrawCircle(width / 2, height / 2, (width - 20) / 2 - 39 - 40, paintCircle);  //画内圈，文字高度大概39

                //画居中线
                //SKPath sKPath = new SKPath();
                //sKPath.MoveTo(new SKPoint(-width / 2, width / 2));
                //sKPath.LineTo(new SKPoint(width / 2, width / 2));
                //SKPaint strokePaint = new SKPaint
                //{
                //    Style = SKPaintStyle.Stroke, //轮廓
                //    Color = SKColors.Red,
                //    StrokeWidth = 5  //设置为小点 顶部就看似重合了
                //};
                //bitmapCanvas.DrawPath(sKPath, strokePaint);

                PaintStampText(bitmapCanvas, Company, width, height, skTextPaint);

                PaintStampType(bitmapCanvas, stampType, width, height, skTextPaint);

                PaintFiveStar(bitmapCanvas, width, height);
            }
            Common.ImageBytes = BitmapHelper.GetBytes(stampBitmap); //将位图转为字节数组（以后 重新生成时用）

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
            float textRadius = (width - 140) / 2;  //文字组成的圆 半径
            paint.TextSize = 50;

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
                byte[] words = Encoding.UTF8.GetBytes(word);

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
                    xStart = -Math.Abs(xStart); //左半部分
                }
                else
                {
                    xStart = Math.Abs(xStart);
                }

                if (Math.Abs(i) < 90)
                {
                    yStart = -Math.Abs(yStart); //正负90度以内
                }
                else
                {
                    yStart = Math.Abs(yStart);
                }

                bitmapCanvas.Translate(xStart, yStart); //移到圆上 某个点 去操作（在这个点上 画文本）
                bitmapCanvas.RotateDegrees(i);
                //bitmapCanvas.DrawText(word, 0, 0, paint);
                bitmapCanvas.DrawText(words, -charWidth / 4, 0, paint);
                bitmapCanvas.RotateDegrees(-i);
                bitmapCanvas.Translate(-xStart, -yStart);  //移到中心点

                index++;
            }
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
            SKRect rect = new SKRect();
            skTextPaint.MeasureText(stampType, ref rect);  //注意不同字体大小 测出来的长度不一样
            float textWidth = rect.Width;
            float textHeight = rect.Height;
            bitmapCanvas.DrawText(texts, -textWidth / 2, textHeight / 2, skTextPaint); //文本是左下角为原点
        }

        /// <summary>
        /// 画五角星
        /// </summary>
        /// <param name="bitmapCanvas"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void PaintFiveStar(SKCanvas bitmapCanvas, int width, int height)
        {
            bitmapCanvas.Translate(-width / 2, -height / 2);  //移到原点

            float top = 90 * height / 100;
            SKPoint center = new SKPoint(width / 2, top); //五角星的中心点
            float radius = 24;  //控制五角星大小

            SKPath path = new SKPath
            {
                FillType = SKPathFillType.Winding
            };
            path.MoveTo(width / 2, top - radius); //五角星的起点

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
                Color = color
            };
            bitmapCanvas.DrawPath(path, fillPaint);
        }


        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            stampType = (String)sealTypePicker.SelectedItem;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            CreateBitmap();
            if (stampBitmap != null)
            {
                canvasView.InvalidateSurface();
            }
        }

        private void skia_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            if (stampBitmap != null)
            {
                canvas.Clear();
                canvas.DrawBitmap(stampBitmap, (info.Width - width) / 2, (info.Height - height) / 2 - 200); //坐标
            }
        }
    }
}