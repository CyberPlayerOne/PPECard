using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;//for 窗体可拖动代码


namespace PPECard
{
    public partial class AboutForm : Form
    {
        #region 窗体可拖动代码
        //窗体可拖动代码
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        //窗体可拖动代码完
        #endregion

        public AboutForm()
        {
            InitializeComponent();
            foreach (Control control in this.Controls)
            {
                if(!(control is Button))
                control.BackColor = Color.Transparent;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void AboutForm_MouseDown(object sender, MouseEventArgs e)
        {
            //响应窗体拖动
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
    }
}