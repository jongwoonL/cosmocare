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
using System.Runtime.InteropServices;
using teamProject;

namespace teamProject
{
    public partial class Form4 : Form
    {
        Form7 form7 = new Form7();
        List<Chart> charts = new List<Chart>();
        public enum PDataFields
        {
            datetime,
            ReactA_Temp,
            ReactB_Temp,
            ReactC_Temp,
            ReactD_Temp,
            ReactE_Temp,
            ReactF_Temp,
            ReactF_PH,
            Power,
            CurrentA,
            CurrentB,
            CurrentC
        }

        public Form4()
        {
            InitializeComponent();
            CenterToScreen(); // 폼을 화면의 정중앙에 배치합니다.

            ShowForm7AsChildForm();
            DataManager.LoadP();
            for (int i = 0; i < Utils.pdata.Count(); i++)
            {
                charts.Add(new Chart());
            }
            //button1.Location = new Point(form7.getLocationX() + 12, form7.getLocationY() + 20);
            DrawCharts(charts);
        }
        private void ShowForm7AsChildForm()
        {
            form7.TopLevel = false;
            form7.FormBorderStyle = FormBorderStyle.None;
            form7.Dock = DockStyle.Fill;

            tableLayoutPanel1.Controls.Add(form7, 0, 0);
            form7.submitButton().Click += button1_Click;
            form7.setDataType("PData");
            form7.Show();
        }
        private void loadCharts()
        {
            DrawCharts(charts);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form7.finalQueryGen();
            //DataManager.LoadQ(string.Join(" ", form7.conditions));
            //loadCharts();

            if (form7.conditions.Count == 0)
            {
                DataManager.LoadP();
            }
            else
            {
                DataManager.LoadP(string.Join(" ", form7.conditions));
            }
            loadCharts();
        }

        private void DrawCharts(List<Chart> charts)
        {
            panel1.Controls.Clear();
            for (int i = 1; i < Utils.pdata.Count(); i++)
            {
                Chart chart = charts[i];
                chart.Series.Clear();
                chart.ChartAreas.Clear();
                chart.Legends.Clear();
                chart.Titles.Clear();
                ChartArea chartArea = new ChartArea();
                Legend legend = new Legend();
                System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();

                chart.Series.Add(Utils.pdata[i]);
                chart.ChartAreas.Add(chartArea);
                chart.Series[Utils.pdata[i]].ChartType = SeriesChartType.FastLine;

                chart.Name = "PData";
                legend.Name = "legend1";
                legend.Docking = Docking.Top;
                chartArea.Name = "ChartArea1";
                series.Name = Utils.pdata[i];

                series.ChartArea = chartArea.Name;
                series.Legend = "Legend1";

                int xSize = (panel1.Size.Width / 2) - 10;
                int ySize = 200;
                int marginTop = 0;
                chart.Size = new Size(xSize, ySize);
                chart.Location = new Point(((i - 1) % 2) * xSize, marginTop + (((i - 1) / 2) * ySize));

                chart.Legends.Add(legend);


                //chart.Series[0].Name = Utils.pdata[i];
                if (DataManager.datasP.Count > 0)
                {
                    foreach (var data in DataManager.datasP)
                    {
                        chart.Series[Utils.pdata[i]].Points.AddXY(data.datetime,
                                Convert.ToDouble(data.GetType().GetProperty(Utils.pdata[i]).GetValue(data)));
                    }
                }
                panel1.Controls.Add(chart);
            }
        }

        //private void DrawChartsOnTable(List<Chart> charts)
        //{
        //    panel1.Controls.Clear();
        //    for (int i = 1; i < Utils.pdata.Count(); i++)
        //    {
        //        Chart chart = charts[i];
        //        chart.Series.Clear();
        //        chart.ChartAreas.Clear();
        //        chart.Legends.Clear();
        //        chart.Titles.Clear();
        //        ChartArea chartArea = new ChartArea();
        //        Legend legend = new Legend();
        //        System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();

        //        chart.Series.Add(Utils.pdata[i]);
        //        chart.ChartAreas.Add(chartArea);
        //        chart.Series[Utils.pdata[i]].ChartType = SeriesChartType.FastLine;

        //        chart.Name = "PData";
        //        legend.Name = "legend1";
        //        legend.Docking = Docking.Top;
        //        chartArea.Name = "ChartArea1";
        //        series.Name = Utils.pdata[i];

        //        series.ChartArea = chartArea.Name;
        //        series.Legend = "Legend1";

        //        int xSize = (panel1.Size.Width / 2) - 10;
        //        int ySize = 200;
        //        int marginTop = 0;
        //        chart.Size = new Size(xSize, ySize);
        //        chart.Location = new Point(((i - 1) % 2) * xSize, marginTop + (((i - 1) / 2) * ySize));


        //        chart.Legends.Add(legend);


        //        //chart.Series[0].Name = Utils.pdata[i];
        //        if (DataManager.datasP.Count > 0)
        //        {
        //            foreach (var data in DataManager.datasP)
        //            {
        //                chart.Series[Utils.pdata[i]].Points.AddXY(data.datetime,
        //                        Convert.ToDouble(data.GetType().GetProperty(Utils.pdata[i]).GetValue(data)));
        //            }
        //        }
        //        panel1.Controls.Add(chart, 0, 1);
        //    }
        //}
    }
}