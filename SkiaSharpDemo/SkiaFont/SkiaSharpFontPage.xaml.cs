using Microsoft.Extensions.PlatformAbstractions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SkiaSharpDemo.SkiaFont
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SkiaSharpFontPage : ContentPage
    {
        SKCanvasView canvasView;
        public SkiaSharpFontPage()
        {
            InitializeComponent();

            Init();
        }


        private void Init()
        {
            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;

        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();


            //TestFont(canvas, info);

            TestFont2(canvas, info);
        }

        private void TestFont(SKCanvas canvas, SKImageInfo info)
        {
            SKPaint paint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 40
            };

            float fontSpacing = paint.FontSpacing;
            float x = 20;               // left margin
            float y = fontSpacing;      // first baseline
            float indent = 100;


            SKFontManager fontManager = SKFontManager.Default;
            List<string> fams = fontManager.FontFamilies.ToList();
            for (int i = 0; i < fams.Count; i++)
            {
                Console.WriteLine(fams[i]);
            }

            paint.Typeface = fontManager.MatchCharacter("sans-serif-black", SKFontStyleWeight.ExtraBold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright, new string[] { "ja" }, '时');
            paint.TextSize = 40;
            canvas.DrawText("加粗 SKCanvasView Height and Width:", x, y, paint);
            y += fontSpacing;
            canvas.DrawText(String.Format("{0:F2} x {1:F2}",
                                          canvasView.Width, canvasView.Height),
                            x + indent, y, paint);

            paint.Typeface = fontManager.MatchCharacter("mitype-bold", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright, new string[] { "zh" }, '时');
            paint.TextSize = 40;
            y += fontSpacing * 2;
            canvas.DrawText("正常 SKCanvasView CanvasSize:", x, y, paint);
            y += fontSpacing;
            canvas.DrawText(canvasView.CanvasSize.ToString(), x + indent, y, paint);
            y += fontSpacing * 2;
            canvas.DrawText("SKImageInfo Size:", x, y, paint);
            y += fontSpacing;
            canvas.DrawText(info.Size.ToString(), x + indent, y, paint);

        }

        private void TestFont2(SKCanvas canvas, SKImageInfo info)
        {
            SKPaint paint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 40
            };

            float fontSpacing = paint.FontSpacing;
            float x = 20;               // left margin
            float y = fontSpacing;      // first baseline
            float indent = 100;


            SKFontManager fontManager = SKFontManager.Default;
            List<string> fams = fontManager.FontFamilies.ToList();
            for (int i = 6 * 0; i < 6 * 1;)
            {
                //string path = $"{PlatformServices.Default.Application.ApplicationBasePath}";

                paint.Typeface = fontManager.MatchFamily("Lobster", SKFontStyle.Bold);   //这样设置无效
                paint.TextSize = 40;
                canvas.DrawText($"{fams[i]} 第一个SKCanvasView Height and Width:", x, y, paint);

                paint.Typeface = fontManager.MatchFamily("华文彩云", SKFontStyle.Bold); //这样设置无效
                y += fontSpacing;
                canvas.DrawText(String.Format("{0:F2} x {1:F2}",
                                              canvasView.Width, canvasView.Height),
                                x + indent, y, paint);


                paint.Typeface = fontManager.MatchFamily(fams[i + 2], SKFontStyle.Bold); //这样设置有效
                y += fontSpacing * 2;
                canvas.DrawText(" SKCanvasView CanvasSize:", x, y, paint);

                paint.Typeface = fontManager.MatchFamily(fams[i + 3], SKFontStyle.Bold);
                y += fontSpacing;
                canvas.DrawText(canvasView.CanvasSize.ToString(), x + indent, y, paint);

                paint.Typeface = fontManager.MatchFamily(fams[i + 4], SKFontStyle.Bold);
                y += fontSpacing * 2;
                canvas.DrawText("SKImageInfo Size:", x, y, paint);

                paint.Typeface = fontManager.MatchFamily(fams[i + 5], SKFontStyle.Bold);
                y += fontSpacing;
                canvas.DrawText(info.Size.ToString(), x + indent, y, paint);

                i = i + 6;
            }
        }

    }
}