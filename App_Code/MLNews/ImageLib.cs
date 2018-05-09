using System;

namespace DotNetNuke.News
{
    public class ImageLib
    {
        public static string GetExtension(string file)
        {
            string[] s = file.Split('.');
            if (s.Length > 1)
                return s[s.Length - 1];
            return "";
        }

        public static bool IsImage(string file)
        {
            string ext = GetExtension(file);
            ext = ext.ToLower();
            if (ext == "gif" || ext == "jpg" || ext == "jpeg" || ext == "png")
                return true;
            return false;
        }

        public static string GetFileName(string file, char spliter)
        {
            string[] s = file.Split(spliter);
            if (s.Length > 1)
                return s[s.Length - 1];
            return "";
        }
    }
}