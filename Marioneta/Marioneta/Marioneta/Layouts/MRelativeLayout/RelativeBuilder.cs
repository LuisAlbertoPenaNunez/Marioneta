using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.ComponentModel;

namespace Marioneta
{
	/// <summary>
	/// RelativeBuilder is a layout builder that uses a fluent-sintax to create layout way faster than ever before!
	/// No more constraint Yay!
	/// </summary>
	public class RelativeBuilder : BindableObject, IBuilder, IDisposable
	{
		public static BindableProperty BackgroundColorProperty = 
			BindableProperty.Create<RelativeBuilder, Color>(ctrl => ctrl.BackgroundColor,
			defaultValue: Color.White,
			defaultBindingMode: BindingMode.OneWay,
			propertyChanging: (bindable, oldValue, newValue) => 
			{
				var ctrl = (RelativeBuilder) bindable;
				
				ctrl.BackgroundColor = newValue;
		});
		
		public Color BackgroundColor 
		{
			get 
			{ 
				return (Color)GetValue(BackgroundColorProperty); 
			}
			set 
			{ 
				SetValue (BackgroundColorProperty, value);

				_relativeLayout.BackgroundColor = value;
			}
		}

        /// <summary>
        /// The layout
        /// </summary>
		RelativeLayout _relativeLayout;

        /// <summary>
        /// List of transactions that will be used to tell the class how to render the layout. Check the documentation for more details.
        /// </summary>
		ITransactionManager _transactionManager;

        /// <summary>
        /// We will use this view to have a reference to the Y center in the layout
        /// </summary>
		public View Left { get; private set; }

		/// <summary>
		/// We will use this view to have a reference to the top
		/// </summary>
		public View Top { get; private set; }

		/// <summary>
		/// The Y bottom.
		/// </summary>
		public View Bottom { get; private set; }

        /// <summary>
        /// We will use this view to have a reference to the XY center in the layout
        /// </summary>
		public View Center { get; private set; }

		/// <summary>
		/// We will use this view to have a reference to the right
		/// </summary>
		public View Right { get; private set; }

        /// <summary>
        /// The last view that was added to the collection
        /// </summary>
        View _lastViewAddedToCollection;

		public RelativeBuilder (bool iOSPadding = false) : this(new RelativeBuilderTransactionManager(), iOSPadding) 
		{
		}

		RelativeBuilder (ITransactionManager transactionManager, bool iOSPadding)
		{
			_transactionManager = transactionManager;

			Init(iOSPadding);
		}
    	
		void Init (bool iOSPadding)
		{
			Left = new Label();

			Top = new Label();

			Bottom = new Label();

            Center = new Label();

			Right = new Label();

            _relativeLayout = new RelativeLayout
            {
				BackgroundColor = BackgroundColor,
                Children =
                {
                    {
                        Left,
                        Constraint.RelativeToParent(p => 0),
						Constraint.RelativeToParent(p=> (p.Height / 2) - (Left.Height / 2)),
                        Constraint.RelativeToParent(p=> 10),
                        Constraint.RelativeToParent(p=> 10)
                    },
					{
						Top,
						Constraint.RelativeToParent(p => (p.Width / 2) - (Top.Width / 2)),
						Constraint.RelativeToParent(p => 0),
						Constraint.RelativeToParent(p=> 10),
						Constraint.RelativeToParent(p=> 10)
					},
					{
						Bottom,
						Constraint.RelativeToParent(p => (p.Width / 2) - (Bottom.Width / 2)),
						Constraint.RelativeToParent(p => p.Height - 10),
						Constraint.RelativeToParent(p => 10),
						Constraint.RelativeToParent(p => 10)
					},
                    {
                        Center,
                        Constraint.RelativeToParent(p => (p.Width / 2) - (Center.Width / 2)),
						Constraint.RelativeToParent(p=> (p.Height / 2) - (Center.Height / 2)),
                        Constraint.RelativeToParent(p=> 10),
                        Constraint.RelativeToParent(p=> 10)
                    },
					{
						Right,
						Constraint.RelativeToParent(p => p.Width - 10),
						Constraint.RelativeToParent(p=> (p.Height / 2) - (Right.Height / 2)),
						Constraint.RelativeToParent(p=> 10),
						Constraint.RelativeToParent(p=> 10)
					}
                }
            };
			
			_relativeLayout.PropertyChanged += OnRelativeLayoutPropertyChanged;
		}

