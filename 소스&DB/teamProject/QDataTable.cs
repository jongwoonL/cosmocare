using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using teamProject;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace teamProject
{
    public partial class QDataTable : Form
    {
        List<string> errors = new List<string>();
        string placeholderText = "ex. 2024-01-01";
        ConditionalSearch gnb = new ConditionalSearch();
        public QDataTable()
        {
            InitializeComponent();
            CenterToScreen(); // 폼을 화면의 정중앙에 배치합니다.

            ShowgnbAsChildForm();

            this.dataGridView1.ForeColor = Color.Black;
            placeholder.Text = placeholderText;
            placeholder.Enabled = false;

            progressBar1.Style = ProgressBarStyle.Marquee; // Marquee 스타일은 애니메이션 형태의 로딩바입니다.
            progressBar1.MarqueeAnimationSpeed = 30; // 로딩바의 애니메이션 속도를 조절합니다.

            Utils.reScreen(dataGridView1, "QData", Main.digit, progressBar1);

        }
        private void ShowgnbAsChildForm()
        {
            gnb.TopLevel = false;
            gnb.FormBorderStyle = FormBorderStyle.None;
            gnb.Dock = DockStyle.Fill;

            tableLayoutPanel1.Controls.Add(gnb, 0, 0);
            gnb.submitButton().Click += submit_Click;
            gnb.setDataType("QData");
            gnb.Show();
        }

        private void submit_Click(object sender, EventArgs e)
        {
            gnb.finalQueryGen();

            if (gnb.conditions.Count == 0)
            {
                Utils.reScreen(dataGridView1, "QData", Main.digit, progressBar1);
            }
            else
            {
                Utils.reScreen(dataGridView1, "QData", string.Join(" ", gnb.conditions), Main.digit, progressBar1);
            }
        }

        // 글자 입력되면 플레이스홀더 제거
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                placeholder.Visible = false;
            }
            else
            {
                placeholder.Visible = true;
            }
        }

        string select;

        // 셀 선택 시 textBox에 선택값 할당
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            QData data = dataGridView1.CurrentRow.DataBoundItem as QData;
            //select = data.date.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            select = data.date.ToString("yyyy-MM-dd");
            //textBox1.Text = data.date.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            textBox1.Text = data.date.ToString("yyyy-MM-dd");
            textBox2.Text = data.weight.ToString();
            textBox3.Text = data.water.ToString();
            textBox4.Text = data.material.ToString();
            textBox5.Text = data.HSO.ToString();
            textBox6.Text = data.pH.ToString();
        }

        // 데이터 유효성 검사와 데이터 처리
        private QData ValidateAndCreateDataObject()
        {
            QData data = new QData();
            data.date = DateTime.Now;
            double tempValue;

            if (!DateTime.TryParse(textBox1.Text, out _))
                errors.Add("date");
            if (!double.TryParse(textBox2.Text, out tempValue))
                errors.Add("weight");
            if (!double.TryParse(textBox3.Text, out tempValue))
                errors.Add("water");
            if (!double.TryParse(textBox4.Text, out tempValue))
                errors.Add("material");
            if (!double.TryParse(textBox5.Text, out tempValue))
                errors.Add("HSO");
            if (!double.TryParse(textBox6.Text, out tempValue))
                errors.Add("pH");

            if (errors.Count > 0)
            {
                MessageBox.Show($"{string.Join(", ", errors.ToArray())}에 알맞은 데이터 값을 입력하세요");
                errors.Clear();
                return null;
            }
            else
            {
                data.date = DateTime.Parse(textBox1.Text);
                data.weight = double.Parse(textBox2.Text);
                data.water = double.Parse(textBox3.Text);
                data.material = double.Parse(textBox4.Text);
                data.HSO = double.Parse(textBox5.Text);
                data.pH = double.Parse(textBox6.Text);
            }
            return data;
        }

        // 데이터 추가
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                QData data = ValidateAndCreateDataObject();
                if (data != null)
                {
                    if (DataManager.datasQ.Any(d => d.date.Date == data.date.Date))
                    {
                        MessageBox.Show("이미 동일한 날짜에 데이터가 입력되었습니다.");
                        return;
                    }

                    DataManager.Save(data);
                    MessageBox.Show($"{data.date.ToString("yyyy-MM-dd")} 데이터가 추가 되었습니다.");
                    Utils.reScreen(dataGridView1, "QData", Main.digit, progressBar1);
                }
            }
            catch
            {
                MessageBox.Show("이미 동일한 날짜에 데이터가 있습니다.");
            }
        }

        // 데이터 수정
        private void button2_Click(object sender, EventArgs e)
        {
            QData data = ValidateAndCreateDataObject();
            if (data != null)
            {
                // 수정을 위해 select 값을 사용할 수 있어야 합니다.
                if (!string.IsNullOrEmpty(select))
                {
                    DataManager.Update(data, select);
                    MessageBox.Show($"{select} 데이터가 수정 되었습니다.");
                    Utils.reScreen(dataGridView1, "QData", Main.digit, progressBar1);
                }
                else
                {
                    MessageBox.Show("수정할 데이터를 선택하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /*
        // 데이터 추가
        private void button1_Click(object sender, EventArgs e)
        {
            QData data = new QData();
            data.date = DateTime.Parse(textBox1.Text);
            data.weight = double.Parse(textBox2.Text);
            data.water = double.Parse(textBox3.Text);
            data.material = double.Parse(textBox4.Text);
            data.HSO = double.Parse(textBox5.Text);
            data.pH = double.Parse(textBox6.Text);

            DataManager.Save(data);
            MessageBox.Show($"{data.date.ToString("yyyy-MM-dd")} 데이터가 추가 되었습니다.");
            Utils.reScreen(dataGridView1, "QData", main.digit);
        }

        // 데이터 수정
        private void button2_Click(object sender, EventArgs e)
        {
            QData data = new QData();
            data.weight = double.Parse(textBox2.Text);
            data.water = double.Parse(textBox3.Text);
            data.material = double.Parse(textBox4.Text);
            data.HSO = double.Parse(textBox5.Text);
            data.pH = double.Parse(textBox6.Text);

            DataManager.Update(data, select);
            MessageBox.Show($"{select} 데이터가 수정 되었습니다.");
            Utils.reScreen(dataGridView1, "QData", main.digit);
        }
        */

        // 선택 데이터 삭제
        private void button3_Click(object sender, EventArgs e)
        {
            // textBox1.Text와 동일한 datetime을 갖는 PData 객체 찾기
            //QData data = DataManager.datasQ.SingleOrDefault(x => x.date.ToString("yyyy-MM-dd HH:mm:ss.fffffff") == textBox1.Text);
            QData data = DataManager.datasQ.SingleOrDefault(x => x.date.ToString("yyyy-MM-dd") == textBox1.Text);
            if (data != null)
            {
                DataManager.Delete(data);
                MessageBox.Show($"{textBox1.Text} 데이터가 삭제 되었습니다.");
                Utils.reScreen(dataGridView1, "QData", Main.digit, progressBar1);
            }
            else
            {
                MessageBox.Show("해당하는 데이터가 없습니다.");
            }
        }


        // 텍스트 박스 초기화
        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }
    }
}