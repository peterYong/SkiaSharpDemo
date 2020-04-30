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
    public partial class CreateSignStampPage : ContentPage
    {

        public CreateSignStampPage()
        {
            InitializeComponent();
        }

        private void createV1_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateSignsPage(SignType.V1));
        }

        private void createV2_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateSignsPage(SignType.V2));
        }

        private void createV2EN_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateSignsPage(SignType.V2EN));
        }

        private void createV3EN_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateSignsPage(SignType.V3EN));
        }

        private void createV4EN_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateSignsPage(SignType.V4EN));

        }

      
    }
}