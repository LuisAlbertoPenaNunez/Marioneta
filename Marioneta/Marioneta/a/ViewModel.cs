using System;

using Xamarin.Forms;
using System.ComponentModel;
using PropertyChanged;

namespace a
{
	[ImplementPropertyChanged]
	public class ViewModel : INotifyPropertyChanged
	{
		public Color BackgroundColor { get; set; }

		public ViewModel ()
		{
			
		}

		public void NavigatedTo ()
		{
			BackgroundColor = Color.Black;
		}

		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		protected virtual void OnPropertyChanged (PropertyChangedEventArgs e)
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler (this, e);
		}
	}


}

