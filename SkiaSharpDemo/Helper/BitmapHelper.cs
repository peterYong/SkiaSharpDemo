using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkiaSharpDemo.Helper
{
    public class BitmapHelper
    {

        /// <summary>
        /// 位图转为字节数组
        /// </summary>
        /// <param name="saveBitmap"></param>
        /// <returns></returns>
        public static byte[] GetBytes(SKBitmap saveBitmap)
        {
            using (SKImage image = SKImage.FromBitmap(saveBitmap))
            {
                SKData data = image.Encode();
                byte[] bytes = data.ToArray();
                return bytes;
            }
        }

    }
}
