using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace teamProject
{
    public partial class Form0 : Form
    {
        private Form1 form1;
        private Thread loadingThread;

        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private int currentIndex = 0;

        // 이미지 배열 혹은 리스트를 만듭니다.
        private Image[] images = { Properties.Resources.image1, Properties.Resources.image2, Properties.Resources.image3, Properties.Resources.image4};

        public Form0()
        {
            InitializeComponent();
            CenterToScreen(); // 폼을 화면의 정중앙에 배치합니다.

            // 타이머 설정
            timer.Interval = 1000; // 1초마다 변경
            timer.Tick += Timer_Tick;
            timer.Start();

            // 초기 이미지 표시
            pictureBox2.Image = images[currentIndex];
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 다음 이미지로 변경
            currentIndex = (currentIndex + 1) % images.Length;
            pictureBox2.Image = images[currentIndex];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}
