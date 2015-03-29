using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Warehouse.Silverlight.Controls
{
    public class ZoomImage : Control
    {
        private const string Image = "Image";
        private const string ZoomInButton = "ZoomInButton";
        private const string ZoomOutButton = "ZoomOutButton";
        private const string RotateButton = "RotateButton";
        private const string Canvas = "Canvas";
        private const string Transform = "Transform";

        private Image image;
        private Button zoomInButton;
        private Button zoomOutButton;
        private Button rotateButton;
        private Canvas canvas;
        private RotateTransform transform;

        private readonly double[] zoomValues = { 0.125, 0.25, 0.5, 0.75, 1, 1.25, 1.5, 2 };
        private int zoomIndex;
        private double w;
        private double h;

        public ZoomImage()
        {
            DefaultStyleKey = typeof (ZoomImage);
        }

        #region Uri

        public Uri Uri
        {
            get { return (Uri)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(Uri), typeof(ZoomImage), new PropertyMetadata(null, OnUriChanged));

        private static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZoomImage)d).InitImage();
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Unsubscribe();

            image = GetTemplateChild(Image) as Image;
            zoomInButton = GetTemplateChild(ZoomInButton) as Button;
            zoomOutButton = GetTemplateChild(ZoomOutButton) as Button;
            rotateButton = GetTemplateChild(RotateButton) as Button;
            canvas = GetTemplateChild(Canvas) as Canvas;
            transform = GetTemplateChild(Transform) as RotateTransform;

            Subscribe();

            InitImage();
        }

        private void InitImage()
        {
            if (Uri == null) return;
            if (image == null) return;

            image.Source = new BitmapImage(Uri);
        }

        private void Subscribe()
        {
            if (image != null)
            {
                image.ImageOpened += OnImageOpened;
            }
            if (zoomInButton != null)
            {
                zoomInButton.Click += ZoomInButtonClick;
            }
            if (zoomOutButton != null)
            {
                zoomOutButton.Click += ZoomOutButtonClick;
            }
            if (rotateButton != null)
            {
                rotateButton.Click += Rotate;
            }
        }

        private void Unsubscribe()
        {
            if (image != null)
            {
                image.ImageOpened -= OnImageOpened;
                image = null;
            }
            if (zoomInButton != null)
            {
                zoomInButton.Click -= ZoomInButtonClick;
                zoomInButton = null;
            }
            if (zoomOutButton != null)
            {
                zoomOutButton.Click -= ZoomOutButtonClick;
                zoomOutButton = null;
            }
            if (rotateButton != null)
            {
                rotateButton.Click -= Rotate;
            }
        }

        private void OnImageOpened(object sender, RoutedEventArgs e)
        {
            zoomIndex = 1;
            transform.Angle = 0;
            image.SetValue(System.Windows.Controls.Canvas.TopProperty, 0d);
            image.SetValue(System.Windows.Controls.Canvas.LeftProperty, 0d);

            var source = (BitmapImage)image.Source;
            w = source.PixelWidth;
            h = source.PixelHeight;

            Zoom(0);
        }

        private void ZoomInButtonClick(object sender, RoutedEventArgs e)
        {
            Zoom(1);
        }

        private void ZoomOutButtonClick(object sender, RoutedEventArgs e)
        {
            Zoom(-1);
        }

        private void Zoom(int zoomIndexDelta)
        {
            zoomIndex += zoomIndexDelta;

            zoomInButton.IsEnabled = zoomIndex < zoomValues.Length - 1;
            zoomOutButton.IsEnabled = 0 < zoomIndex;

            var k = zoomValues[zoomIndex];

            canvas.Width = k * w;
            canvas.Height = k * h;

            image.Width = k * w;
            image.Height = k * h;
        }

        private void Rotate(object sender, RoutedEventArgs e)
        {
            transform.Angle += 90;

            var angle = Convert.ToInt32(transform.Angle);
            if (angle == 360)
            {
                angle = 0;
                transform.Angle = 0;
            }

            var k = zoomValues[zoomIndex];
            var original = angle % 180 == 0;
            canvas.Width = k * (original ? w : h);
            canvas.Height = k * (original ? h : w);

            if (angle == 0)
            {
                image.SetValue(System.Windows.Controls.Canvas.TopProperty, 0d);
                image.SetValue(System.Windows.Controls.Canvas.LeftProperty, 0d);
            }
            else if (angle == 90)
            {
                image.SetValue(System.Windows.Controls.Canvas.TopProperty, 0d);
                image.SetValue(System.Windows.Controls.Canvas.LeftProperty, h * k);
            }
            else if (angle == 180)
            {
                image.SetValue(System.Windows.Controls.Canvas.TopProperty, h * k);
                image.SetValue(System.Windows.Controls.Canvas.LeftProperty, w * k);
            }
            else if (angle == 270)
            {
                image.SetValue(System.Windows.Controls.Canvas.TopProperty, w * k);
                image.SetValue(System.Windows.Controls.Canvas.LeftProperty, 0d);
            }
        }
    }
}
