using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Marioneta
{
	/// <summary>
	/// MRelativeLayout is a layout that uses a fluent sintax to create layout way faster than ever before!
	/// No more constraint Yay!
	/// </summary>
	public class MRelativeLayout : IMLayout, IDisposable
	{
        /// <summary>
        /// The layout
        /// </summary>
		RelativeLayout _relativeLayout;

        /// <summary>
        /// List of transactions that will be used to tell the class how to render the layout. Check the documentation for more details.
        /// </summary>
        IDictionary<View, Transaction> _transactions;
        
        /// <summary>
        /// We will use this view to have a reference to the X center in the layout
        /// </summary>
        View XCenter;

        /// <summary>
        /// We will use this view to have a reference to the Y center in the layout
        /// </summary>
        View YCenter;

        /// <summary>
        /// We will use this view to have a reference to the XY center in the layout
        /// </summary>
        View XYCenter;

        /// <summary>
        /// The last view that was added to the collection
        /// </summary>
        View _lastViewAddedToCollection;

		public MRelativeLayout ()
		{
			Init();
		}
    
		void Init ()
		{
			XCenter = new Label(){ BackgroundColor = Color.Red };

            YCenter = new Label();

            XYCenter = new Label();

            _transactions = new Dictionary<View, Transaction>();

            _relativeLayout = new RelativeLayout
            {
                Children =
                {
                    {
                        XCenter,
                        Constraint.RelativeToParent(p => (p.Width / 2) - (XCenter.Width / 2)),
                        Constraint.RelativeToParent(p=> 0),
                        Constraint.RelativeToParent(p=> 10),
                        Constraint.RelativeToParent(p=> 10)
                    },
                    {
                        YCenter,
                        Constraint.RelativeToParent(p => 0),
                        Constraint.RelativeToParent(p=> (p.Height / 2) - (XCenter.Height / 2)),
                        Constraint.RelativeToParent(p=> 10),
                        Constraint.RelativeToParent(p=> 10)
                    },
                    {
                        XYCenter,
                        Constraint.RelativeToParent(p => (p.Width / 2) - (XCenter.Width / 2)),
                        Constraint.RelativeToParent(p=> (p.Height / 2) - (XCenter.Height / 2)),
                        Constraint.RelativeToParent(p=> 10),
                        Constraint.RelativeToParent(p=> 10)
                    }
                }
            };

			_relativeLayout.PropertyChanged += OnRelativeLayoutPropertyChanged;
		}

		void OnRelativeLayoutPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if(e.PropertyName.Equals ("Width") || e.PropertyName.Equals("Height"))
			{
				_relativeLayout.ForceLayout ();
			}
		}

		public MRelativeLayout AddNewView(View viewToBePlaced)
        {
            if (viewToBePlaced == null)
                throw new Exception("MRelativeLayout.AddNewView() -- The argument was null");

            var isDuplicated = _transactions.ContainsKey(viewToBePlaced);

            if (isDuplicated)
            {
                throw new Exception("MRelativeLayout.AddNewView() -- You are trying to add an already existing view to the layout");
            }

            var newTransaction = new Transaction();

            _transactions.Add(viewToBePlaced, newTransaction);

            ChangeContext(viewToBePlaced);

            return this;
        }

        void ChangeContext(View newView)
        {
            if (_lastViewAddedToCollection != null)
            {
                _lastViewAddedToCollection = null;
            }
            else
            {
                _lastViewAddedToCollection = newView;
            }
        }

        MRelativeLayout ExpandViewHorizontallyBetween(View left, View right)
        {
            if(left == null || right == null)
                throw new Exception("MRelativeLayout.ExpandViewHorizontallyBetween() -- One of the argument passed was null");

            throw new NotImplementedException("This feature is not available yet.");
        }

        MRelativeLayout ExpandViewVerticallyBetween(View topView, View bottomView)
        {
            if (topView == null || bottomView == null)
                throw new Exception("MRelativeLayout.ExpandViewVerticallyBetween() -- One of the argument passed was null");

            throw new NotImplementedException("This feature is not available yet.");
        }

        public MRelativeLayout ExpandViewHorizontally()
        {
			ExpandView(ExpandViewDirection.ExpandViewHorizontally);

			return this;
        }
        
        public MRelativeLayout ExpandViewVertically()
        {
			ExpandView(ExpandViewDirection.ExpandViewVertically);

			return this;
        }
        
        public MRelativeLayout AlignViewToParentLeft()
        {
			AlignViewX(AlignViewDirectionX.ParentLeft);

			return this;
        }

		public MRelativeLayout AlignViewToParentTop()
        {
			AlignViewY(AlignViewDirectionY.ParentTop);

			return this;
        }

		public MRelativeLayout AlignViewToParentRight()
        {
			AlignViewX(AlignViewDirectionX.ParentRight);

			return this;
        }

		public MRelativeLayout AlignViewToParentBottom()
        {
			AlignViewY(AlignViewDirectionY.ParentBottom);

			return this;
        }

		public MRelativeLayout AlignViewParentCenterVertical()
        {
			AlignViewY(AlignViewDirectionY.ParentCenterY);

            return this;
        }

		public MRelativeLayout AlignViewParentCenterHorizontal()
        {
			AlignViewX(AlignViewDirectionX.ParentCenterX);

            return this;
        }

		public MRelativeLayout AlignViewParentCenterXY()
        {
			AlignViewX(AlignViewDirectionX.ParentCenterX);

			AlignViewY(AlignViewDirectionY.ParentCenterY);

            return this;
        }

		public MRelativeLayout ExpandView(ExpandViewDirection expandViewDirection)
		{
			var transaction = GetTransactionFor(_lastViewAddedToCollection);

			transaction.ExpandTo = expandViewDirection;

			return this;
		}

		public MRelativeLayout AlignViewX(AlignViewDirectionX alignViewDirection)
		{
			var transaction = GetTransactionFor(_lastViewAddedToCollection);

			transaction.AlignViewToX = alignViewDirection;

			return this;
		}

		public MRelativeLayout AlignViewY(AlignViewDirectionY alignViewDirection)
		{
			var transaction = GetTransactionFor(_lastViewAddedToCollection);

			transaction.AlignViewToY = alignViewDirection;

			return this;
		}

		MRelativeLayout AlignViewBelowOf(View sibling)
        {
            if (sibling == null)
                throw new Exception("MRelativeLayout.AlignViewBelowOf() -- The argument was null");

            var transaction = GetTransactionFor(_lastViewAddedToCollection);

            

            return this;
        }

		MRelativeLayout AlignViewAboveOf(View sibling)
        {
            if (sibling == null)
                throw new Exception("MRelativeLayout.AlignViewAboveOf() -- The argument was null");

            var transaction = GetTransactionFor(_lastViewAddedToCollection);

            

            return this;
        }

		MRelativeLayout AlignViewToLeftOf(View sibling)
        {
            if (sibling == null)
                throw new Exception("MRelativeLayout.AlignViewToLeftOf() -- The argument was null");

            var transaction = GetTransactionFor(_lastViewAddedToCollection);

            

            return this;
        }

		MRelativeLayout AlignViewToRightOf(View sibling)
        {
            if (sibling == null)
                throw new Exception("MRelativeLayout.AlignViewToRightOf() -- The argument was null");

            var transaction = GetTransactionFor(_lastViewAddedToCollection);

            

            return this;
        }

		public MRelativeLayout WithPadding(Thickness thickness)
        {
            if (thickness == null)
                throw new Exception("MRelativeLayout.WithPadding() -- The argument was null");

            var transaction = GetTransactionFor(_lastViewAddedToCollection);

			transaction.Padding = thickness;
            
            return this;
        }

		public View ApplyTemplate(Func<IRelativeTemplate> template)
		{
            if (template == null)
				throw new Exception("MRelativeLayout.ApplyTemplate() -- The argument was null");

            throw new NotImplementedException();
		}
        
        private Transaction GetTransactionFor(View view)
        {
            var transaction = _transactions.First(x => x.Key == view);

            //If KeyValuePair<View,Transaction> was not found

            if (transaction.Equals(default(KeyValuePair<View, Transaction>)))
                throw new Exception("MRelativeLayout.GetTransactionFor() -- Are you sure you added a view before calling this method?");

            var transactionValue = transaction.Value;

            //If the KeyValuePair<View,Transaction> was found but the transaction is null at the moment

            if (transactionValue == null)
                throw new Exception("MRelativeLayout.GetTransactionFor() -- A bug has ocurred please report the output log to developers");

            return transactionValue;
        }

        public View BuildLayout()
        {
            foreach (var transaction in _transactions)
            {

                var view = transaction.Key;

                var transactionValues = transaction.Value;
                
				var xPosition = GetXPositionFor(transactionValues.AlignViewToX, view, transactionValues.Padding.Left, transactionValues.Padding.Right);

				var yPosition = GetYPositionFor(transactionValues.AlignViewToY, view, transactionValues.Padding.Top, transactionValues.Padding.Bottom);

                var width = GetWidthFor(transactionValues.ExpandTo);

                var height = GetHeightFor(transactionValues.ExpandTo);

                AddChildrenToLayout(view,
                    xPosition,
					yPosition,
                    width,
                    height);
            }

            return _relativeLayout;
        }

        void AddChildrenToLayout(View view,
            Constraint xConstraint,
            Constraint yConstraint,
            Constraint widthConstraint,
            Constraint heightConstraint)
        {
            _relativeLayout.Children.Add
                (view,
                 xConstraint,
                 yConstraint,
                 widthConstraint,
                 heightConstraint);
        }

		private Constraint GetXPositionFor(AlignViewDirectionX alignViewTo, View view, double paddingLeft, double paddingRight)
        {
            switch (alignViewTo)
            {
                case AlignViewDirectionX.ParentCenterX:
                    {
						return Constraint.RelativeToView(XCenter , (p,s) => (p.Width / 2) - (view.Width / 2));
                    }
                case AlignViewDirectionX.ParentLeft:
                    {
                        return Constraint.RelativeToParent(p => paddingLeft);
                    }
                case AlignViewDirectionX.ParentRight:
                    {
                        return Constraint.RelativeToParent(p => p.Width - paddingRight);
                    }
                default:
                    {
                        return Constraint.RelativeToParent(p => 0);
                    }
            }
        }

        private Constraint GetYPositionFor(AlignViewDirectionY alignViewTo, View view, double paddingTop, double paddingBottom)
        {
            switch (alignViewTo)
            {
			case AlignViewDirectionY.ParentCenterY:
                    {
                        return Constraint.RelativeToView(XCenter, (p, s) => (s.X) - (view.Width / 2));
                    }
			case AlignViewDirectionY.ParentTop:
                    {
                        return Constraint.RelativeToParent(p => paddingTop);
                    }
			case AlignViewDirectionY.ParentBottom:
                    {
                        return Constraint.RelativeToParent(p => p.Height - paddingTop);
                    }
                default:
                    {
                        return Constraint.RelativeToParent(p => 0);
                    }
            }
        }

		private Constraint GetWidthFor(ExpandViewDirection expandTo)
        {
            switch (expandTo)
            {
                case ExpandViewDirection.ExpandViewHorizontally:
                    {
                        return Constraint.RelativeToParent(p => p.Width);
                    }

                case ExpandViewDirection.ExpandViewHorizontallyBetween:
                    {
                        throw new NotImplementedException();
                    }
                default:
                    {
                        return Constraint.RelativeToParent(p => 0);
                    }
            }
        }

		private Constraint GetHeightFor(ExpandViewDirection expandTo)
        {
            switch (expandTo)
            {
                case ExpandViewDirection.ExpandViewVertically:
                    {
                        return Constraint.RelativeToParent(p => p.Height);
                    }

                case ExpandViewDirection.ExpandViewVerticallyBetween:
                    {
                        throw new NotImplementedException();
                    }
                default:
                    {
						return Constraint.RelativeToParent(p => p.Height);
                    }
            }
        }
 
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _transactions = null;

                    XCenter = null;

                    YCenter = null;

                    XYCenter = null;

                    _lastViewAddedToCollection = null;

					_relativeLayout.PropertyChanged -= OnRelativeLayoutPropertyChanged;
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

    }
}