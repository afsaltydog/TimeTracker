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
using System.Text.RegularExpressions;

namespace TimeTracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Stopwatch stopWatch = new Stopwatch();

        private void Form1_Load(object sender, EventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                stopWatch.ElapsedMilliseconds.ToString();
                TimeSpan ts = stopWatch.Elapsed;
                lblTimer.Text = GetElapsedTime(ts);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            stopWatch.Start();
            lblTimer.Text = DateTime.Now.ToString();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopWatch.Stop();

            stopWatch.ElapsedMilliseconds.ToString();
            TimeSpan ts = stopWatch.Elapsed;
            
            lblTimer.Text = GetElapsedTime(ts);
        }

        private string GetElapsedTime(TimeSpan ts)
        {
            if (ts.Hours > 0)
                return string.Format("{0} hours {1} minutes {2} seconds", ts.Hours.ToString(), ts.Minutes.ToString(), ts.Seconds.ToString());

            if (ts.Minutes > 0)
                return string.Format("{0} minutes {1} seconds", ts.Minutes.ToString(), ts.Seconds.ToString());

            return string.Format("{0} seconds", ts.Seconds.ToString());
        }

        InputBox.InputBoxValidation validation = delegate (string val)
        {
            if (val == "")
                return "Value cannot be empty.";

            if (!(new Regex(@"^[a-zA-Z0-9_\-\.]")).IsMatch(val))
                return "Only letters, numbers, underscores, dashes, and periods are acceptable characters.";

            return "";
        };

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lblTimer.Text == "0 seconds")
                return;

            string text = string.Empty;
            string value = string.Empty;
            if (InputBox.Show("What label do you want to add to the time?", "Label:", ref value, validation) == DialogResult.OK)
                text = string.Format("{0}: {1}", value, lblTimer.Text);
            else if (InputBox.Show("What label do you want to add to the time?", "Label:", ref value, validation) == DialogResult.Cancel)
                return;

            cblTimes.Items.Add(text);

            stopWatch.Reset();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int hours = 0, minutes = 0, seconds = 0;
            string projectName = string.Empty;

            for (int i = 0; i < cblTimes.Items.Count; i++)
            {
                if (cblTimes.GetItemCheckState(i) == CheckState.Checked)
                {
                    //string[] times = cblTimes.Items[i].ToString().Split(':');

                    char[] delims = { ':', ' ' };
                    string[] times = cblTimes.Items[i].ToString().Split(delims);

                    for (int j = 3; j < times.Length; j++)
                    {
                        switch (times[j])
                        {
                            case "hours":
                                hours += int.Parse(times[j-1]);
                                break;
                            case "minutes":
                                minutes += int.Parse(times[j-1]);
                                break;
                            case "seconds":
                                seconds += int.Parse(times[j-1]);
                                projectName = times[0];
                                break;
                        }
                    }
                }
            }

            TimeSpan timeSpan = new TimeSpan(hours, minutes, seconds);
            MessageBox.Show(string.Format("You spent {0} hours, {1} minutes, and {2} seconds on Project {3}",
                                          timeSpan.Hours,
                                          timeSpan.Minutes,
                                          timeSpan.Seconds,
                                          projectName), "Accumulated Time", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
