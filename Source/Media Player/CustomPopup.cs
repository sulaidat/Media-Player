using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Media_Player
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Media_Player"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Media_Player;assembly=Media_Player"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomPopup/>
    ///
    /// </summary>
    public class CustomPopup : Popup
    {
        protected override void OnOpened(EventArgs e)
        {
            var friend = this.PlacementTarget;
            friend.QueryCursor += friend_QueryCursor;

            base.OnOpened(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            var friend = this.PlacementTarget;
            friend.QueryCursor -= friend_QueryCursor;

            base.OnClosed(e);
        }

        private void friend_QueryCursor(object sender, System.Windows.Input.QueryCursorEventArgs e)
        {
            this.HorizontalOffset += +0.1;
            this.HorizontalOffset += -0.1;
        }
    }
}
