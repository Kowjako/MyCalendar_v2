using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCalendar
{
    public partial class Form1 : Form
    {
        private List<FlowLayoutPanel> listFlDay = new List<FlowLayoutPanel>();
        private DateTime currentDay = DateTime.Today;
        private DateTime currentData = DateTime.Today;
        public Form1()
        {
            InitializeComponent();
            GeneratePanel();
            DisplayCurrentDate();
            refreshTime_Tick(this,null);
        }
        private Point lastpoint;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastpoint = new Point(e.X, e.Y);
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastpoint.X;
                this.Top += e.Y - lastpoint.Y;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void GeneratePanel()
        {
            flDays.Controls.Clear();
            listFlDay.Clear();
            for (int i = 0; i < 42; i++)
            {
                FlowLayoutPanel tmp = new FlowLayoutPanel();
                tmp.Name = $"flDay{i}";
                tmp.Size = new Size(117, 84);
                tmp.BackColor = Color.White;
                tmp.BorderStyle = BorderStyle.FixedSingle;
                flDays.Controls.Add(tmp);
                listFlDay.Add(tmp);
            }
        }
        private void addDayLabel(int startday, int countdaysinmonth)
        {
            foreach (FlowLayoutPanel tmp in listFlDay)
            {
                tmp.Controls.Clear();
                tmp.BackColor = Color.White;
            }
            for (int i = 1; i <= countdaysinmonth; i++)
            {

                Label tmp = new Label();
                tmp.Name = $"lblDay{i}";
                tmp.AutoSize = false;
                tmp.TextAlign = ContentAlignment.MiddleRight;
                tmp.Size = new Size(114, 23);
                tmp.Text = i.ToString();
                tmp.ForeColor = Color.Black;
                tmp.Font = new Font("Tahoma", 9, FontStyle.Bold);
                if (i == currentData.Day && currentDay == currentData)
                {
                    listFlDay[(i - 1) + (startday - 1)].BackColor = Color.MistyRose;
                    listFlDay[(i - 1) + (startday - 1)].Controls.Add(tmp);
                }
                else
                    listFlDay[(i - 1) + (startday - 1)].Controls.Add(tmp); 
            }
            for (int i = 0; i < startday - 1; i++)
            {
                Label tmp = new Label();
                tmp.Name = $"lblDay{i}";
                tmp.AutoSize = false;
                tmp.TextAlign = ContentAlignment.MiddleRight;
                tmp.Size = new Size(114, 23);
                tmp.Text = (GetNumberOfPreviousDays() - startday + 2 + i).ToString();
                tmp.ForeColor = Color.Gray;
                tmp.Font = new Font("Tahoma", 9);
                listFlDay[i].Controls.Add(tmp);
            }
            int buff = 1;
            for (int i = startday+countdaysinmonth - 1; i < 42; i++)
            {
                Label tmp = new Label();
                tmp.Name = $"lblDay{i}";
                tmp.AutoSize = false;
                tmp.TextAlign = ContentAlignment.MiddleRight;
                tmp.Size = new Size(114, 23);
                tmp.Text = buff.ToString();
                tmp.ForeColor = Color.Gray;
                tmp.Font = new Font("Tahoma", 9);
                listFlDay[i].Controls.Add(tmp);
                buff += 1;
            }
        }
        private void DisplayCurrentDate()
        {
            label9.Text = currentDay.ToString("MMMM, yyyy");
            addDayLabel(GetFirstDay(), GetNumberOfdays());
        }
        private void PreviousMonth()
        {
            currentDay = currentDay.AddMonths(-1);
            DisplayCurrentDate();
        }
        private void NextMonth()
        {
            currentDay = currentDay.AddMonths(1);
            DisplayCurrentDate();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            PreviousMonth();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            NextMonth();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentDay = DateTime.Today;
            DisplayCurrentDate();
        }
        private int GetFirstDay()
        {
            DateTime firstDayOfMonth = new DateTime(currentDay.Year, currentDay.Month, 1);
            return (int)firstDayOfMonth.DayOfWeek + 1;
        }
        private int GetNumberOfdays()
        {
            DateTime firstDayOfCurrentDate = new DateTime(currentDay.Year, currentDay.Month, 1);
            return firstDayOfCurrentDate.AddMonths(1).AddDays(-1).Day;
        }
        private int GetNumberOfPreviousDays()
        {
            DateTime firstDayOfCurrentDate = new DateTime(currentDay.Year, currentDay.Month, 1);
            return firstDayOfCurrentDate.AddMonths(0).AddDays(-1).Day;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CreateEvent form = new CreateEvent();
            if(form.ShowDialog()==DialogResult.OK)
            {
                listFlDay[form.day+2].Controls.Add(form.eventtest);
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Timer refreshTime = new Timer();
            refreshTime.Interval = (30 * 1000);
            refreshTime.Tick += new EventHandler(refreshTime_Tick);
            refreshTime.Start();
        }
        private void refreshTime_Tick(object sender, EventArgs e)
        {
            timelbl.Text = DateTime.Now.ToShortTimeString();
        }
    }
}
