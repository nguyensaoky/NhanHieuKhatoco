using System;
using System.Web;
using System.Web.Util;
using System.Drawing;
using System.Drawing.Imaging;

namespace DotNetNuke.Gallery
{
	/// <summary>
	/// Summary description for ThumbGen.
	/// </summary>
	public class ThumbGen : IHttpHandler
	{
        public Size NewImageSize(int OriginalHeight, int OriginalWidth, double FormatSize)
        {
            Size NewSize;
            double tempval;
            if (OriginalHeight > FormatSize || OriginalWidth > FormatSize)
            {
                if (OriginalHeight > OriginalWidth)
                    tempval = FormatSize / Convert.ToDouble(OriginalHeight);
                else
                    tempval = FormatSize / Convert.ToDouble(OriginalWidth);

                NewSize = new Size(Convert.ToInt32(tempval * OriginalWidth), Convert.ToInt32(tempval * OriginalHeight));
            }
            else
            {
                NewSize = new Size(OriginalWidth, OriginalHeight);
            }
            return NewSize;
        } 

		public void ProcessRequest (HttpContext context)
		{
            if (context.Request["path"] != null)
			{
                string img = (string)context.Request["path"];
				img=context.Server.MapPath(img);
				System.Drawing.Image.GetThumbnailImageAbort myCallback =
				new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
				Bitmap bitmap = new Bitmap(img);
				ImageFormat imgformat=bitmap.RawFormat;
                double thumbSize = 100.0;
                if (context.Request["size"] != null)
                {
                    thumbSize = double.Parse(context.Request["size"]);
                }
                Size size = NewImageSize(bitmap.Height, bitmap.Width, thumbSize);
                System.Drawing.Image imgOut = bitmap.GetThumbnailImage(size.Width, size.Height, myCallback, IntPtr.Zero);
				bitmap.Dispose();
                context.Response.ContentType = "image/jpeg";
				imgOut.Save(context.Response.OutputStream, ImageFormat.Jpeg);
				imgOut.Dispose();
			}
		}

		public bool IsReusable
		{
			get {return true;}
		}

        public bool ThumbnailCallback()
		{
			return false;
		}
	}
}
