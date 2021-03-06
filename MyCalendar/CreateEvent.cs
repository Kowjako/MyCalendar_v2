﻿using MySql.Data.MySqlClient;
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
    public partial class CreateEvent : Form
    {
        public Label eventtest;
        public CreateEvent()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
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
        public int day = 0, month = 0, year = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            if (parseName() == 0 || parseTime() == 0)
            {
                ExceptionForm form = new ExceptionForm();
                form.label3.Text = "Fill the lables correctly";
                form.Show();
            }
            else
            {
                day = dateTimePicker1.Value.Day;
                month = dateTimePicker1.Value.Month;
                year = dateTimePicker1.Value.Year;
                eventtest = new Label();
                eventtest.AutoSize = false;
                eventtest.Name = $"eventtest";
                eventtest.TextAlign = ContentAlignment.MiddleRight;
                eventtest.Size = new Size(115, 23);
                eventtest.Text = textBox1.Text;
                eventtest.ForeColor = Color.Black;
                eventtest.BackColor = color;
                eventtest.Font = new Font("Trebuchet MS", 10);
                eventtest.BorderStyle = BorderStyle.FixedSingle;
                eventtest.TextAlign = ContentAlignment.MiddleCenter;
                eventtest.Margin = new Padding(0);
                eventtest.Click += new EventHandler(onClick);
                this.DialogResult = DialogResult.OK;
                addEventToDataBase();
            }
        }
        private void addEventToDataBase()
        {
            Color tmpcolor = Color.FromArgb(color.R, color.G, color.B);
            DataBase db = new DataBase();
            MySqlCommand command = new MySqlCommand("INSERT INTO `eventdb` (`id`, `name`, `time`, `date`, `month`, `tip`, `color`) VALUES (NULL,@uName,@uTime,@uDate,@uMonth,@uTip,@uColor)", db.getConnection());
            command.Parameters.Add("@uName", MySqlDbType.VarChar).Value = textBox1.Text;
            command.Parameters.Add("@uTime", MySqlDbType.VarChar).Value = textBox2.Text;
            command.Parameters.Add("@uDate", MySqlDbType.VarChar).Value = String.Format($"{dateTimePicker1.Value.Day}");
            command.Parameters.Add("@uTip", MySqlDbType.VarChar).Value = richTextBox1.Text;
            command.Parameters.Add("@uMonth", MySqlDbType.VarChar).Value = dateTimePicker1.Value.ToString("MMMM");
            command.Parameters.Add("@uColor", MySqlDbType.VarChar).Value = tmpcolor.ToString();
            db.startConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                ExceptionForm form = new ExceptionForm();
                form.label3.Text = "Event was added sucessful";
                form.Show();
            }
            db.closeConnection();
        }
        private int parseName()
        {
            if (textBox1.Text == "") return 0;
            else return 1;
        }
        private int parseTime()
        {
            int hour = 0, minute = 0;
            try
            {
                hour = Convert.ToInt32((textBox2.Text).Substring(0, 2));
                minute = Convert.ToInt32((textBox2.Text).Substring(3, 2));
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        private void onClick(object sender, EventArgs e)
        {
            InfoForm form = new InfoForm();
            form.label6.Text = textBox1.Text;
            form.label7.Text = String.Format($"{dateTimePicker1.Value.Day}, {dateTimePicker1.Value.ToString("MMMM")}");
            form.label8.Text = textBox2.Text;
            form.label9.Text = richTextBox1.Text;
            if (form.ShowDialog() == DialogResult.OK)
            {
                
            }
        }
        private Color color = Color.Aqua;
        private void button3_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog()==DialogResult.OK)
            {
                color = colorDialog1.Color;
            }
        }
    }
}
