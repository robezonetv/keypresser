using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace KeysPresser
{
    public partial class Form1 : Form
    {

        private int status = 0;

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);

        public Form1()
        {
            InitializeComponent();
        }
        public class Apps
        {
            public string name { get; set; }
            public int pid { get; set; }
        }

        public void UpdateComboBox(string data)
        {
            comboBox1.Items.Add(data);
        }
        
        private async void btn_Start_Click(object sender, EventArgs e)
        {
            this.TopMost = true;

            this.comboBox1.Enabled = false;
            this.btn_Start.Enabled = false;
            this.btn_Refresh.Enabled = false;
            this.timeout.Enabled = false;

            this.status = 0;
            int pid = ((Apps)this.comboBox1.SelectedItem).pid;
            Process proc = Process.GetProcessById(pid);
            int j;
            while (true)
            {
                for (int i = 1; i <= 10; i++)
                {
                    if (i == 10)
                    {
                        j = 0;
                    }
                    else
                    {
                        j = i;
                    }

                    if (this.status == 0)
                    {
                        SetForegroundWindow(proc.MainWindowHandle);
                        SendKeys.SendWait("{" + j.ToString() + "}");
                        await Task.Delay((int)this.timeout.Value);
                        //Thread.Sleep((int) this.timeout.Value);
                    }
                    else
                    {
                        break;
                    }
                }
                if (this.status == 1)
                {
                    break;
                }
            }
        }
        private void btn_Stop_Click(object sender, EventArgs e)
        {
            this.status = 1;
            this.comboBox1.Enabled = true;
            this.btn_Start.Enabled = true;
            this.btn_Refresh.Enabled = true;
            this.timeout.Enabled = true;
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            this.comboBox1.DataSource = null;
            //this.comboBox1.Items.Clear();
            var dataSource = new List<Apps>();

            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    dataSource.Add(new Apps() { name = process.Id.ToString() + " ["+process.ProcessName+"] "+process.MainWindowTitle, pid = process.Id });
                    //this.UpdateComboBox(process.ProcessName + " (" + process.MainWindowTitle + ")");
                }
            }

            this.comboBox1.DataSource = dataSource;
            this.comboBox1.DisplayMember = "name";
            this.comboBox1.ValueMember = "pid";

        }
    }
}
