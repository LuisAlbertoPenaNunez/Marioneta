# Marioneta
Marioneta is an open source project that aims to provide a powerful and cross platform set of controls to work with Xamarin Forms.

##How does it works?

Marioneta was built with Xamarin Forms in mind, since Xamarin Forms is at its early stage there is still a need for a lot of controls. Marioneta is here to fill that missing part recycling what Xamarin has created with Xamarin Forms, so we guarantee you that every control of Marioneta will just works with no issues.

##Getting Started

There is a variety of controls to use in Marioneta, you can pull request your own too!

###RelativeBuilder

```cs
var builder = new RelativeBuilder();

			builder
				.AddView(image)
				.ExpandViewToParentXY();

			builder
				.AddView(shim1)
				.ExpandViewToParentXY();

			builder
				.AddView(entryAndButtonContainer)
				.AlignParentCenterXY();

			return builder.BuildLayout();
```

![Image of Ziti](http://s23.postimg.org/kslfxm8gr/Simulator_Screen_Shot_Dec_26_2015_19_20_59.png)

> This is just a preview of what you can do with it

##RelativeBuilder basic method documentation

```cs

    RelativeBuilder AddView(View viewToBePlaced);

    RelativeBuilder ExpandViewHorizontallyBetween(View left, View right);

    RelativeBuilder ExpandViewVerticallyBetween(View top, View bottom);

    RelativeBuilder AlignParentLeft();

    RelativeBuilder AlignParentTop();

    RelativeBuilder AlignParentRight();

    RelativeBuilder AlignParentBottom();

    RelativeBuilder AlignParentCenterVertical();

    RelativeBuilder AlignParentCenterHorizontal();

    RelativeBuilder AlignParentCenterXY();

    RelativeBuilder AlignLeft (View sibling);

    RelativeBuilder AlignTop (View sibling);

    RelativeBuilder AlignRight (View sibling);

    RelativeBuilder AlignBottom (View sibling);

    RelativeBuilder BelowOf(View sibling);

    RelativeBuilder AboveOf(View sibling);

    RelativeBuilder ToLeftOf(View sibling);

    RelativeBuilder ToRightOf(View sibling);

    RelativeBuilder ExpandViewToParentWidth();

    RelativeBuilder ExpandViewToParentHeight();

    RelativeBuilder ExpandViewToParentXY();

    RelativeBuilder ExpandViewToParentX(View view);

    RelativeBuilder ExpandViewToParentY(View view);

    RelativeBuilder AlignViewRelativeToY(View sibling, ViewDirectionY alignViewY);

    RelativeBuilder AlignViewRelativeToX(View sibling, ViewDirectionX alignViewX);

    RelativeBuilder AlignViewX(ViewDirectionX alignViewDirection);

    RelativeBuilder AlignViewY(ViewDirectionY alignViewDirection);

    RelativeBuilder ExpandViewX(ExpandViewDirectionX expandViewDirection);

    RelativeBuilder ExpandViewY(ExpandViewDirectionY expandViewDirection);

    RelativeBuilder WithPadding(Thickness thickness);

    RelativeBuilder WithDimension(double width, double height);

    RelativeBuilder WithDimension(Dimension dimension);

    RelativeBuilder ApplyConfiguration(Action<RelativeLayout, View> view);

    View BuildLayout();

    Constraint GetXPositionFor(ViewDirectionX alignViewTo, View view, View relative, double paddingLeft);

    Constraint GetYPositionFor(ViewDirectionY alignViewTo, View view, View relative, double paddingTop);

    Constraint GetWidthFor(ExpandViewDirectionX expandTo, double desiredWidth ,double paddingRight);

    Constraint GetHeightFor(ExpandViewDirectionY expandTo, double desiredHeight, double paddingBottom);

```
