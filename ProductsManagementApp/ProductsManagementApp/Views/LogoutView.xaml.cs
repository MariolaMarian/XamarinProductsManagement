﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProductsManagementApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LogoutView : ContentPage
	{
		public LogoutView ()
		{
			InitializeComponent ();
		}

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}