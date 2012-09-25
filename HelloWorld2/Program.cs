using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsLglcd;
using CsLglcd.UI.Windows;
using System.Drawing;

namespace HelloWorld2
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
                helloWorldApplet.Title = "Hello World 2";
                helloWorldApplet.Connect();

                Device<QvgaImageUpdater> qvgaDevice = new Device<QvgaImageUpdater>();
                qvgaDevice.Applet = helloWorldApplet;
                qvgaDevice.DeviceType = Devices.QVGA;
                qvgaDevice.Attach();

                Console.WriteLine("Press ENTER to draw a test image with CsLglcd.UI");
                Console.ReadLine();

                Bitmap testImage = qvgaDevice.SpecializedImageUpdater.CreateValidImage();
                Screen form = new Screen()
                {
                    Title = "Hello World 2"
                };
                form.Draw(testImage);

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
