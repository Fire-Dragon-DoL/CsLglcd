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
                using (Graphics drawer = Graphics.FromImage(testImage))
                {
                    QvgaScreen form = new QvgaScreen(helloWorldApplet, qvgaDevice);
                    ContainerControl controls = new ContainerControl();
                    form.Control = controls;
                    controls.AddControl(new TextControl() { Text = "test", X = 5, BaseFont = form.BaseFont });
                    var progressbarcontrol = new ProgressBarControl() { X = 8, Y = 100 };
                    controls.AddControl(progressbarcontrol);

                    form.Draw(testImage, drawer);
                    Console.WriteLine("Current percentage: {0}%", progressbarcontrol.Percentage * 100);
                    qvgaDevice.SpecializedImageUpdater.SetPixels(testImage);
                    qvgaDevice.Update();

                    Console.WriteLine("Press ENTER for 0%");
                    Console.ReadLine();

                    progressbarcontrol.Current = 0;
                    form.Draw(testImage, drawer);
                    Console.WriteLine("Current percentage: {0}%", progressbarcontrol.Percentage * 100);
                    qvgaDevice.SpecializedImageUpdater.SetPixels(testImage);
                    qvgaDevice.Update();

                    Console.WriteLine("Press ENTER for 100%");
                    Console.ReadLine();

                    progressbarcontrol.Current = 1000;
                    form.Draw(testImage, drawer);
                    Console.WriteLine("Current percentage: {0}%", progressbarcontrol.Percentage * 100);
                    qvgaDevice.SpecializedImageUpdater.SetPixels(testImage);
                    qvgaDevice.Update();

                    // Try raising app to front

                    Console.WriteLine("Press ENTER for 100% and BRING APPLICATION TO FRONT");
                    Console.ReadLine();

                    progressbarcontrol.Current = 1000;
                    form.Draw(testImage, drawer);
                    Console.WriteLine("Current percentage: {0}%", progressbarcontrol.Percentage * 100);
                    qvgaDevice.ForegroundApplet = true;
                    qvgaDevice.SpecializedImageUpdater.SetPixels(testImage);
                    qvgaDevice.Update(UpdatePriorities.Alert);
                }

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
