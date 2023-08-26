using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ComicLib
{
    /// <summary>
    /// Предоставляет функционал для преобразования изображений
    /// </summary>
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                byte[] img = (byte[])value;

                using (MemoryStream byteStream = new MemoryStream(img))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = byteStream;
                    image.EndInit();
                    return image;
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public BitmapImage BitmapImageConvert(string path)
        {
            Uri uri = new Uri(path);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = uri;
            image.EndInit();

            return image;
        }

        public BitmapImage BitmapImageFromByteArrayConvert(byte[] img)
        {
            using (MemoryStream byteStream = new MemoryStream(img))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = byteStream;
                image.EndInit();
                return image;
            }
        }

        public byte[] GetFromBitmapImageControl(BitmapImage image)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(memStream);
                return memStream.ToArray();
            }
        }
    }
}
