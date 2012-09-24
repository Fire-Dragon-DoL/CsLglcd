using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsLglcd;
using CsLglcd.UI;
using System.Drawing;

namespace HelloWorld1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing basic functionality for CsLglcd");
            Console.WriteLine("Loading applet...");

            using (Lglcd lglcd = new Lglcd())
            {
                Applet helloWorldApplet = new Applet();
                helloWorldApplet.SupportedDevices = SupportedDevices.QVGA;
                helloWorldApplet.Title = "Hello World 1";
                helloWorldApplet.Connect();

                Device<QvgaImageUpdater> qvgaDevice = new Device<QvgaImageUpdater>();
                qvgaDevice.Applet = helloWorldApplet;
                qvgaDevice.DeviceType = Devices.QVGA;
                qvgaDevice.Attach();

                Console.WriteLine("Press ENTER to draw a test image");
                Console.ReadLine();

                Bitmap testImage = qvgaDevice.SpecializedImageUpdater.CreateValidImage();
                using (Graphics g = Graphics.FromImage(testImage))
                {
                    g.FillRectangle(Brushes.Red, 0, 0, testImage.Width, testImage.Height);
                    g.DrawString("Hello world", new Font("monospace", 12f), Brushes.White, 15f, 15f);
                }

                qvgaDevice.SpecializedImageUpdater.SetPixels(testImage);
                qvgaDevice.Update();

                Console.WriteLine("Press ENTER to cleanup memory");
                Console.ReadLine();

                testImage.Dispose();
                qvgaDevice.Detach();
                qvgaDevice.Dispose();
                helloWorldApplet.Disconnect();
                helloWorldApplet.Dispose();
            }

            Console.WriteLine("Press ENTER to close application");
            Console.ReadLine();
        }
    }
}
