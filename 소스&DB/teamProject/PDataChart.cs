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
    public partial class PDataChart : Form
    {
        ConditionalSearch gnb = new ConditionalSearch();
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

        public PDataChart()
        {
            InitializeComponent();
            CenterToScreen(); // 폼을 화면의 정중앙에 배치합니다.

            ShowgnbAsChildForm();
            DataManager.LoadP();
            for (int i = 0; i < Utils.pdata.Count(); i++)
            {
                charts.Add(new Chart());
            }
            //button1.Location = new Point(gnb.getLocationX() + 12, gnb.getLocationY() + 20);
            DrawCharts(charts);
        }
        private void ShowgnbAsChildForm()
        {
            gnb.TopLevel = false;
            gnb.FormBorderStyle = FormBorderStyle.None;
            gnb.Dock = DockStyle.Fill;

            tableLayoutPanel1.Controls.Add(gnb, 0, 0);
            gnb.submitButton().Click += button1_Click;
            gnb.setDataType("PData");
            gnb.Show();
        }
        private void loadCharts()
        {
            DrawCharts(charts);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gnb.finalQueryGen();
            //DataManager.LoadQ(string.Join(" ", gnb.conditions));
            //loadCharts();

            if (gnb.conditions.Count == 0)
            {
                DataManager.LoadP();
            }
            else
            {
                DataManager.LoadP(string.Join(" ", gnb.conditions));
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