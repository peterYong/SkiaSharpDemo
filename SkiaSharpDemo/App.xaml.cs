using SkiaSharpDemo.Font;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SkiaSharpDemo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            TypefaceCustomize.Init();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
