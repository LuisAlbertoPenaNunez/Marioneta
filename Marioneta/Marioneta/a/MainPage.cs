using System;

using Xamarin.Forms;
using Marioneta;
using System.ComponentModel;

namespace a
{
	public class MainPage : ContentPage
	{
		ViewModel _vm;

		public MainPage ()
		{
			_vm = new ViewModel ();

			Content = CreateView();

			BindingContext = _vm;

			_vm.NavigatedTo();
		}

		View CreateView ()
		{
			var layout = new RelativeBuilder()
				.AddView(new Label{Text = "KLK"})
				.AlignParentCenterXY();
			
			layout.SetBinding<ViewModel>(RelativeBuilder.BackgroundColorProperty, m => m.BackgroundColor);

			return layout.BuildLayout();
		}
	}

}

