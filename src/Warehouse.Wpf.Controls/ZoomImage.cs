using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Warehouse.Wpf.Controls
{
    public class ZoomImage : Control
    {
        private const string Image = "Image";
        private const string ZoomInButton = "ZoomInButton";
        private const string ZoomOutButton = "ZoomOutButton";
        private const string RotateButton = "RotateButton";
        private const string Canvas = "Canvas";
        private const string TranslateTransform = "TranslateTransform";
        private const string RotateTransform = "RotateTransform";

        private Image image;
        private Button zoomInButton;
        private Button zoomOutButton;
        private Button rotateButton;
        private Canvas canvas;
        private TranslateTransform translateTransform;
        private RotateTransform rotateTransform;

        private BitmapImage bitmapImage;
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
            translateTransform = GetTemplateChild(TranslateTransform) as TranslateTransform;
            rotateTransform = GetTemplateChild(RotateTransform) as RotateTransform;

            Subscribe();

            InitImage();
        }

        private void InitImage()
        {
            if (Uri == null) return;
            if (image == null) return;

            if (bitmapImage != null)
            {
                bitmapImage.DownloadCompleted -= OnImageOpened;
                image.Source = null;
                bitmapImage = null;
            }

            bitmapImage = new BitmapImage(Uri);
            if (bitmapImage.IsDownloading)
            {
                bitmapImage.DownloadCompleted += OnImageOpened;
            }
            else
            {
                OnImageOpened(bitmapImage, EventArgs.Empty);
            }

            image.Source = bitmapImage;
        }

        private void Subscribe()
        {
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

        private void OnImageOpened(object sender, EventArgs e)
        {
            zoomIndex = 1;

            rotateTransform.Angle = 0;

            translateTransform. X = 0;
            translateTransform.Y = 0;

            w = bitmapImage.PixelWidth;
            h = bitmapImage.PixelHeight;

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
            image.Width = k * w;
            image.Height = k * h;

            RefreshCanvas();
        }

        private void Rotate(object sender, RoutedEventArgs e)
        {
            var angle = Convert.ToInt32(rotateTransform.Angle);

            angle += 90;

            if (angle == 360)
            {
                angle = 0;
            }

            rotateTransform.Angle = angle;

            RefreshCanvas();
        }

        private void RefreshCanvas()
        {
            var k = zoomValues[zoomIndex];

            var angle = Convert.ToInt32(rotateTransform.Angle);

            var original = angle % 180 == 0;
            canvas.Width = k * (original ? w : h);
            canvas.Height = k * (original ? h : w);

            if (angle == 0)
            {
                translateTransform.X = 0;
                translateTransform.Y = 0;
            }
            else if (angle == 90)
            {
                translateTransform.X = h * k;
                translateTransform.Y = 0;
            }
            else if (angle == 180)
            {
                translateTransform.X = w * k;
                translateTransform.Y = h * k;
            }
            else if (angle == 270)
            {
                translateTransform.X = 0;
                translateTransform.Y = w * k;
            }

        }
    }
}
