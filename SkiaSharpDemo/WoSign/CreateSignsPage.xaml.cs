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
    public partial class CreateSignsPage : ContentPage
    {
        public CreateSignsPage()
        {
            InitializeComponent();
        }

        #region Field

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        private string _companyl;
        public string Company
        {
            get { return _companyl; }
            set { _companyl = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 印章位图
        /// </summary>
        SKBitmap stampBitmap;

        //位图的宽和高
        int width = 0;
        int height = 0;

        //string text = "Hello123" + Environment.NewLine + "xx技术有限公司";
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
        SKPaint paintText;

        /// <summary>
        /// 横线画刷
        /// </summary>
        SKPaint paintLine;

        SKColor colorBlue = Color.FromRgb(68, 114, 196).ToSKColor();
        SKColor colorBlack = Color.FromRgb(0, 0, 0).ToSKColor();
        SKColor colorRed = Color.Red.ToSKColor();

        /// <summary>
        /// 创建图章的方法
        /// </summary>
        private Action CreateStampAction;

        /// <summary>
        /// 签章类型
        /// </summary>
        private SignType signType;

        SKFontManager fontManager = SKFontManager.Default;

        #endregion

        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="type"></param>
        public CreateSignsPage(SignType type)
        {
            InitializeComponent();


            BindingContext = this;

            Init(type);
        }


        private void Init(SignType type)
        {
            signType = type;

            paintText = new SKPaint() { Color = colorBlue };
            paintText.IsAntialias = true;
            //兼容中文
            //skTextPaint.Typeface = fontManager.MatchCharacter("时");

            //字体粗细、字体宽度（压缩/扩展）、字体倾斜
            paintText.Typeface = fontManager.MatchCharacter("", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright, new string[] { "zh" }, '时');
            //skTextPaint.Typeface = fontManager.MatchCharacter("sans-serif-black", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright, null, '时');
            paintLine = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = colorBlack,
                StrokeWidth = 3
            };

            switch (type)
            {
                case SignType.V1:
                    Name = "胡梁勇";
                    Email = "mes3@mesince.com";
                    Title = "V1签名";
                    CreateStampAction = CreateV1;
                    break;
                case SignType.V2:
                    Name = "胡梁勇";
                    Title = "V2中文章";
                    CreateStampAction = CreateV2;
                    break;
                case SignType.V2EN:
                    Name = "Your Fullname";
                    Email = "yournm@yourdm.com";
                    Title = "V2签名";
                    CreateStampAction = CreateV2EN;
                    break;
                case SignType.V3EN:
                    Name = "Your Fullname";
                    Email = "yournm@yourdm.com";
                    Company = "Your Organization Name";
                    Title = "V3签名";
                    CreateStampAction = CreateV3EN;
                    break;
                case SignType.V4EN:
                    Name = "Your Fullname";
                    Email = "yournm@yourdm.com";
                    Company = "Your Organization Name";
                    Title = "V4签名";
                    CreateStampAction = CreateV4EN;
                    break;
            }
            CreateStampAction.Invoke();
        }

        #region V1


        private void CreateV1()
        {
            width = Convert.ToInt32(300 * scale);
            height = Convert.ToInt32(114 * scale); ;
            stampBitmap = new SKBitmap(width, height);
            using (SKCanvas bitmapCanvas = new SKCanvas(stampBitmap))
            {
                bitmapCanvas.Clear(SKColors.White); //位图的底部颜色

                PaintName(bitmapCanvas, width, height, paintText);
                PaintLine(bitmapCanvas, width, height, paintLine);
                PaintEmail(bitmapCanvas, width, height, paintText);
            }
            Common.ImageBytes = BitmapHelper.GetBytes(stampBitmap); //将位图转为字节数组（以后 重新生成时用）
        }

        private void PaintName(SKCanvas bitmapCanvas, int width, int height, SKPaint paintText)
        {
            if (signType == SignType.V2EN)
            {
                paintText.Typeface = fontManager.MatchCharacter("sans-serif-black", '时');
            }
            paintText.Color = colorBlack;
            paintText.TextSize = 80;
            SKRect rect = new SKRect();
            paintText.MeasureText(Name, ref rect);
            bitmapCanvas.DrawText(Name, 0, (height / 2.0f) / 2.0f + rect.Height / 2.0f, paintText);
        }

        /// <summary>
        /// 画 横线
        /// </summary>
        /// <param name="bitmapCanvas"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="paint"></param>
        private void PaintLine(SKCanvas bitmapCanvas, int width, int height, SKPaint paint)
        {
            bitmapCanvas.DrawLine(0, height / 2.0f, width, height / 2.0f, paint);
        }

        private void PaintEmail(SKCanvas bitmapCanvas, int width, int height, SKPaint paintText)
        {
            paintText.Color = colorBlue;
            paintText.TextSize = 60;
            SKRect rect = new SKRect();
            paintText.MeasureText(Email, ref rect);
            bitmapCanvas.DrawText(Email, 0, 3 * height / 4.0f + rect.Height / 2.0f - rect.Height / 2.0f, paintText); //为了靠近一点横线，减号后面是临时加的
        }
        #endregion


        #region V2

        private void CreateV2()
        {
            width = Convert.ToInt32(200 * scale);
            height = Convert.ToInt32(200 * scale); ;
            stampBitmap = new SKBitmap(width, height);
            using (SKCanvas bitmapCanvas = new SKCanvas(stampBitmap))
            {
                bitmapCanvas.Clear(SKColors.White); //位图的底部颜色

                //画居中线
                SKPath sKPath = new SKPath();
                sKPath.MoveTo(new SKPoint(0, height / 2));
                sKPath.LineTo(new SKPoint(width, height / 2));
                SKPaint strokePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke, //轮廓
                    Color = SKColors.Red,
                    StrokeWidth = 5  //设置为小点 顶部就看似重合了
                };
                bitmapCanvas.DrawPath(sKPath, strokePaint);
                //画居中线
                SKPath sKPath2 = new SKPath();
                sKPath2.MoveTo(new SKPoint(width / 2, 0));
                sKPath2.LineTo(new SKPoint(width / 2, height));
                bitmapCanvas.DrawPath(sKPath2, strokePaint);


                float offset = 30 + 8 + 4;
                //PaintSquare(bitmapCanvas, width, height, paintText, offset);
                //PaintName(bitmapCanvas, Name, width - offset, height - offset, paintText, offset);


                //PaintSquare(bitmapCanvas, width, height, paintText, offset);
                //PaintName(bitmapCanvas, Name, width - 2 * offset, height - 2 * offset, paintText, offset);


                PaintName(bitmapCanvas, Name, width, height , paintText, offset);
                PaintSquare(bitmapCanvas, width, height, paintText, offset);

            }
            Common.ImageBytes = BitmapHelper.GetBytes(stampBitmap); //将位图转为字节数组（以后 重新生成时用）
        }

        /// <summary>
        /// 画正方框
        /// </summary>
        /// <param name="bitmapCanvas"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="paintText"></param>
        private void PaintSquare(SKCanvas bitmapCanvas, int width, int height, SKPaint paintText, float offset)
        {
            SKRect frameRect = new SKRect(0, 0, width, height);
            //float strokeWidth = 30;
            float strokeWidth = offset - 12;
            SKPaint framePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth,
                Color = SKColors.Red
            };
            // Draw one frame。
            bitmapCanvas.DrawRoundRect(frameRect, 0, 0, framePaint);

            // Inflate the frameRect and draw another
            frameRect.Inflate(-(strokeWidth + 8), -(strokeWidth + 8));
            framePaint.StrokeWidth = 4;
            bitmapCanvas.DrawRoundRect(frameRect, 0, 0, framePaint);


        }

        private void PaintName(SKCanvas bitmapCanvas, string name, float width, float height, SKPaint paintText, float offset)
        {
            ////相对中心点来 画文本，
            //bitmapCanvas.Translate(width / 2.0f, height / 2.0f);

            ////宽高用内部正方形
            //width = width - 2 * offset;
            //height = height - 2 * offset;

            //bitmapCanvas.Translate(offset, offset);

            name = name + "印";
            paintText.Color = colorRed;
            paintText.TextSize = 140;
            SKRect rect = new SKRect();
            string word;

            if (name.Length <= 2)
            {
                float x = 0;
                float y = 0;
                for (int i = 0; i < name.Length; i++)
                {
                    word = name[i].ToString();
                    paintText.MeasureText(word, ref rect);

                    x = width / 2.0f - rect.Width / 2.0f;
                    y = height / 4.0f + rect.Height / 2.0f + i * (height / 2.0f);
                    if (name[i] == '印')
                    {
                        paintText.TextSize = 180;
                    }
                    bitmapCanvas.DrawText(word, x, y, paintText);
                }
            }
            else if (name.Length <= 4)
            {
                float x = 0;
                float y = 0;
                for (int i = 0; i < name.Length; i++)
                {
                    word = name[i].ToString();
                    paintText.MeasureText(word, ref rect);

                    y = height / 4.0f + rect.Height / 2.0f + (i % 2) * (height / 2.0f);  //height加0或1个 字高的一半
                    if (i <= 1)
                    {
                        x = width / 4.0f - rect.Width / 2.0f + width / 2.0f;
                    }
                    else
                    {
                        x = width / 4.0f - rect.Width / 2.0f;
                    }

                    if (name[i] == '印')
                    {
                        paintText.TextSize = 180;
                    }
                    bitmapCanvas.DrawText(name[i].ToString(), x, y, paintText);
                }
            }

        }

        private void CreateV2EN()
        {
            width = Convert.ToInt32(325 * scale);
            height = Convert.ToInt32(161 * scale); ;
            stampBitmap = new SKBitmap(width, height);
            using (SKCanvas bitmapCanvas = new SKCanvas(stampBitmap))
            {
                bitmapCanvas.Clear(SKColors.White); //位图的底部颜色

                PaintName(bitmapCanvas, width, height, paintText);
                PaintLine(bitmapCanvas, width, height, paintLine);
                PaintNameAndEmail(bitmapCanvas, width, height, paintText);
            }
            Common.ImageBytes = BitmapHelper.GetBytes(stampBitmap); //将位图转为字节数组（以后 重新生成时用）
        }

        private void PaintNameAndEmail(SKCanvas bitmapCanvas, int width, int height, SKPaint paintText)
        {
            paintText.Color = colorBlue;
            paintText.TextSize = 60;
            SKRect rect = new SKRect();
            paintText.MeasureText(Name, ref rect);
            float y1 = height / 2.0f + rect.Height + 20;
            bitmapCanvas.DrawText(Name, 0, y1, paintText); //为了靠近一点横线，减号后面是临时加的

            rect = new SKRect();
            paintText.MeasureText(Email, ref rect);
            bitmapCanvas.DrawText(Email, 0, y1 + rect.Height + 10, paintText); //为了靠近一点横线，减号后面是临时加的
        }

        #endregion



        #region V3

        private void CreateV3EN()
        {
            width = Convert.ToInt32(397 * scale);
            height = Convert.ToInt32(218 * scale); ;
            stampBitmap = new SKBitmap(width, height);
            using (SKCanvas bitmapCanvas = new SKCanvas(stampBitmap))
            {
                bitmapCanvas.Clear(SKColors.White); //位图的底部颜色

                PaintV3Up(bitmapCanvas, width, height, paintText);
                PaintLine(bitmapCanvas, width, height, paintLine);
                PaintV3Down(bitmapCanvas, width, height, paintText);
            }
            Common.ImageBytes = BitmapHelper.GetBytes(stampBitmap); //将位图转为字节数组（以后 重新生成时用）
        }

        private void PaintV3Up(SKCanvas bitmapCanvas, int width, int height, SKPaint paintText)
        {
            paintText.Color = colorBlack;
            paintText.TextSize = 60;
            SKRect rect = new SKRect();
            string text = "For and on behalf of";
            paintText.MeasureText(text, ref rect);
            float top = rect.Height + 10;
            bitmapCanvas.DrawText(text, 0, top, paintText);

            paintText.Color = colorBlue;
            paintText.TextSize = 60;
            rect = new SKRect();
            paintText.MeasureText(Company, ref rect);
            top = top + rect.Height + 20;
            bitmapCanvas.DrawText(text, 20, top, paintText);

            paintText.Color = colorBlack;
            paintText.TextSize = 80;
            rect = new SKRect();
            paintText.MeasureText(Name, ref rect);
            top = top + rect.Height + 30;
            bitmapCanvas.DrawText(Name, 40, top, paintText);
        }

        private void PaintV3Down(SKCanvas bitmapCanvas, int width, int height, SKPaint paintText)
        {
            paintText.Color = colorBlue;
            paintText.TextSize = 60;
            SKRect rect = new SKRect();
            paintText.MeasureText(Email, ref rect);
            float top = height / 2.0f + rect.Height + 10;
            bitmapCanvas.DrawText(Email, 20, top, paintText);

            top += 35;
            PaintDotLine(bitmapCanvas, width, top);

            paintText.Color = colorBlack;
            rect = new SKRect();
            string text = "Authorized Signature";
            paintText.MeasureText(text, ref rect);
            top = top + rect.Height + 15;
            bitmapCanvas.DrawText(text, width - rect.Width - 20, top, paintText);
        }
        /// <summary>
        /// 画虚线
        /// </summary>
        private void PaintDotLine(SKCanvas bitmapCanvas, float width, float top)
        {
            float[] intervals = new float[] { 20, 20 };
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Gray,
                StrokeWidth = 3,
                StrokeCap = SKStrokeCap.Round,
                PathEffect = SKPathEffect.CreateDash(intervals, 20)
            };
            SKPath path = new SKPath();
            path.MoveTo(0, top);
            path.LineTo(width, top);

            bitmapCanvas.DrawPath(path, paint);
        }

        #endregion

        #region V4
        private void CreateV4EN()
        {
            width = Convert.ToInt32(397 * scale);
            height = Convert.ToInt32(218 * scale); ;
            stampBitmap = new SKBitmap(width, height);
            using (SKCanvas bitmapCanvas = new SKCanvas(stampBitmap))
            {
                bitmapCanvas.Clear(SKColors.White); //位图的底部颜色

                PaintV3Up(bitmapCanvas, width, height, paintText);
                PaintLine(bitmapCanvas, width, height, paintLine);
                PaintV4Down(bitmapCanvas, width, height, paintText);
            }
            Common.ImageBytes = BitmapHelper.GetBytes(stampBitmap); //将位图转为字节数组（以后 重新生成时用）
        }

        private void PaintV4Down(SKCanvas bitmapCanvas, int width, int height, SKPaint paintText)
        {
            paintText.Color = colorBlue;
            paintText.TextSize = 60;
            SKRect rect = new SKRect();
            paintText.MeasureText(Name, ref rect);
            float top = height / 2.0f + rect.Height + 10;
            bitmapCanvas.DrawText(Name, 20, top, paintText);

            rect = new SKRect();
            paintText.MeasureText(Email, ref rect);
            top = top + rect.Height + 10;
            bitmapCanvas.DrawText(Email, 20, top, paintText);

            top += 35;
            PaintDotLine(bitmapCanvas, width, top);

            paintText.Color = colorBlack;
            rect = new SKRect();
            string text = "Authorized Signature";
            paintText.MeasureText(text, ref rect);
            top = top + rect.Height + 15;
            bitmapCanvas.DrawText(text, width - rect.Width - 20, top, paintText);
        }

        #endregion

        private void Button_Clicked(object sender, EventArgs e)
        {
            CreateStampAction.Invoke();
            if (stampBitmap != null)
            {
                canvasView.InvalidateSurface();
            }
        }

        private void skia_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            //if (width == skiaBitmapsRadius)
            //{
            // //比例，像素与Xamarin.Forms之间的比例。差不多是2.5倍
            //    width = Convert.ToInt32((canvasView.CanvasSize.Width / canvasView.Width) * width);
            //    height = Convert.ToInt32((canvasView.CanvasSize.Width / canvasView.Width) * height);
            //}


            if (stampBitmap != null)
            {
                canvas.Clear();
                canvas.DrawBitmap(stampBitmap, (info.Width - width) / 2, (info.Height - height) / 2 - 200); //坐标
            }
        }
    }
}