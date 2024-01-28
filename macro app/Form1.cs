using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace macro_app
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        const int KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        static void SimulateKeyPress(byte keyCode)
        {
            keybd_event(keyCode, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
        }

        public static bool isEnabled = false;
        public static bool isPaused = false;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (button1.Text.Equals("start"))
            {
                button1.Text = "stop";
                isEnabled = true;
                input_Listener();
                
                MessageBox.Show("Macro is running...");
                WindowState = FormWindowState.Minimized;
            }
            
            else
            {
                button1.Text = "start";
                isEnabled = false;
            }
        }

        static async void input_Listener()
        {
            var R = 0x52;

            while (isEnabled)
            {
                // Pause listener if needed
                pause_listener();
                
                // Check if the mouse button is clicked
                if (((GetAsyncKeyState(R) & 0x8000) != 0) && !isPaused)
                {
                    // Simulate pressing keys '1' through '7' simultaneously
                    SimulateKeyPress(0x31); // '1'
                    SimulateKeyPress(0x32); // '2'
                    SimulateKeyPress(0x33); // '3'
                    SimulateKeyPress(0x34); // '4'
                    SimulateKeyPress(0x35); // '5'
                    SimulateKeyPress(0x36); // '6'
                    SimulateKeyPress(0x37); // '7'
                }

                // change the loop speed, otherwise one click might invoke the function multiple times at once
                await Task.Delay(100);
            }
        }

        // press scroll wheel to pause
        static void pause_listener()
        {
            var ScrollWheelClick = 0x04;
                
            if ((GetAsyncKeyState(ScrollWheelClick) & 0x8000) != 0)
            {
                // Pause/Unpause the macro
                isPaused = !isPaused;
            }
        }

    }

}