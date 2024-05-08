using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace teamProject
{
    public partial class ConditionalSearch : Form
    {
        public List<string> conditions = new List<string>();
        public ListBox listBox1 = new ListBox();
        public TabControl dataTypeTab = new TabControl();

        public ConditionalSearch()
        {
            InitializeComponent();
            panel1.Controls.Add(dataTypeTab);
            listBox3.SelectedValueChanged += selectCondition;

            panel1.Height = panel1.Parent.Height - 45;
            dataTypeTab.Dock = DockStyle.Fill;

            listBox2.Items.AddRange(operatorDict.Keys.ToArray());
        }

        public void button2_Click(object sender, EventArgs e)
        {
            if (!textBox4.Text.Equals("") && listBox1.SelectedItem != null && listBox2.SelectedItem != null)
            {

                string column = listBox1.SelectedItem.ToString();
                string op = operatorDict[listBox2.SelectedItem.ToString()];
                string val = textBox4.Text;

                if (op.Equals("LIKE"))
                    val += "%";
                if (column.Equals("datetime") || column.Equals("date"))
                    val = $"'{val}'";

                string condition = $"{column} {op} {val}";
                if (IsValidWhereClause(condition))
                {
                    if (conditions.Count != 0)
                    {
                        if (conditions.Last().ToString().Equals("AND") || conditions.Last().ToString().Equals("OR"))
                        {
                            conditions.Add(condition);
                        }
                        else
                        {
                            string andor = "";
                            if (conditions.Count != 0)
                            {
                                andor = selectAndOr();
                            }
                            //conditions.Add("AND");
                            conditions.Add(andor + condition);
                        }
                    }
                    else
                    {
                        conditions.Add(condition);
                    }
                }
                else
                {
                    MessageBox.Show("조건 구성이 올바르지 않습니다.");
                }
                textBox4.Clear();
            }
            condListRefresher();
        }

        public bool IsValidWhereClause(string whereClause)
        {
            string pattern = @"^(?:\s*\w+\s*(?:=|<>|>|<|>=|<=|LIKE|BETWEEN)\s*(?:'[^']*'|[\w\d%_\-\.]+(?:\.\d+)?)(?:\s*AND\s*(?:'[\w\d%_\-\.]+(?:\.\d+)?'))?(?:\s*ESCAPE\s*'\w')?(?:\s*AND\s*(?:'[\w\d%_\-\.]+(?:\.\d+)?'))?(?:\s*ESCAPE\s*'\w')?\s*(?:AND|OR)?\s*)*$";
            return Regex.IsMatch(whereClause, pattern, RegexOptions.IgnoreCase);
        }

        public void condListRefresher()
        {
            listBox3.Items.Clear();
            if (conditions.Count != 0)
            {
                string andorTemp = "";
                if (conditions[0].Contains("AND "))
                    andorTemp = "AND ";
                conditions[0] = conditions[0].Replace("AND ", "");
                if (conditions[0].Contains("OR "))
                    andorTemp = "OR ";
                conditions[0] = conditions[0].Replace("OR ", "");
                if (conditions.Count > 1)
                {
                    if (!conditions[1].Contains("AND ") || !conditions[1].Contains("OR "))
                    {
                        conditions[1] = andorTemp + conditions[1];
                    }
                }
            }
            listBox3.Items.AddRange(conditions.ToArray());
        }

        public void listBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (listBox3.SelectedItem != null)
                {
                    string selectedItem = listBox3.SelectedItem.ToString();
                    conditions.Remove(selectedItem);
                    condListRefresher();
                }
            }
        }

        public string selectAndOr()
        {
            string result = "AND ";
            Form andorForm = new Form();
            andorForm.Size = new Size(200, 100);
            andorForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            andorForm.MaximizeBox = false;
            andorForm.MinimizeBox = false;
            andorForm.StartPosition = FormStartPosition.CenterParent;
            andorForm.Text = "Select AND OR";

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            Button and = new Button();
            Button or = new Button();
            and.Margin = new Padding(10, 10, 5, 10);
            or.Margin = new Padding(5, 10, 10, 10);
            and.Dock = DockStyle.Fill;
            or.Dock = DockStyle.Fill;
            and.Text = "AND";
            or.Text = "OR";

            and.Click += (s, e) => { result = "AND "; andorForm.Close(); };
            or.Click += (s, e) => { result = "OR "; andorForm.Close(); };

            andorForm.Controls.Add(tableLayoutPanel);
            tableLayoutPanel.Controls.Add(and, 0, 0);
            tableLayoutPanel.Controls.Add(or, 1, 0);

            andorForm.ShowDialog(this);
            and.Focus();

            return result;
        }
        private void selectCondition(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {
                string[] splitCon = listBox3.SelectedItem.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int i = 0;
                foreach (string c in splitCon)
                {
                    if (c.Equals("AND") || c.Equals("OR"))
                    {

                    }
                    else
                    {
                        if (i == 0)
                        {
                            listBox1.SelectedItem = c;
                        }
                        else if (i == 1)
                        {
                            listBox2.SelectedItem = operatorDict.FirstOrDefault(x => x.Value == c).Key;
                        }
                        else
                        {
                            string value = c;
                            if (c.Contains("%"))
                            {
                                value = value.Replace("%", "");
                            }
                            if (c.Contains("'"))
                            {
                                value = value.Replace("'", "");
                            }
                            textBox4.Text = value;
                        }
                        i++;
                    }
                }
                textBox4.Focus();
                textBox4.SelectAll();
            }
        }

        public void button3_Click(object sender, EventArgs e) // 날짜 입력
        {
            // 새로운 Form 생성
            Form calendarForm = new Form();
            calendarForm.Size = new Size(400, 400);
            calendarForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            calendarForm.MaximizeBox = false;
            calendarForm.MinimizeBox = false;
            calendarForm.StartPosition = FormStartPosition.CenterParent;
            calendarForm.Text = "Select Date";

            MonthCalendar calendar1 = new MonthCalendar();

            // MonthCalendar의 속성 설정
            calendar1.ShowToday = true;
            calendar1.ShowTodayCircle = true;

            // MonthCalendar의 DateSelected 이벤트 처리
            calendar1.DateSelected += (s, args) =>
            {
                // 선택한 날짜를 yyyy-MM-dd 형식으로 가져오기
                string selectedDate = args.Start.ToString("yyyy-MM-dd");

                // 선택한 날짜를 TextBox에 추가
                textBox4.Text = selectedDate;

                // 대화상자 닫기
                calendarForm.Close();
            };

            calendar1.KeyDown += (s, args) =>
            {
                if (args.KeyCode == Keys.Escape)
                {
                    calendarForm.Close();
                }
            };

            // 닫기 버튼 생성
            System.Windows.Forms.Button closeButton = new Button();
            closeButton.Text = "Close";
            closeButton.DialogResult = DialogResult.Cancel;

            // 컨트롤 배치를 위한 TableLayoutPanel 생성
            TableLayoutPanel layoutPanel = new TableLayoutPanel();
            layoutPanel.RowCount = 2;
            layoutPanel.ColumnCount = 1;
            layoutPanel.Dock = DockStyle.Fill;
            layoutPanel.Controls.Add(calendar1, 0, 0);
            layoutPanel.Controls.Add(closeButton, 0, 1);

            // TableLayoutPanel을 Form에 추가
            calendarForm.Controls.Add(layoutPanel);

            // 대화상자 크기 조정

            calendarForm.ClientSize = new Size(calendar1.Width + 60, calendar1.Height + closeButton.Height + 40);

            // 대화상자 표시
            calendarForm.ShowDialog(this);
        }

        public void button8_Click(object sender, EventArgs e)
        {
            conditions.Clear();
            condListRefresher();
        }

        public void finalQueryGen()
        {
            int len = conditions.Count();

            if (len != 0)
            {
                if (conditions[len - 1].Equals("AND") || conditions[len - 1].Equals("OR"))
                {
                    conditions.RemoveAt(len - 1);
                }
            }
            condListRefresher();
        }


        public void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataTypeTab.SelectedTab.Text.Equals("PData"))
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(Utils.pdata);
                //groupBox1.Controls.Add(button1);
                //groupBox1.Controls.Remove(button5);
                dataTypeTab.SelectedTab.Controls.Add(listBox1);
                conditions.Clear();
                condListRefresher();
            }
            else
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(Utils.qdata);
                //groupBox1.Controls.Add(button5);
                //groupBox1.Controls.Remove(button1);
                dataTypeTab.SelectedTab.Controls.Add(listBox1);
                conditions.Clear();
                condListRefresher();
            }
        }

        public void button9_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex > 0)
            {
                string temp;
                temp = conditions[listBox3.SelectedIndex];
                conditions[listBox3.SelectedIndex] = conditions[listBox3.SelectedIndex - 1];
                conditions[listBox3.SelectedIndex - 1] = temp;
                int afterindex = listBox3.SelectedIndex - 1;
                condListRefresher();
                listBox3.SelectedIndex = afterindex;
            }
        }

        public void button10_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex >= 0 && listBox3.SelectedIndex < conditions.Count - 1)
            {
                string temp;
                temp = conditions[listBox3.SelectedIndex];
                conditions[listBox3.SelectedIndex] = conditions[listBox3.SelectedIndex + 1];
                conditions[listBox3.SelectedIndex + 1] = temp;
                int afterindex = listBox3.SelectedIndex + 1;
                condListRefresher();
                listBox3.SelectedIndex = afterindex;
            }
        }

        public Control getTab()
        {
            return dataTypeTab;
        }

        public Control getGroupBox()
        {
            return groupBox1;
        }

        public int getLocationX()
        {
            return groupBox1.Location.X;
        }

        public int getLocationY()
        {
            return groupBox1.Location.Y;
        }

        public string getCurrentTab()
        {
            return dataTypeTab.SelectedTab.Text;
        }

        public Button submitButton()
        {
            return button1;
        }

        public void setDataType(string dataType)
        {
            if (dataType.Equals("PData"))
            {
                TabPage tabPage = new TabPage();
                tabPage.Text = "PData";
                dataTypeTab.TabPages.Add(tabPage);
                listBox1.Items.AddRange(Utils.pdata);
                tabPage.Controls.Add(listBox1);
            }
            else if (dataType.Equals("QData"))
            {
                TabPage tabPage = new TabPage();
                tabPage.Text = "QData";
                dataTypeTab.TabPages.Add(tabPage);
                listBox1.Items.AddRange(Utils.qdata);
                tabPage.Controls.Add(listBox1);
            }
        }

        public void setDataType()
        {
            TabPage tabPage1 = new TabPage();
            TabPage tabPage2 = new TabPage();
            tabPage1.Text = "PData";
            tabPage2.Text = "QData";
            dataTypeTab.TabPages.Add(tabPage1);
            dataTypeTab.TabPages.Add(tabPage2);
            listBox1.Items.AddRange(Utils.pdata);
            tabPage1.Controls.Add(listBox1);
            dataTypeTab.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
        }

        Dictionary<string, string> operatorDict = new Dictionary<string, string>
        {
            {"정확히 일치", "="},
            {"같거나 비슷함", "LIKE"},
            {"큼", ">"},
            {"크거나 같음", ">="},
            {"작음", "<"},
            {"작거나 같음", "<="}
        };

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {

                string column = listBox1.SelectedItem.ToString();
                string op = operatorDict[listBox2.SelectedItem.ToString()];
                string val = textBox4.Text;

                if (op.Equals("LIKE"))
                    val += "%";
                if (column.Equals("datetime") || column.Equals("date"))
                    val = $"'{val}'";

                string condition = $"{column} {op} {val}";
                if (IsValidWhereClause(condition))
                {
                    if (listBox3.SelectedIndex != 0)
                    {
                        string andor = "";
                        if (conditions.Count != 0)
                        {
                            andor = selectAndOr();
                        }
                        //conditions.Add("AND");
                        conditions[listBox3.SelectedIndex] = andor + condition;
                    }
                    else
                    {
                        conditions[listBox3.SelectedIndex] = condition;
                    }
                }
                else
                {
                    MessageBox.Show("조건 구성이 올바르지 않습니다.");
                }
                textBox4.Clear();
            }
            condListRefresher();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {
                string selectedItem = listBox3.SelectedItem.ToString();
                conditions.Remove(selectedItem);
                condListRefresher();
                textBox4.Clear();
            }
        }
    }
}