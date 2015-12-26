using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Marioneta;
using Xamarin.Forms;

namespace MarionetaTest
{
    [TestClass]
    public class MRelativeLayoutTest
    {
        [TestMethod]
        public void CreateViewRelativeToEachOther()
        {
            var layout = new MRelativeLayout();

            var label = new Label();

            var userNameEntry = new Entry();

            var passwordEntry = new Entry();

            var loginButton = new Button();

            layout.AddNewView(label).AlignViewToParentTop().WithPadding(new Thickness(0,40,0,0));

            layout.AddNewView(userNameEntry).AlignViewParentCenterXY().WithPadding(new Thickness(0, 60, 0, 0));

            layout.AddNewView(passwordEntry).AlignViewBelowOf(userNameEntry).WithPadding(new Thickness(0,16,0,0));

            layout.AddNewView(loginButton).AlignViewToParentBottom().AlignViewBelowOf(passwordEntry).WithPadding(new Thickness(0,10));

            var build = layout.BuildLayout();

            Assert.Fail();
        }

        [TestMethod]
        public void TestUsingStatementWithMRelativeLayout()
        {
            using (var layout = new MRelativeLayout())
            {
                
            }
        }
    }
}