		/// <summary>
		/// We need this method to recalculate the layout so we can center views
		/// </summary>
		void OnRelativeLayoutPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if(e.PropertyName.Equals ("Width") || e.PropertyName.Equals("Height"))
			{
				_relativeLayout.ForceLayout ();
			}
		}

		/// <summary>
		/// Add a new view to the layout
		/// </summary>
		/// <returns><see cref="Marioneta.MRelativeLayout"/></returns>
		/// <param name="viewToBePlaced">The view to be placed</param>
		public RelativeBuilder AddView(View viewToBePlaced)
        {
            if (viewToBePlaced == null)
				throw new ArgumentNullException("viewToBePlaced");

            var isDuplicated = _transactionManager.ContainsTransaction(viewToBePlaced);

            if (isDuplicated)
            {
				throw new DuplicatedViewException("MRelativeLayout.AddView() -- This view already exists");
            }

            var newTransaction = new Transaction();

			_transactionManager.AddTransaction(viewToBePlaced, newTransaction);

            ChangeContext(viewToBePlaced);

            return this;
        }

		/// <summary>
		/// Set the last view added to collection to operate on
		/// </summary>
		/// <param name="newView">New view.</param>
        void ChangeContext(View newView)
        {
			_lastViewAddedToCollection = newView;
        }

		/// <summary>
		/// Expand between two views for relative positioning
		/// </summary>
		/// <returns><see cref="Marioneta.MRelativeLayout"/>.</returns>
		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
        public RelativeBuilder ExpandViewHorizontallyBetween(View left, View right)
        {
			if (left == null)
				throw new ArgumentNullException("left");

			if (right == null)
				throw new ArgumentNullException("right");

			AlignLeft(left);

//			AlignRight(right);

			return this;
        }

		/// <summary>
		/// Let a view expand between two views vertically
		/// </summary>
        public RelativeBuilder ExpandViewVerticallyBetween(View top, View bottom)
        {
			if (top == null)
				throw new ArgumentNullException("top");

			if (bottom == null)
				throw new ArgumentNullException("bottom");

			AlignTop(top);

//			AlignBottom(bottom);

			return this;
        }

		/// <summary>
		/// Align the view to the parent left
		/// </summary>
		/// <returns><see cref="Marioneta.MRelativeLayout"/></returns>
        public RelativeBuilder AlignParentLeft()
        {
			return AlignViewX(ViewDirectionX.ParentLeft);
        }

		/// <summary>
		/// Align the view to the parent top
		/// </summary>
		/// <returns><see cref="Marioneta.MRelativeLayout"/></returns>
		public RelativeBuilder AlignParentTop()
        {
			return AlignViewY(ViewDirectionY.ParentTop);
        }

		/// <summary>
		/// Align the view to the parent right
		/// </summary>
		/// <returns><see cref="Marioneta.MRelativeLayout"/></returns>
		public RelativeBuilder AlignParentRight()
        {
			return AlignViewX(ViewDirectionX.ParentRight);
        }

		/// <summary>
		/// Align the view to the parent bottom
		/// </summary>
		/// <returns><see cref="Marioneta.MRelativeLayout"/></returns>
		public RelativeBuilder AlignParentBottom()
        {
			return AlignViewY(ViewDirectionY.ParentBottom);
        }

		/// <summary>
		/// Align the view to the parent center vertically
		/// </summary>
		/// <returns><see cref="Marioneta.MRelativeLayout"/></returns>
		public RelativeBuilder AlignParentCenterVertical()
        {
			return AlignViewY(ViewDirectionY.ParentCenterY);
        }

		/// <summary>
		/// Align the view to the parent center horizontally
		/// </summary>
		/// <returns><see cref="Marioneta.MRelativeLayout"/></returns>
		public RelativeBuilder AlignParentCenterHorizontal()
        {
			return AlignViewX(ViewDirectionX.ParentCenterX);
        }

