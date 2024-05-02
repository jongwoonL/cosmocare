using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using teamProject;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace teamProject
{
    public partial class Form2 : Form
    {
        List<string> errors = new List<string>();
        Form7 form7 = new Form7();

        public Form2()
        {
            InitializeComponent();
            CenterToScreen(); // 폼을 화면의 정중앙에 배치합니다.

            ShowForm7AsChildForm();
            textBox1.Enabled = false;

            this.dataGridView1.ForeColor = Color.Black;

            progressBar1.Style = ProgressBarStyle.Marquee; // Marquee 스타일은 애니메이션 형태의 로딩바입니다.
            progressBar1.MarqueeAnimationSpeed = 30; // 로딩바의 애니메이션 속도를 조절합니다.
            Utils.reScreen(dataGridView1, "PData", Form1.digit, progressBar1);
        }

        private void ShowForm7AsChildForm()
        {
            form7.TopLevel = false;
            form7.FormBorderStyle = FormBorderStyle.None;
            form7.Dock = DockStyle.Fill;

            panel1.Controls.Add(form7);
            form7.submitButton().Click += submit_Click;
            form7.setDataType("PData");
            form7.Show();
        }

        private void submit_Click(object sender, EventArgs e)
        {
            form7.finalQueryGen();

            if (form7.conditions.Count == 0)
            {
                Utils.reScreen(dataGridView1, "PData", Form1.digit, progressBar1);
            }
            else
            {
                Utils.reScreen(dataGridView1, "PData", string.Join(" ", form7.conditions), Form1.digit, progressBar1);
            }
        }

        string select;

        // 셀 선택 시 textBox에 선택값 할당
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            PData data = dataGridView1.CurrentRow.DataBoundItem as PData;
            select = data.datetime.ToString("yyyy-MM-dd HH:mm:ss.fffffff"); // 수정에 사용
            textBox1.Text = data.datetime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            textBox2.Text = data.ReactA_Temp.ToString();
            textBox3.Text = data.ReactB_Temp.ToString();
            textBox4.Text = data.ReactC_Temp.ToString();
            textBox5.Text = data.ReactD_Temp.ToString();
            textBox6.Text = data.ReactE_Temp.ToString();
            textBox7.Text = data.ReactF_Temp.ToString();
            textBox8.Text = data.ReactF_PH.ToString();
            textBox9.Text = data.Power.ToString();
            textBox10.Text = data.CurrentA.ToString();
            textBox11.Text = data.CurrentB.ToString();
            textBox12.Text = data.CurrentC.ToString();
        }

        // 데이터 유효성 검사와 데이터 처리
        private PData ValidateAndCreateDataObject()
        {
            PData data = new PData();
            data.datetime = DateTime.Now;
            double tempValue;
            if (!double.TryParse(textBox2.Text, out tempValue))
                errors.Add("ReactA_Temp");
            if (!double.TryParse(textBox3.Text, out tempValue))
                errors.Add("ReactB_Temp");
            if (!double.TryParse(textBox4.Text, out tempValue))
                errors.Add("ReactC_Temp");
            if (!double.TryParse(textBox5.Text, out tempValue))
                errors.Add("ReactD_Temp");
            if (!double.TryParse(textBox6.Text, out tempValue))
                errors.Add("ReactE_Temp");
            if (!double.TryParse(textBox7.Text, out tempValue))
                errors.Add("ReactF_Temp");
            if (!double.TryParse(textBox8.Text, out tempValue))
                errors.Add("ReactF_PH");
            if (!double.TryParse(textBox9.Text, out tempValue))
                errors.Add("Power");
            if (!double.TryParse(textBox10.Text, out tempValue))
                errors.Add("CurrentA");
            if (!double.TryParse(textBox11.Text, out tempValue))
                errors.Add("CurrentB");
            if (!double.TryParse(textBox12.Text, out tempValue))
                errors.Add("CurrentC");

            if (errors.Count > 0)
            {
                MessageBox.Show($"{string.Join(", ", errors.ToArray())}에 알맞은 데이터 값을 입력하세요");
                errors.Clear();
                return null;
            }
            else
            {
                data.ReactA_Temp = double.Parse(textBox2.Text);
                data.ReactB_Temp = double.Parse(textBox3.Text);
                data.ReactC_Temp = double.Parse(textBox4.Text);
                data.ReactD_Temp = double.Parse(textBox5.Text);
                data.ReactE_Temp = double.Parse(textBox6.Text);
                data.ReactF_Temp = double.Parse(textBox7.Text);
                data.ReactF_PH = double.Parse(textBox8.Text);
                data.Power = double.Parse(textBox9.Text);
                data.CurrentA = double.Parse(textBox10.Text);
                data.CurrentB = double.Parse(textBox11.Text);
                data.CurrentC = double.Parse(textBox12.Text);
            }
            return data;
        }
        /*
        // 데이터 추가
        private void button1_Click(object sender, EventArgs e)
        {
            PData data = new PData();
            data.datetime = DateTime.Now;
            data.ReactA_Temp = double.Parse(textBox2.Text);
            data.ReactB_Temp = double.Parse(textBox3.Text);
            data.ReactC_Temp = double.Parse(textBox4.Text);
            data.ReactD_Temp = double.Parse(textBox5.Text);
            data.ReactE_Temp = double.Parse(textBox6.Text);
            data.ReactF_Temp = double.Parse(textBox7.Text);
            data.ReactF_PH = double.Parse(textBox8.Text);
            data.Power = double.Parse(textBox9.Text);
            data.CurrentA = double.Parse(textBox10.Text);
            data.CurrentB = double.Parse(textBox11.Text);
            data.CurrentC = double.Parse(textBox12.Text);

            DataManager.Save(data);
            MessageBox.Show($"{data.datetime.ToString("yyyy-MM-dd HH:mm:ss.fffffff")} 데이터가 추가 되었습니다.");
            Utils.reScreen(dataGridView1, "PData", Form1.digit);
        }

        // 데이터 수정
        private void button2_Click(object sender, EventArgs e)
        {
            PData data = new PData();

            data.ReactA_Temp = double.Parse(textBox2.Text);
            data.ReactB_Temp = double.Parse(textBox3.Text);
            data.ReactC_Temp = double.Parse(textBox4.Text);
            data.ReactD_Temp = double.Parse(textBox5.Text);
            data.ReactE_Temp = double.Parse(textBox6.Text);
            data.ReactF_Temp = double.Parse(textBox7.Text);
            data.ReactF_PH = double.Parse(textBox8.Text);
            data.Power = double.Parse(textBox9.Text);
            data.CurrentA = double.Parse(textBox10.Text);
            data.CurrentB = double.Parse(textBox11.Text);
            data.CurrentC = double.Parse(textBox12.Text);

            // 데이터베이스 업데이트
            DataManager.Update(data, select);
            MessageBox.Show($"{select} 데이터가 수정 되었습니다.");
            Utils.reScreen(dataGridView1, "PData", Form1.digit);
        }
        */


        // 데이터 추가
        private void button1_Click(object sender, EventArgs e)
        {
            PData data = ValidateAndCreateDataObject();
            if (data != null)
            {
                DataManager.Save(data);
                MessageBox.Show($"{data.datetime.ToString("yyyy-MM-dd HH:mm:ss.fffffff")} 데이터가 추가 되었습니다.");
                Utils.reScreen(dataGridView1, "PData", Form1.digit, progressBar1);
            }
        }

        // 데이터 수정
        private void button2_Click(object sender, EventArgs e)
        {
            PData data = ValidateAndCreateDataObject();
            if (data != null)
            {
                // 수정을 위해 select 값을 사용할 수 있어야 합니다.
                if (!string.IsNullOrEmpty(select))
                {
                    DataManager.Update(data, select);
                    MessageBox.Show($"{select} 데이터가 수정 되었습니다.");
                    Utils.reScreen(dataGridView1, "PData", Form1.digit, progressBar1);
                }
                else
                {
                    MessageBox.Show("수정할 데이터를 선택하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // 선택 데이터 삭제
        private void button3_Click(object sender, EventArgs e)
        {
            // textBox1.Text와 동일한 datetime을 갖는 PData 객체 찾기
            PData data = DataManager.datasP.SingleOrDefault(x => x.datetime.ToString("yyyy-MM-dd HH:mm:ss.fffffff") == textBox1.Text);
            if (data != null)
            {
                DataManager.Delete(data);
                MessageBox.Show($"{textBox1.Text} 데이터가 삭제 되었습니다.");
                Utils.reScreen(dataGridView1, "PData", Form1.digit, progressBar1);
            }
            else
            {
                MessageBox.Show("해당하는 데이터가 없습니다.");
            }
        }

        // 랜덤 데이터 생성
        public PData makeRandom()
        {
            Random rn = new Random();

            PData data = new PData();
            data.datetime = DateTime.Now;
            data.ReactA_Temp = rn.NextDouble() * (21.0 - 19.0) + 19.0;
            data.ReactB_Temp = rn.NextDouble() * (21.0 - 19.0) + 19.0;
            data.ReactC_Temp = rn.NextDouble() * (21.0 - 19.0) + 19.0;
            data.ReactD_Temp = rn.NextDouble() * (21.0 - 19.0) + 19.0;
            data.ReactE_Temp = rn.NextDouble() * (21.0 - 19.0) + 19.0;
            data.ReactF_Temp = rn.NextDouble() * (10.0 - 5.0) + 5.0;
            data.ReactF_PH = rn.NextDouble() * (2.0 - 1.0) + 1.0;
            data.Power = rn.NextDouble() * (1400.0 - 1300.0) + 1300.0;
            data.CurrentA = rn.NextDouble() * (0.4 - 0.2) + 0.2;
            data.CurrentB = rn.NextDouble() * (1.8 - 1.6) + 1.6;
            data.CurrentC = rn.NextDouble() * (1.4 - 1.2) + 1.2;

            return data;
        }

        // 테스트 데이터 n개 입력
        private void button4_Click(object sender, EventArgs e)
        {
            int count = 1;
            if (!textBox13.Text.Trim().Equals(""))
            {
                count = int.Parse(textBox13.Text);
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
                Utils.reScreen(dataGridView1, "PData", Form1.digit, progressBar1);

                // 사용자에게 완료 메시지 표시
                textBox13.Text = "";
                MessageBox.Show($"테스트 데이터가 {count}개 생성 되었습니다.");
            }
            else // 취소를 선택한 경우
            {
                MessageBox.Show("테스트 데이터 생성이 취소 되었습니다.");
            }
        }

        // 텍스트 박스 초기화
        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
        }
    }
}