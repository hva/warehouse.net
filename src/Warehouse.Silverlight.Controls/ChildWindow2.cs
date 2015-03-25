using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Warehouse.Silverlight.Controls
{
    public class ChildWindow2 : ChildWindow
    {
        private Grid root;
        private Point m;
        private Point h;

        public ChildWindow2()
        {
            DefaultStyleKey = typeof(ChildWindow);
            if (MinWidth == 0) MinWidth = 300;
            if (MinHeight == 0) MinHeight = 200;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            root = (Grid)GetTemplateChild("ContentRoot");
            var th = new Thumb { Style = Application.Current.Resources["Thumb_ChildWindow2_Style"] as Style };
            th.DragStarted += delegate
            {
                h = new Point(RootWidth, RootHeight);
                m = new Point(0, 0);
            };
            th.DragDelta += DragDelta;
            root.Children.Add(th);
        }

        private void DragDelta(object sender, DragDeltaEventArgs e)
        {
            m.X += e.HorizontalChange;
            m.Y += e.VerticalChange;
            double width = h.X + m.X;
            double height = h.Y + m.Y;
            if (width > MinWidth && height > MinHeight)
            {
                RootWidth = width;
                RootHeight = height;
            }
        }

        private double RootWidth
        {
            get { return (double)root.GetValue(WidthProperty); }
            set { root.SetValue(WidthProperty, value); }
        }

        private double RootHeight
        {
            get { return (double)root.GetValue(HeightProperty); }
            set { root.SetValue(HeightProperty, value); }
        }
    }
}
