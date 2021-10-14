using Microsoft.Win32;
using System;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SampleInstallFontsOnWindows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //font name list
        private readonly string[] fontList = { "NanumSquare_acB", "NanumSquare_acEB", "NanumSquare_acL",
                "NanumSquare_acR", "NanumSquareB", "NanumSquareEB", "NanumSquareL", "NanumSquareR"};

        //https://www.it-swarm-ko.tech/ko/c%23/%ed%94%84%eb%a1%9c%ea%b7%b8%eb%9e%98%eb%b0%8d-%eb%b0%a9%ec%8b%9d%ec%9c%bc%eb%a1%9c-%ea%b8%80%ea%bc%b4%ec%9d%84-%ec%84%a4%ec%b9%98%ed%95%98%eb%8a%94-%eb%b0%a9%eb%b2%95-c/1045179572/
        [DllImport("gdi32", EntryPoint = "AddFontResource")]
        public static extern int AddFontResourceA(string lpFileName);
        [DllImport("gdi32.dll")]
        private static extern int AddFontResource(string lpszFilename);
        [DllImport("gdi32.dll")]
        private static extern int CreateScalableFontResource(uint fdwHidden, string
            lpszFontRes, string lpszFontFile, string lpszCurrentPath);
        private static void RegisterFont(string contentFontName)
        {
            string sourceDir2 = Directory.GetCurrentDirectory() + @"\font";
            // Creates the full path where your font will be installed
            var fontDestination = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), contentFontName);

            if (!File.Exists(fontDestination))
            {
                // Copies font to destination
                File.Copy(Path.Combine(sourceDir2, contentFontName), fontDestination);

                // Retrieves font name
                // Makes sure you reference System.Drawing
                PrivateFontCollection fontCol = new PrivateFontCollection();
                fontCol.AddFontFile(fontDestination);
                var actualFontName = fontCol.Families[0].Name;

                //Add font
                AddFontResource(fontDestination);
                //Add registry entry   
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts", actualFontName, contentFontName, RegistryValueKind.String);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int n = 0; n < fontList.Length; n++)
            {
                RegisterFont($"{fontList[n]}.ttf");
            }
        }
    }
}
