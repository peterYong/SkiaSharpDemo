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
    public partial class BasicsHomePage : ContentPage
    {
        public BasicsHomePage()
        {
            InitializeComponent();
        }

        private void circle_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SimpleCirclePage());
        }

        private void toggle_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TapToggleFillPage());
        }

        private void framed_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FramedTextPage());
        }

        private void bitmaps_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BasicBitmapsPage());
        }

        private void ellipse_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EllipseFillPage());
        }
    }
}