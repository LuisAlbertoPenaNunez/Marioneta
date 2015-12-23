using System;
using Xamarin.Forms;

namespace Marioneta
{
	public class Transaction
	{
		public Thickness Padding { get; set; }

		public ExpandViewDirectionX ExpandToX { get; set; }

		public ExpandViewDirectionY ExpandToY { get; set; }

		public ViewDirectionX AlignViewToX { get; set; }

		public ViewDirectionY AlignViewToY { get; set; }

		public View RelativeView { get; set; }

		public Dimension DesiredDimension { get; set; }

		public Transaction ()
		{
			DesiredDimension = new Dimension();
		}
	}
}