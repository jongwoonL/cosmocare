using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using teamProject;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace teamProject
{
    public partial class Form1 : Form
    {
        public static int digit = 3;
        private List<string> conditions = new List<string>();

        Form7 form7 = new Form7();

        /*        // 조건 초기화
                public void resetCon()
                {
                    Utils.reScreen(dataGridView1, dataGridView2, digit);
                    MessageBox.Show("조건이 초기화되었습니다.");
                }
        */

        public Form1()
        {
            InitializeComponent();
            CenterToScreen(); // 폼을 화면의 정중앙에 배치합니다.

            /* 너무 느려져서 기각
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // 열 너비 맞춤
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // 가운데 정렬
            */
            progressBar1.Style = ProgressBarStyle.Marquee; // Marquee 스타일은 애니메이션 형태의 로딩바입니다.
            progressBar1.MarqueeAnimationSpeed = 30; // 로딩바의 애니메이션 속도를 조절합니다.

            button20.Text = "←.0\n.00";
            button30.Text = ".00\n→.0";

            this.dataGridView1.ForeColor = Color.Black;
            this.dataGridView2.ForeColor = Color.Black;

            ShowForm7AsChildForm();

            Utils.reScreen(dataGridView1, dataGridView2, digit, progressBar1);

            //button1.Location = new Point(form7.getLocationX() + 12, form7.getLocationY() + 20);
            menuStrip1.Focus();
        }

        private void ShowForm7AsChildForm()
        {
            form7.TopLevel = false;
            form7.FormBorderStyle = FormBorderStyle.None;
            form7.Dock = DockStyle.Fill;

            panel1.Controls.Add(form7);
            form7.submitButton().Click += button1_Click;
            form7.setDataType();
            form7.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("test");
            if (form7.conditions.Count == 0)
            {
                Utils.reScreen(dataGridView1, dataGridView2, digit, progressBar1);
            }
            else
            {
                form7.finalQueryGen();
                if (form7.getCurrentTab().Equals("PData"))
                {
                    Utils.reScreen(dataGridView1, form7.getCurrentTab(), string.Join(" ", form7.conditions), digit, progressBar1);
                }
                else if (form7.getCurrentTab().Equals("QData"))
                {
                    Utils.reScreen(dataGridView2, form7.getCurrentTab(), string.Join(" ", form7.conditions), digit, progressBar1);
                }
            }
        }


        /*
                // 셀 선택 시 할당되는 값
                string select = "";

                // 셀 선택 시 select에 datetime(PK) 할당
                private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
                {
                    PData  data = dataGridView1.CurrentRow.DataBoundItem as PData;
                    select = data.datetime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    label1.Text = "현재 선택 : " + select + " 기록 데이터";
                }
        */
        /*
                // 선택 데이터 삭제
                private void button2_Click(object sender, EventArgs e)
                {
                    // select와 동일한 datetime을 갖는 PData 객체 찾기
                    PData data = DataManager.datas.SingleOrDefault(x => x.datetime.ToString("yyyy-MM-dd HH:mm:ss.fff") == select);
                    if (data != null)
                    {
                        DataManager.Delete(data);
                        label1.Text = "현재 선택 : ";
                        MessageBox.Show($"{select} 데이터가 삭제 되었습니다.");
                        select = "";
                        button1_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("해당하는 데이터가 없습니다.");
                    }
                }
        */
        /*
                // 테스트 데이터 n개 입력
                private void button3_Click(object sender, EventArgs e)
                {
                    int count = 1;
                    if (!textBox4.Text.Trim().Equals(""))
                    {
                        count = int.Parse(textBox4.Text);
                    }

                    // 확인 대화 상자 표시
                    DialogResult result = MessageBox.Show($"테스트 데이터를 {count}개 생성하시겠습니까?\n(이 작업은 수 초 내지 수 분이 소요될 수 있습니다.)", 
                        "생성", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                    // 생성을 선택한 경우
                    if (result == DialogResult.OK)
                    {
                        // 테스트 데이터 생성 및 저장
                        for (int i = 0; i < count; i++)
                        {
                            PData random = makeRandom();
                            Thread.Sleep(100);
                            DataManager.Save(random);
                        }

                        // 데이터 그리드뷰 다시 불러오기
                        reScreen();

                        // 사용자에게 완료 메시지 표시
                        textBox4.Text = "";
                        MessageBox.Show($"테스트 데이터가 {count}개 생성 되었습니다.");
                    }
                    else // 취소를 선택한 경우
                    {
                        MessageBox.Show("테스트 데이터 생성이 취소 되었습니다.");
                    }
                }
        */

        // 공정 데이터 관리
        private void ToolStrip1_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
            Utils.reScreen(dataGridView1, "PData", digit, progressBar1);
        }

        // QC 데이터 관리
        private void ToolStrip2_Click(object sender, EventArgs e)
        {
            new Form3().ShowDialog();
            Utils.reScreen(dataGridView2, "QData", digit, progressBar1);
        }


        // 공정 데이터 차트
        private void ToolStrip3_Click(object sender, EventArgs e)
        {
            new Form4().ShowDialog();
        }

        // QC 데이터 차트
        private void ToolStrip4_Click(object sender, EventArgs e)
        {
            new Form5().ShowDialog();
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
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // 유효한 셀이 클릭되었는지 확인합니다.
            {
                DataGridViewCell clickedCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                // 클릭된 셀의 정보를 가져옵니다.
                string value = clickedCell.Value.ToString(); // 셀의 값
                int rowIdx = e.RowIndex; // 행 인덱스
                int colIdx = e.ColumnIndex; // 열 인덱스 

                /*
                // 가져온 정보를 사용하여 원하는 작업을 수행합니다.
                MessageBox.Show($"클릭된 셀: 행 {rowIdx}, 열 {colIdx}, 값 {value}");
                */

                textBox3.Text = value;
            }
        }

        // 셀 클릭 GridView2
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // 유효한 셀이 클릭되었는지 확인합니다.
            {
                DataGridViewCell clickedCell = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];
                // 클릭된 셀의 정보를 가져옵니다.
                string value = clickedCell.Value.ToString(); // 셀의 값
                int rowIdx = e.RowIndex; // 행 인덱스
                int colIdx = e.ColumnIndex; // 열 인덱스

                /*
                // 가져온 정보를 사용하여 원하는 작업을 수행합니다.
                MessageBox.Show($"클릭된 셀: 행 {rowIdx}, 열 {colIdx}, 값 {value}");
                */

                textBox3.Text = value;
            }
        }

        // Form1 종료 시 Form0도 함께 종료
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Form0를 종료합니다. 
            Application.OpenForms["Form0"].Close();
        }
    }
}