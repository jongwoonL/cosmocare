using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.Common;
using System.Globalization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using teamProject;

namespace teamProject
{
    public partial class Form5 : Form
    {
        Form7 form7 = new Form7();
        List<Chart> charts = new List<Chart>();
        public enum QDataFields
        {
            date,
            weight,
            water,
            material,
            HSO,
            pH
        }

        public Form5()
        {
            InitializeComponent();
            CenterToScreen(); // 폼을 화면의 정중앙에 배치합니다.

            ShowForm7AsChildForm();

            DataManager.LoadQ();
            DataManager.LoadP();
            // datetime between '2022-04-02' and '2022-04-09'
            //loadCharts();
            //DrawChart(chart1, "ReactA_Temp");
            for (int i = 0; i < Utils.qdata.Count(); i++)
            {
                charts.Add(new Chart());
            }
            //button1.Location = new Point(form7.getLocationX() +12 , form7.getLocationY() +20);
            DrawCharts(charts);

        }

        private void ShowForm7AsChildForm()
        {
            form7.TopLevel = false;
            form7.FormBorderStyle = FormBorderStyle.None;
            form7.Dock = DockStyle.Fill;

            tableLayoutPanel1.Controls.Add(form7, 0, 0);
            form7.submitButton().Click += button1_Click;
            form7.setDataType("QData");
            form7.Show();
            //groupBox2.Size = new Size(form7.getGroupBox().Width, form7.Height);
        }

        private void loadCharts()
        {
            //plotView1.Model = DrawGraph(QDataFields.weight.ToString());
            //plotView2.Model = DrawGraph(QDataFields.water.ToString());
            //plotView3.Model = DrawGraph(QDataFields.material.ToString());
            //plotView4.Model = DrawGraph(QDataFields.HSO.ToString());
            //plotView5.Model = DrawGraph(QDataFields.pH.ToString());
            DrawCharts(charts);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form7.finalQueryGen();
            //DataManager.LoadQ(string.Join(" ", form7.conditions));
            //loadCharts();

            if (form7.conditions.Count == 0)
            {
                DataManager.LoadQ();
            }
            else
            {
                DataManager.LoadQ(string.Join(" ", form7.conditions));
            }
            loadCharts();
        }

        private void DrawCharts(List<Chart> charts)
        {
            panel1.Controls.Clear();
            for (int i = 1; i < Utils.qdata.Count(); i++)
            {
                Chart chart = charts[i];
                chart.Series.Clear();
                chart.ChartAreas.Clear();
                chart.Legends.Clear();
                chart.Titles.Clear();
                ChartArea chartArea = new ChartArea();
                Legend legend = new Legend();
                System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();

                chart.Series.Add(Utils.qdata[i]);
                chart.ChartAreas.Add(chartArea);
                chart.Series[Utils.qdata[i]].ChartType = SeriesChartType.FastLine;

                chart.Name = "QData";
                legend.Name = "legend1";
                legend.Docking = Docking.Top;
                chartArea.Name = "ChartArea1";
                series.Name = Utils.qdata[i];

                series.ChartArea = chartArea.Name;
                series.Legend = "Legend1";

                int xSize = (panel1.Size.Width / 2) - 10;
                int ySize = 250;
                int marginTop = 0;
                chart.Size = new Size(xSize, ySize);
                chart.Location = new Point(((i - 1) % 2) * xSize, marginTop + (((i - 1) / 2) * ySize));


                chart.Legends.Add(legend);


                //chart.Series[0].Name = Utils.qdata[i];
                if (DataManager.datasQ.Count > 0)
                {
                    foreach (var data in DataManager.datasQ)
                    {
                        chart.Series[Utils.qdata[i]].Points.AddXY(data.date,
                                Convert.ToDouble(data.GetType().GetProperty(Utils.qdata[i]).GetValue(data)));
                    }
                }
                panel1.Controls.Add(chart);
                //chart.Show();
            }
        }
    }
}