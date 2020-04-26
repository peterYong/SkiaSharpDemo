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
    public partial class WoSignHomePage : ContentPage
    {
        public WoSignHomePage()
        {
            InitializeComponent();
        }

        private void create_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateImagePage());
        }

        private void show_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ShowImageBytesPage());
        }

        private void createStamp_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateStampPage());
        }
    }
}