		/// <summary>
		/// Align the view to the XY coordinate of the screen
		/// </summary>
		/// <returns><see cref="Marioneta.MRelativeLayout"/></returns>
		public RelativeBuilder AlignParentCenterXY()
        {
			AlignViewX(ViewDirectionX.ParentCenterX);

			AlignViewY(ViewDirectionY.ParentCenterY);

            return this;
        }

		/// <summary>
		/// Aligns the view to the left of the sibling
		/// </summary>
		public RelativeBuilder AlignLeft (View sibling)
		{
			return AlignViewRelativeToX(sibling, ViewDirectionX.AlignLeft);
		}

		/// <summary>
		/// Aligns the view to the top of the sibling
		/// </summary>
		public RelativeBuilder AlignTop (View sibling)
		{
			return AlignViewRelativeToY(sibling, ViewDirectionY.AlignAbove);
		}

		/// <summary>
		/// Aligns the view to the right of the sibling
		/// </summary>
		public RelativeBuilder AlignRight (View sibling)
		{
			return AlignViewRelativeToX(sibling, ViewDirectionX.AlignRight);
		}

		/// <summary>
		/// Aligns the view to the bottom of the sibling
		/// </summary>
		public RelativeBuilder AlignBottom (View sibling)
		{
			return AlignViewRelativeToY(sibling, ViewDirectionY.AlignBottom);
		}

		/// <summary>
		/// Put the view below of the sibling
		/// </summary>
		public RelativeBuilder BelowOf(View sibling)
        {
			return AlignViewRelativeToY(sibling, ViewDirectionY.BelowOf);
        }

		/// <summary>
		/// Put the view above of the sibling
		/// </summary>
		public RelativeBuilder AboveOf(View sibling)
        {
			return AlignViewRelativeToY(sibling, ViewDirectionY.AboveOf);
        }

		/// <summary>
		/// Put the view to the left of the sibling
		/// </summary>
		public RelativeBuilder ToLeftOf(View sibling)
        {
			return AlignViewRelativeToX(sibling, ViewDirectionX.LeftOf);
        }

		/// <summary>
		/// Put the view to the right of the sibling
		/// </summary>
		public RelativeBuilder ToRightOf(View sibling)
        {
			return AlignViewRelativeToX(sibling, ViewDirectionX.RightOf);
        }

		/// <summary>
		/// Expand the view to parent width
		/// </summary>
		public RelativeBuilder ExpandViewToParentWidth()
		{
			return ExpandViewToParentX(_lastViewAddedToCollection);
		}

		/// <summary>
		/// Expand the view to parent height
		/// </summary>
		public RelativeBuilder ExpandViewToParentHeight()
		{
			return ExpandViewToParentY(_lastViewAddedToCollection);
		}

		/// <summary>
		/// Expand the view to parent width and height
		/// </summary>
		public RelativeBuilder ExpandViewToParentXY()
		{
			ExpandViewToParentX(_lastViewAddedToCollection);

			ExpandViewToParentY(_lastViewAddedToCollection);

			return this;
		}

		RelativeBuilder ExpandViewToParentX(View view)
		{
			if(view == null)
				throw new ArgumentNullException("view");

			var transaction = _transactionManager.GetTransactionFor(view);

			transaction.ExpandToX = ExpandViewDirectionX.ExpandParentWidth;

			return this;
		}

		RelativeBuilder ExpandViewToParentY(View view)
		{
			if(view == null)
				throw new ArgumentNullException("view");

			var transaction = _transactionManager.GetTransactionFor(view);

			transaction.ExpandToY = ExpandViewDirectionY.ExpandParentHeight;

			return this;
		}

		RelativeBuilder AlignViewRelativeToY(View sibling, ViewDirectionY alignViewY)
		{
			if (sibling == null)
				throw new ArgumentNullException("sibling");
			
			var transaction = _transactionManager.GetTransactionFor(_lastViewAddedToCollection);

			transaction.AlignViewToY = alignViewY;

			transaction.RelativeView = sibling;

			return this;
		}

