using System.Drawing.Text;

namespace DsFunctions.Commons
{
    public class DsFont
    {
        public static string CurrentDirectory { get; set; } = "";

        public const int HEAD_CAPTION_4K = 14;
        public const int CONTENTS_4K = 13;
        public const int TINY_CONTENTS_4K = 12;
        public const int HEAD_CAPTION_FHD = 11;
        public const int CONTENTS_FHD = 10;
        public const int TINY_CONTENTS_FHD = 9;

        public const int IMAGE_4K = 24;
        public const int IMAGE_TOOLBAR_4K = 48;
        public const int IMAGE_FHD = 24;

        /// <summary>
        /// 시스템에 등록되지 않은 별도 폰트를 적용해서 Font객체를 반환하는 클래스
        /// Resources 폴더에 커스텀 폰트파일을 넣어줘야 합니다.
        /// </summary>
        /// <param name="fontSize"></param>
        /// <param name="fontStyle"></param>
        /// <param name="FontFile"></param>
        /// <returns></returns>
        public static Font GetCustomFont(float fontSize, FontStyle fontStyle = FontStyle.Regular, string FontFile = "NanumGothic.ttf")
        {
            if (CurrentDirectory == null || CurrentDirectory == string.Empty)
            {
                CurrentDirectory = Directory.GetCurrentDirectory();
            }

            PrivateFontCollection privateFont = new PrivateFontCollection();
            string fontPath = Path.Combine(CurrentDirectory, "Resources", FontFile);
            string fontBoldPath = Path.Combine(CurrentDirectory, "Resources", FontFile);
            privateFont.AddFontFile(fontPath);
            privateFont.AddFontFile(fontBoldPath);

            return new Font(privateFont.Families[0], fontSize, fontStyle);
            //return new Font(FontFile, fontSize, fontStyle);
        }
    }
}
