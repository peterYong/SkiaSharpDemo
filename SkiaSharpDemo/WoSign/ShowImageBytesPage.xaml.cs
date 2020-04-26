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
    public partial class ShowImageBytesPage : ContentPage
    {
        public ShowImageBytesPage()
        {
            InitializeComponent();

            BindingContext = this;
            Title = "展示图片";
            Init();
        }

        private void Init()
        {
            SKBitmap saveBitmap = SKBitmap.Decode(Common.ImageBytes);
            SKImageImageSource sKImageImageSource = new SKImageImageSource();
            sKImageImageSource.Image = SKImage.FromBitmap(saveBitmap);
            imageStamp.Source = sKImageImageSource;
        }
    }
}