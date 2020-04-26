using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Reflection;

namespace SkiaSharpDemo.Basics
{
    /// <summary>
    /// 加载位图（从互联网/本地图片/项目中的图片）
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasicBitmapsPage : ContentPage
    {
        SKCanvasView canvasView;

        HttpClient httpClient = new HttpClient();

        /// <summary>
        /// 从Web加载的位图
        /// </summary>
        SKBitmap webBitmap;
        SKBitmap resourceBitmap;
        SKBitmap libraryBitmap;


        public BasicBitmapsPage()
        {
            InitializeComponent();

            Title = "Basic Bitmaps";

            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;


            // 加载项目中的位图资源，图片属性设置为 嵌入的资源
            string resourceID = "SkiaSharpDemo.Media.monkey.png";
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                resourceBitmap = SKBitmap.Decode(stream);
            }

            ////暂不实现
            //// Add tap gesture recognizer
            //TapGestureRecognizer tapRecognizer = new TapGestureRecognizer();
            //tapRecognizer.Tapped += async (sender, args) =>
            //{
            //    // Load bitmap from photo library
            //    IPhotoLibrary photoLibrary = DependencyService.Get<IPhotoLibrary>();

            //    using (Stream stream = await photoLibrary.PickPhotoAsync())
            //    {
            //        if (stream != null)
            //        {
            //            libraryBitmap = SKBitmap.Decode(stream);
            //            canvasView.InvalidateSurface();
            //        }
            //    }
            //};
            //canvasView.GestureRecognizers.Add(tapRecognizer);
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (webBitmap != null)
            {
                float x = (info.Width - webBitmap.Width) / 2;
                float y = (info.Height / 3 - webBitmap.Height) / 2;
                canvas.DrawBitmap(webBitmap, x, y);
            }
            //SKBitmap saveBitmap = SKBitmap.Decode(Common.ImageBytes);
            //if (saveBitmap != null)
            //{
            //    float x = (info.Width - saveBitmap.Width) / 2;
            //    float y = (info.Height / 3 - saveBitmap.Height) / 2;
            //    canvas.DrawBitmap(saveBitmap, x, y);
            //}

            if (resourceBitmap != null)
            {
                canvas.DrawBitmap(resourceBitmap,
                    new SKRect(0, info.Height / 3, info.Width, 2 * info.Height / 3));
            }

            if (libraryBitmap != null)
            {
                float scale = Math.Min((float)info.Width / libraryBitmap.Width,
                                       info.Height / 3f / libraryBitmap.Height);

                float left = (info.Width - scale * libraryBitmap.Width) / 2;
                float top = (info.Height / 3 - scale * libraryBitmap.Height) / 2;
                float right = left + scale * libraryBitmap.Width;
                float bottom = top + scale * libraryBitmap.Height;
                SKRect rect = new SKRect(left, top, right, bottom);
                rect.Offset(0, 2 * info.Height / 3);

                canvas.DrawBitmap(libraryBitmap, rect);
            }
            else
            {
                using (SKPaint paint = new SKPaint())
                {
                    paint.Color = SKColors.Blue;
                    paint.TextAlign = SKTextAlign.Center;
                    paint.TextSize = 48;

                    canvas.DrawText("Tap to load bitmap",
                        info.Width / 2, 5 * info.Height / 6, paint);
                }
            }
        }


        /// <summary>
        /// 由于将await运算符与HttpClient一起使用最方便，因此无法在BasicBitmapsPage构造函数中执行代码。 
        /// 因此放在 OnAppearing 中执行。 此处的URL指向Xamarin网站上带有一些示例位图的区域，网站上的一个程序包允许附加一个规范以将位图调整为特定的宽度：
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 从Web 加载位图
            string url = "https://ss2.bdstatic.com/70cFvnSh_Q1YnxGkpoWK1HF6hhy/it/u=2197602503,3264180697&fm=26&gp=0.jpg";

            try
            {

                using (var stream = await httpClient.GetStreamAsync(url))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    //当在SKBitmap.Decode方法中使用从GetStreamAsync返回的Stream时，Android操作系统引发异常，因为它在主线程上执行冗长的操作。 
                    //因此，使用CopyToAsync将位图文件的内容复制到MemoryStream对象。

                    //静态SKBitmap.Decode方法负责解码位图文件。 它可以处理JPEG，PNG和GIF位图格式，并将结果存储在内部SkiaSharp格式中。 
                    //此时，需要使SKCanvasView InvalidateSurface，以允许PaintSurface处理程序更新显示。
                    webBitmap = SKBitmap.Decode(memoryStream);
                    canvasView.InvalidateSurface();

                }

            }
            catch (Exception)
            {

            }
        }
    }
}