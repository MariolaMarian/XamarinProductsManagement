using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProductsManagementApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoriesView : ContentPage
	{
		public CategoriesView()
		{
			InitializeComponent();
		}
        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}