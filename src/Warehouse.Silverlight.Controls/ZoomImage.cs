using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Warehouse.Silverlight.Controls
{
    public class ZoomImage : Control
    {
        private const string Image = "Image";
        private const string ZoomInButton = "ZoomInButton";
        private const string ZoomOutButton = "ZoomOutButton";

        private Image image;
        private Button zoomInButton;
        private Button zoomOutButton;

        private readonly double[] zoomValues = { 0.125, 0.25, 0.5, 0.75, 1, 1.25, 1.5, 2 };
        private int zoomIndex = 2;
        private double originalWidth;
        private double originalHeight;

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
        }

        private void OnImageOpened(object sender, RoutedEventArgs e)
        {
            image.ImageOpened -= OnImageOpened;

            var source = (BitmapImage) image.Source;
            originalWidth = source.PixelWidth;
            originalHeight = source.PixelHeight;
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

            image.Width = zoomValues[zoomIndex] * originalWidth;
            image.Height = zoomValues[zoomIndex] * originalHeight;
        }
    }
}
