

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace teamProject
{
    public partial class Main : Form
    {
        public static int digit = 3;
        private List<string> conditions = new List<string>();

        ConditionalSearch gnb = new ConditionalSearch();

        public Main()
        {
            InitializeComponent();
            CenterToScreen(); // 폼을 화면의 정중앙에 배치합니다.

            progressBar1.Style = ProgressBarStyle.Marquee; // Marquee 스타일은 애니메이션 형태의 로딩바입니다.
            progressBar1.MarqueeAnimationSpeed = 30; // 로딩바의 애니메이션 속도를 조절합니다.

            button20.Text = "←.0\n.00";
            button30.Text = ".00\n→.0";

            this.dataGridView1.ForeColor = Color.Black;
            this.dataGridView2.ForeColor = Color.Black;

            ShowgnbAsChildForm();

            Utils.reScreen(dataGridView1, dataGridView2, digit, progressBar1);
            menuStrip1.Focus();
        }

        private void ShowgnbAsChildForm()
        {
            gnb.TopLevel = false;
            gnb.FormBorderStyle = FormBorderStyle.None;
            gnb.Dock = DockStyle.Fill;

            panel1.Controls.Add(gnb);
            gnb.submitButton().Click += button_Click;
            gnb.setDataType();
            gnb.Show();
        }

        private void button_Click(object sender, EventArgs e)
        {
            Console.WriteLine("test");
            if (gnb.conditions.Count == 0)
            {
                Utils.reScreen(dataGridView1, dataGridView2, digit, progressBar1);
            }
            else
            {
                gnb.finalQueryGen();
                if (gnb.getCurrentTab().Equals("PData"))
                {
                    Utils.reScreen(dataGridView1, gnb.getCurrentTab(), string.Join(" ", gnb.conditions), digit, progressBar1);
                }
                else if (gnb.getCurrentTab().Equals("QData"))
                {
                    Utils.reScreen(dataGridView2, gnb.getCurrentTab(), string.Join(" ", gnb.conditions), digit, progressBar1);
                }
            }
        }

        // 공정 데이터 관리
        private void ToolStrip1_Click(object sender, EventArgs e)
        {
            new PDataTable().ShowDialog();
            Utils.reScreen(dataGridView1, "PData", digit, progressBar1);
        }

        // QC 데이터 관리
        private void ToolStrip2_Click(object sender, EventArgs e)
        {
            new QDataTable().ShowDialog();
            Utils.reScreen(dataGridView2, "QData", digit, progressBar1);
        }


        // 공정 데이터 차트
        private void ToolStrip3_Click(object sender, EventArgs e)
        {
            new PDataChart().ShowDialog();
        }

        // QC 데이터 차트
        private void ToolStrip4_Click(object sender, EventArgs e)
        {
            new QDataChart().ShowDialog();
        }

        // 메인
        private void ToolStrip0_Click(object sender, EventArgs e)
        {
            Utils.reScreen(dataGridView1, dataGridView2, digit, progressBar1);
        }

        // 자릿수 줄이기(최소 1자리 까지)
        private void button20_Click(object sender, EventArgs e)
        {
            if (digit > 1)
            {
                digit--;
                Utils.reScreen(dataGridView1, dataGridView2, digit, progressBar1);
            }
            else
            {
                MessageBox.Show("더 이상 줄일 수 없습니다.");
            }
        }

        // 자릿수 늘리기(최대 9자리 까지)
        private void button30_Click(object sender, EventArgs e)
        {
            if (digit < 9)
            {
                digit++;
                Utils.reScreen(dataGridView1, dataGridView2, digit, progressBar1);
            }
            else
            {
                MessageBox.Show("더 이상 늘릴 수 없습니다.");
            }
        }

        // 셀 클릭 GridView1
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // 유효한 셀이 클릭되었는지 확인
            {
                DataGridViewCell clickedCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                // 클릭된 셀의 정보를 가져옴/
                string value = clickedCell.Value.ToString(); // 셀의 값
                int rowIdx = e.RowIndex; // 행 인덱스
                int colIdx = e.ColumnIndex; // 열 인덱스 

                textBox3.Text = value;
            }
        }

        // 셀 클릭 GridView2
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // 유효한 셀이 클릭되었는지 확인
            {
                DataGridViewCell clickedCell = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];
                // 클릭된 셀의 정보 가져옴
                string value = clickedCell.Value.ToString(); // 셀의 값
                int rowIdx = e.RowIndex; // 행 인덱스
                int colIdx = e.ColumnIndex; // 열 인덱스

                textBox3.Text = value;
            }
        }

        // Main 종료 시 FirstScreen도 함께 종료
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // FirstScreen를 종료합니다. 
            Application.OpenForms["FirstScreen"].Close();
        }
    }
}