		RelativeBuilder AlignViewRelativeToX(View sibling, ViewDirectionX alignViewX)
		{
			if (sibling == null)
				throw new ArgumentNullException("sibling");

			var transaction = _transactionManager.GetTransactionFor(_lastViewAddedToCollection);

			transaction.AlignViewToX = alignViewX;

			transaction.RelativeView = sibling;

			return this;
		}

		RelativeBuilder AlignViewX(ViewDirectionX alignViewDirection)
		{
			var transaction = _transactionManager.GetTransactionFor(_lastViewAddedToCollection);

			transaction.AlignViewToX = alignViewDirection;

			return this;
		}

		RelativeBuilder AlignViewY(ViewDirectionY alignViewDirection)
		{
			var transaction = _transactionManager.GetTransactionFor(_lastViewAddedToCollection);

			transaction.AlignViewToY = alignViewDirection;

			return this;
		}

		RelativeBuilder ExpandViewX(ExpandViewDirectionX expandViewDirection)
		{
			var transaction = _transactionManager.GetTransactionFor(_lastViewAddedToCollection);

			transaction.ExpandToX = expandViewDirection;

			return this;
		}

		RelativeBuilder ExpandViewY(ExpandViewDirectionY expandViewDirection)
		{
			var transaction = _transactionManager.GetTransactionFor(_lastViewAddedToCollection);

			transaction.ExpandToY = expandViewDirection;

			return this;
		}

		/// <summary>
		/// Set the padding for the view operating on
		/// </summary>
		/// <returns><see cref="Marioneta.MRelativeLayout"/></returns>
		/// <param name="thickness">A value type holding the padding parameters</param>
		public RelativeBuilder WithPadding(Thickness thickness)
        {
			var transaction = _transactionManager.GetTransactionFor(_lastViewAddedToCollection);

			transaction.Padding = thickness;
            
            return this;
        }

		/// <summary>
		/// Set a dimension to the view
		/// </summary>
		public RelativeBuilder WithDimension(double width, double height)
		{
			return WithDimension
				(new Dimension
					{
						Width = width,
						Height = height
					}
				);
		}

		RelativeBuilder WithDimension(Dimension dimension)
		{
			var transaction = _transactionManager.GetTransactionFor(_lastViewAddedToCollection);

			transaction.ExpandToY = ExpandViewDirectionY.Custom;

			transaction.ExpandToX = ExpandViewDirectionX.Custom;

			transaction.DesiredDimension = dimension;

			return this;
		}

		/// <summary>
		/// This method returns the layout and the view being operating on
		/// </summary>
		public RelativeBuilder ApplyConfiguration(Action<RelativeLayout, View> view)
		{
			view(_relativeLayout, _lastViewAddedToCollection);

			return this;
		}

