using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;

namespace _3D_KitKat_Touch_Press
{
    class InputRedirectionWindow
    {
        private const string applicationName = "kit-kat";
        public const int X_TOUCH_OFFSET = 0;
        public const int Y_TOUCH_OFFSET = 0;
        public const int TOUCH_WIDTH = 321;
        public const int TOUCH_HEIGHT = 240;

        private Rectangle screenRectDimensions;
        private Rectangle touchRectDimensions;
        private Graphics kitKatGraphics;
        private Process kitKatProcess;


        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, out Rectangle rectangle);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);


        public InputRedirectionWindow()
        {
            kitKatProcess = lookForProcess();
            screenRectDimensions = getWindowDimensions();
            kitKatGraphics = Graphics.FromHdc(GetDC(IntPtr.Zero));
            touchRectDimensions = new Rectangle(X_TOUCH_OFFSET, Y_TOUCH_OFFSET, TOUCH_WIDTH, TOUCH_HEIGHT);
        }

        public Rectangle getScreenDimensions()
        {
            screenRectDimensions = getWindowDimensions();
            return this.screenRectDimensions;
        }

        public Rectangle getTouchRectDimensions()
        {
            screenRectDimensions = getWindowDimensions();
            return this.touchRectDimensions;
        }

        //Look for KitKat Process
        private Process lookForProcess()
        {
            Process[] kitkatProcesses = Process.GetProcessesByName("kit-kat");

            while (kitkatProcesses.Length <= 0)
            {
                kitkatProcesses = Process.GetProcessesByName("kit-kat");
                Console.WriteLine("No Kit-Kat Processes Available");
            }

            Console.WriteLine("Kit-Kat Process Found");

            return kitkatProcesses[0];
        }

        //Get the Dimensions for the KitKat application
        private Rectangle getWindowDimensions()
        {


            IntPtr kitKatPtr = kitKatProcess.MainWindowHandle;

            //Get Size
            Rectangle kitkatRect = new Rectangle();
            GetWindowRect(kitKatPtr, out kitkatRect);
            kitkatRect.Width = kitkatRect.Width - kitkatRect.X;
            kitkatRect.Height = kitkatRect.Height - kitkatRect.Y;

            return kitkatRect;
        }

        public void drawWindowRect()
        {
            screenRectDimensions = getWindowDimensions();
            kitKatGraphics.DrawRectangle(new Pen(Color.Red, 7f), screenRectDimensions);
        }

        public void drawTouchRect()
        {
            screenRectDimensions = getWindowDimensions();
            screenRectDimensions.X = screenRectDimensions.X + X_TOUCH_OFFSET;
            screenRectDimensions.Y = screenRectDimensions.Y + Y_TOUCH_OFFSET;

            kitKatGraphics.DrawRectangle(new Pen(Color.Red, 0.5f), screenRectDimensions.X, screenRectDimensions.Y, TOUCH_WIDTH, TOUCH_HEIGHT);
        }
    }
}
