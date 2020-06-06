using System;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.CurrentActivity;
using ProductsManagementApp.Interfaces;
using Xamarin.Forms;

namespace ProductsManagementApp.Droid
{
    [Activity(Label = "ProductsManagementApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            //zxing
            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            //camera and gallery 
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            UserDialogs.Init(this);

            CreateNotificationFromIntent(Intent);
        }
        
        //zxing
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            //camera and gallery
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.Extras.GetString(AnroidNotificationManager.TitleKey);
                string message = intent.Extras.GetString(AnroidNotificationManager.MessageKey);

                DependencyService.Get<INotificationService>().ReceiveNotification(title, message);
            }
        }
    }

}