		/// <summary>
		/// This method render the layout
		/// </summary>
		/// <returns>Return the Layout rendered</returns>
        public View BuildLayout()
        {
			foreach (var transaction in _transactionManager.GetAllTransactions())
            {
                var view = transaction.Key;

                var transactionValues = transaction.Value;

				var xPosition = GetXPositionFor(transactionValues.AlignViewToX,
					view,
					transactionValues.RelativeView,
					transactionValues.Padding.Left);

				var yPosition = GetYPositionFor(transactionValues.AlignViewToY,
					view,
					transactionValues.RelativeView,
					transactionValues.Padding.Top);

				var width = GetWidthFor(transactionValues.ExpandToX, transactionValues.DesiredDimension.Width, transactionValues.Padding.Right);

				var height = GetHeightFor(transactionValues.ExpandToY, transactionValues.DesiredDimension.Height, transactionValues.Padding.Bottom);

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

		private Constraint GetXPositionFor(ViewDirectionX alignViewTo, View view, View relative, double paddingLeft)
        {
            switch (alignViewTo)
            {
				case ViewDirectionX.None:
					{
						return Constraint.RelativeToParent(p => paddingLeft);
					}
                case ViewDirectionX.ParentCenterX:
                    {
						return Constraint.RelativeToParent(p => (p.Width / 2) - (view.Width / 2) + paddingLeft);
                    }
                case ViewDirectionX.ParentLeft:
                    {
						return Constraint.RelativeToParent(p => paddingLeft);
                    }
                case ViewDirectionX.ParentRight:
                    {
						return Constraint.RelativeToParent(p => (p.Width - view.Width) + paddingLeft);
                    }
				case ViewDirectionX.LeftOf:
					{
						return Constraint.RelativeToView(relative, (p,v) => ((v.X + v.Width) - ((view.Width + v.Width))) + paddingLeft);
					}
				case ViewDirectionX.RightOf:
					{
						return Constraint.RelativeToView(relative, (p,v) => (v.X + v.Width) + paddingLeft);
					}
				case ViewDirectionX.AlignLeft:
					{
						return Constraint.RelativeToView(relative, (p,v) => v.X + paddingLeft);
					}
				case ViewDirectionX.AlignRight:
					{
						return Constraint.RelativeToView(relative, (p,v) => (v.X + v.Width) + paddingLeft);
					}
                default:
                    {
						return null;
                    }
            }
        }

		private Constraint GetYPositionFor(ViewDirectionY alignViewTo, View view, View relative, double paddingTop)
        {
            switch (alignViewTo)
            {
			case ViewDirectionY.None:
					{
						return Constraint.RelativeToParent(p => paddingTop);
					}
			case ViewDirectionY.ParentCenterY:
                    {
						return Constraint.RelativeToParent(p => (p.Height / 2) - (view.Height / 2) + paddingTop);
                    }
			case ViewDirectionY.ParentTop:
                    {
						return Constraint.RelativeToParent(p => paddingTop);
                    }
			case ViewDirectionY.ParentBottom:
                    {
						return Constraint.RelativeToView(Bottom, (p,s) => (s.Y + s.Height) + paddingTop);
                    }
			case ViewDirectionY.BelowOf:
					{
						return Constraint.RelativeToView(relative, (p,v) => v.Y + v.Height + paddingTop);
					}
			case ViewDirectionY.AboveOf:
					{
						return Constraint.RelativeToView(relative, (p,v) => ((v.Y + v.Height) - ((view.Height + v.Height))) + paddingTop);
					}
			case ViewDirectionY.AlignAbove:
					{
						return Constraint.RelativeToView(relative, (p,v) => v.Y + paddingTop);
					}
			case ViewDirectionY.AlignBottom:
					{
						return Constraint.RelativeToView(relative, (p,v) => (v.Y + v.Height) + paddingTop);
					}
                default:
                    {
                        return null;
                    }
            }
        }

		private Constraint GetWidthFor(ExpandViewDirectionX expandTo, double desiredWidth ,double paddingRight)
        {
            switch (expandTo)
            {
			case ExpandViewDirectionX.None:
				{
					//We let the RelativeLayout to find the Width itself depending on the View

					return null;
				}
			case ExpandViewDirectionX.Custom:
				{
					return Constraint.RelativeToParent(p => desiredWidth);
				}
			case ExpandViewDirectionX.ExpandParentWidth:
				{
					return Constraint.RelativeToParent(p => p.Width - paddingRight);
				}
			case ExpandViewDirectionX.ExpandViewHorizontallyBetween:
                    {
						throw new NotImplementedException("This feature is not available yet");
                    }
                default:
                    {
						return Constraint.RelativeToParent(p=>0);
                    }
            }
        }

		private Constraint GetHeightFor(ExpandViewDirectionY expandTo, double desiredHeight, double paddingBottom)
        {
            switch (expandTo)
            {
			case ExpandViewDirectionY.None:
				{
					//We let the RelativeLayout to find the Height itself depending on the View

					return null;
				}
			case ExpandViewDirectionY.Custom:
				{
					return Constraint.RelativeToParent(p => desiredHeight);
				}
			case ExpandViewDirectionY.ExpandParentHeight:
				{
					return Constraint.RelativeToParent(p => p.Height - paddingBottom);
				}
			case ExpandViewDirectionY.ExpandViewVerticallyBetween:
                    {
						throw new NotImplementedException("This feature is not available yet");
                    }
                default:
                    {
						return Constraint.RelativeToParent(p=>0);
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
					_transactionManager = null;

                    Left = null;

                    Center = null;

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