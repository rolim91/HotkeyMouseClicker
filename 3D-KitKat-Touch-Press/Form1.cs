using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace _3D_KitKat_Touch_Press
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        private KitKatWindow kitkatWindow;
        private Thread callThread;
        private HotKey[] hotkeys;


        public enum fsModifiers
        {
            Alt = 0x0001,
            Control = 0x0002,
            Shift = 0x0004,
            Window = 0x0008,
        }



        public Form1()
        {
            InitializeComponent();

            kitkatWindow = new KitKatWindow();
            Rectangle windowParameters = kitkatWindow.getScreenDimensions();

            this.StartPosition = FormStartPosition.Manual;
            this.Left = windowParameters.X;
            this.Top = windowParameters.Y;
            this.Size = new Size(windowParameters.Width, windowParameters.Height);

            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;

            //RegisterHotKey(this.Handle, 100, (int)fsModifiers.Shift | (int)fsModifiers.Alt, (int)Keys.A );
            createHotkey();

            this.callThread = new Thread(new ThreadStart(this.ThreadCall));
            this.callThread.Start();
        }

        //ThreadCall to check if window has been moved
        private void ThreadCall()
        {
            while (true)
            {
                if (this.InvokeRequired)
                {
                    Action callback = moveWindow;
                    this.Invoke(callback);
                }
            }
        }


        private void createHotkey()
        {
            hotkeys = new HotKey[11];

            //Items
            hotkeys[0] = new HotKey((int)fsModifiers.Shift | (int)fsModifiers.Alt, (int)Keys.A, 40, 200);
            hotkeys[1] = new HotKey((int)fsModifiers.Shift | (int)fsModifiers.Alt, (int)Keys.B, 120, 200);
            hotkeys[2] = new HotKey((int)fsModifiers.Shift | (int)fsModifiers.Alt, (int)Keys.C, 200, 200);
            hotkeys[3] = new HotKey((int)fsModifiers.Shift | (int)fsModifiers.Alt, (int)Keys.D, 280, 200);

            //Skills
            hotkeys[4] = new HotKey((int)fsModifiers.Shift | (int)fsModifiers.Alt, (int)Keys.E, 53, 120);
            hotkeys[5] = new HotKey((int)fsModifiers.Shift | (int)fsModifiers.Alt, (int)Keys.F, 160, 120);
            hotkeys[6] = new HotKey((int)fsModifiers.Shift | (int)fsModifiers.Alt, (int)Keys.G, 267, 120);

            //Ping
            hotkeys[7] = new HotKey((int)fsModifiers.Shift | (int)fsModifiers.Alt, (int)Keys.H, 240, 40);

            //MonsterLock
            hotkeys[8] = new HotKey((int)fsModifiers.Shift | (int)fsModifiers.Alt, (int)Keys.I, 27, 40); //Left
            hotkeys[9] = new HotKey((int)fsModifiers.Shift | (int)fsModifiers.Alt, (int)Keys.J, 80, 40); //Middle
            hotkeys[10] = new HotKey((int)fsModifiers.Shift | (int)fsModifiers.Alt, (int)Keys.K, 133, 40); //Right

            for (int i = 0; i < hotkeys.Length; i++)
            {
                RegisterHotKey(this.Handle, i, hotkeys[i].FsModifier, hotkeys[i].Key);
            }
        }

        private void moveWindow()
        {
            Rectangle windowParameters = kitkatWindow.getScreenDimensions();


            this.Left = windowParameters.X;
            this.Top = windowParameters.Y;
        }

        //paint rectangle on location of the touch screen
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphicsObj = this.CreateGraphics();
            Pen myPen = new Pen(System.Drawing.Color.Red, 0.5f);
            Rectangle touchParameters = kitkatWindow.getTouchRectDimensions();
            graphicsObj.DrawRectangle(myPen, touchParameters.X, touchParameters.Y, touchParameters.Width, touchParameters.Height);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            for (int i = 0; i < hotkeys.Length; i++)
            {
                UnregisterHotKey(this.Handle, i);
            }
        }

        protected override void WndProc(ref Message keyPressed)
        {
            base.WndProc(ref keyPressed);
            
            if (keyPressed.Msg == 0x0312)
            {
                
                int hotKeyValue = keyPressed.WParam.ToInt32();

                //call function to input mouse click on HotKey
                int xPos = kitkatWindow.getScreenDimensions().X + kitkatWindow.getTouchRectDimensions().X + hotkeys[hotKeyValue].X_location;
                int yPos = kitkatWindow.getScreenDimensions().Y + kitkatWindow.getTouchRectDimensions().Y + hotkeys[hotKeyValue].Y_location;

                //MessageBox.Show("Hotkey has been pressed! Click at X=" + xPos + " y=" + yPos);
                LeftMouseClick(xPos, yPos);
                
            }
        }

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            System.Threading.Thread.Sleep(50);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }
    }
}
