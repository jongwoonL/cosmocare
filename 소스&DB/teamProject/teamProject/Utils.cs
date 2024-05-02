using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;

namespace teamProject
{
    public class Utils
    {
        // 데이터 표시 포맷 - P or QData(Form2, 3)
        public static void Format(DataGridView dgv, string data, int digit)
        {
            if (data.Equals("PData"))
            {
                // 날짜 설정 :: 시, 분, 초 까지
                dgv.Columns["datetime"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                // 소수점 이하 digit 자리까지만 표시되도록 설정
                string[] columns = { "ReactA_Temp", "ReactB_Temp", "ReactC_Temp", "ReactD_Temp", "ReactE_Temp",
            "ReactF_Temp", "ReactF_PH", "Power", "CurrentA", "CurrentB","CurrentC"};
                for (int i = 0; i < columns.Length; i++)
                {
                    dgv.Columns[columns[i]].DefaultCellStyle.Format = "N" + $"{digit}";
                }
            }

            if (data.Equals("QData"))
            {
                // 날짜 설정 :: 일 까지
                dgv.Columns["date"].DefaultCellStyle.Format = "yyyy-MM-dd";
                // 소수점 이하 digit 자리까지만 표시되도록 설정
                string[] columns2 = { "weight", "water", "material", "HSO", "pH" };
                for (int i = 0; i < columns2.Length; i++)
                {
                    dgv.Columns[columns2[i]].DefaultCellStyle.Format = "N" + $"{digit}";
                }
            }
        }

        // 데이터 표시 포맷, 시간은 초까지, 소수점은 digit 자리까지 - 2개(Form1)
        public static void Format(DataGridView dgv1, DataGridView dgv2, int digit)
        {
            dgv1.Columns["datetime"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            dgv2.Columns["date"].DefaultCellStyle.Format = "yyyy-MM-dd";

            // gridview1 소수점 이하 digit 자리까지만 표시되도록 설정
            string[] columns = { "ReactA_Temp", "ReactB_Temp", "ReactC_Temp", "ReactD_Temp", "ReactE_Temp",
            "ReactF_Temp", "ReactF_PH", "Power", "CurrentA", "CurrentB","CurrentC"};
            for (int i = 0; i < columns.Length; i++)
            {
                dgv1.Columns[columns[i]].DefaultCellStyle.Format = "N" + $"{digit}";
            }

            // gridview2 소수점 이하 digit 자리까지만 표시되도록 설정
            string[] columns2 = { "weight", "water", "material", "HSO", "pH" };
            for (int i = 0; i < columns2.Length; i++)
            {
                dgv2.Columns[columns2[i]].DefaultCellStyle.Format = "N" + $"{digit}";
            }
        }

        // 화면 리프레시 - 1개(Form2, 3)
        public static async Task reScreen(DataGridView dgv, string data, int digit, System.Windows.Forms.ProgressBar pb)
        {
            dgv.DataSource = null;

            pb.Visible = true; // 로딩 바 표시

            if (data.Equals("PData"))
            {
                // 비동기적으로 데이터 로드
                await Task.Run(() =>
                {
//                    Thread.Sleep(10000); // 10초 대기
                    DataManager.LoadP();
                });
            }

            if (data.Equals("QData"))
            {
                // 비동기적으로 데이터 로드
                await Task.Run(() =>
                {
//                    Thread.Sleep(10000); // 10초 대기
                    DataManager.LoadQ();
                });
            }

            //if (data.Equals("PData") && DataManager.datasP.Count > 0)
            if (data.Equals("PData"))
            {
                dgv.DataSource = DataManager.datasP;
                dgv.Columns[0].Width = 150;
            }
            //if (data.Equals("QData") && DataManager.datasQ.Count > 0)
            if (data.Equals("QData"))
            {
                dgv.DataSource = DataManager.datasQ;
                dgv.Columns[0].Width = 100;
            }
            Format(dgv, data, digit);

            pb.Visible = false; // 로딩 바 숨김
        }

        public static async Task reScreen(DataGridView dgv1, DataGridView dgv2, int digit, System.Windows.Forms.ProgressBar pb)
        {
            dgv1.DataSource = null;
            dgv2.DataSource = null;

            pb.Visible = true; // 로딩 바 표시

            // 비동기적으로 데이터 로드
            await Task.Run(() =>
            {
 //               Thread.Sleep(10000); // 10초 대기
                DataManager.LoadP();
                DataManager.LoadQ();
            });

            // 데이터 로드 후 화면 갱신
            dgv1.DataSource = DataManager.datasP;
            dgv1.Columns[0].Width = 150;
            dgv2.DataSource = DataManager.datasQ;
            dgv2.Columns[0].Width = 100;
            Format(dgv1, dgv2, digit);

            pb.Visible = false; // 로딩 바 숨김
        }

        // 화면 리프레시 - 조건
        public static async Task reScreen(DataGridView dgv, string data, string sql, int digit, System.Windows.Forms.ProgressBar pb)
        {
            dgv.DataSource = null;

            pb.Visible = true; // 로딩 바 표시

            if (data.Equals("PData"))
            {
                // 비동기적으로 데이터 로드
                await Task.Run(() =>
                {
//                    Thread.Sleep(10000); // 10초 대기
                    DataManager.LoadP(sql);
                });
            }

            if (data.Equals("QData"))
            {
                // 비동기적으로 데이터 로드
                await Task.Run(() =>
                {
//                    Thread.Sleep(10000); // 10초 대기
                    DataManager.LoadQ(sql);
                });
            }

            // 데이터 로드 후 화면 갱신
            // if (data.Equals("PData") && DataManager.datasP.Count > 0)
            if (data.Equals("PData"))
            {
                dgv.DataSource = DataManager.datasP;
                dgv.Columns[0].Width = 150;
            }

            // 데이터 로드 후 화면 갱신
            // if (data.Equals("QData") && DataManager.datasQ.Count > 0)
            if (data.Equals("QData"))
            {
                dgv.DataSource = DataManager.datasQ;
                dgv.Columns[0].Width = 100;
            }
            Format(dgv, data, digit);

            pb.Visible = false; // 로딩 바 숨김
        }

        public static string[] pdata = new string[] { "datetime", "ReactA_Temp", "ReactB_Temp", "ReactC_Temp", "ReactD_Temp", "ReactE_Temp", "ReactF_Temp", "ReactF_PH", "Power", "CurrentA", "CurrentB", "CurrentC" };
        public static string[] qdata = new string[] { "date", "weight", "water", "material", "HSO", "pH" };
        public static string[] operators = new string[] { "=", "LIKE", ">", ">=", "<", "<=", "AND", "OR" };
    }
}