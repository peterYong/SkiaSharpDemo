using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkiaSharpDemo.Font
{

    public static class Typeface
    {
        public static TypefaseWithMuiltWeight PingFang = new TypefaseWithMuiltWeight
        {
            Regular = SKTypeface.FromFile("Font/simfang.ttf"), //此文件不行
            //Medium = SKTypeface.FromFile("Fonts/PingfangScMedium.ttf")
        };
    }


    public class TypefaseWithMuiltWeight
    {
        public SKTypeface Regular { get; set; }
        public SKTypeface Medium { get; set; }
    }

}
