using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace ProductsManagementApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductView : ContentPage
	{
        public ProductView ()
		{
            InitializeComponent();
        }
        
    }
}