using System;
using System.Drawing;
using System.Windows.Forms;

namespace teamProject
{
    public partial class FirstScreen : Form
    {
        private Main main;

        private Timer timer = new Timer();
        private int currentIndex = 0;

        // 이미지 배열 생성
        private Image[] images = { Properties.Resources.image1, Properties.Resources.image2, Properties.Resources.image3, Properties.Resources.image4};

        public FirstScreen()
        {
            InitializeComponent();
            CenterToScreen(); // 폼을 화면의 정중앙에 배치합니다.

            // 타이머 설정
            timer.Interval = 1000; // 1초마다 변경
            timer.Tick += Timer_Tick;
            timer.Start();

            // 초기 이미지 표시
            mainImages.Image = images[currentIndex];
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 다음 이미지로 변경
            currentIndex = (currentIndex + 1) % images.Length;
            mainImages.Image = images[currentIndex];
        }
        
        // 시작하기

        private void startButton_Click(object sender, EventArgs e)
        {
            main = new Main();
            main.Show();
            this.Hide();
        }
    }
}
