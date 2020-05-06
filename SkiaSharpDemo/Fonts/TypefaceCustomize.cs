using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SkiaSharpDemo.Font
{
    /// <summary>
    /// 自定义字体
    /// </summary>
    public class TypefaceCustomize
    {
        /// <summary>
        /// 常规字体（含中文）
        /// </summary>
        public static SKTypeface Regular { get; set; }

        /// <summary>
        /// Palace Script MT Semi Bold
        /// </summary>
        public static SKTypeface PalaceScript { get; set; }


        /// <summary>
        /// 字体初始化
        /// </summary>
        public static void Init()
        {
            SKFontManager fontManager = SKFontManager.Default;
            //IEnumerable<string> vs = fontManager.FontFamilies;
            //paintText.Typeface = fontManager.MatchCharacter('时');  //兼容中文
            //paintText.Typeface = fontManager.MatchFamily("PALSCRI", SKFontStyle.Bold);  //不设置中文时可用
            //字体粗细、字体宽度（压缩/扩展）、字体倾斜
            Regular = fontManager.MatchCharacter("", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright, null, '时');

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(TypefaceCustomize)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("SkiaSharpDemo.Fonts.Palace Script MT Semi Bold.ttf");
            PalaceScript = SKTypeface.FromStream(stream);

        }


    }


}
