using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamProject
{
    public class DataManager
    {
        public static List<PData> datasP = new List<PData>();
        public static List<QData> datasQ = new List<QData>();

        // 앞으로 DB와의 연락은 mssql이 모두 수행
        private static DBHelper_MSSQL mssql = DBHelper_MSSQL.getInstance; // 싱글톤

        public DataManager()
        {
            LoadP();
            LoadQ();
        }

        // PData 전체 불러오기
        public static void LoadP()
        {
            try
            {
                // Process_Data
                mssql.DoQueryRP(); // Process_Data 전체 조회
                datasP.Clear(); // datas 초기화
                foreach (DataRow item in mssql.dt.Rows)
                {
                    // Process_Data
                    PData data = new PData();
                    // datetime2 값을 밀리세컨드까지 포함하여 가져옴
                    if (item["datetime"] != DBNull.Value)
                    {
                        data.datetime = (DateTime)item["datetime"];
                    }
                    else
                    {
                        data.datetime = new DateTime(); // 또는 다른 기본값 설정
                    }

                    data.ReactA_Temp = double.Parse(item["ReactA_Temp"].ToString());
                    data.ReactB_Temp = double.Parse(item["ReactB_Temp"].ToString());
                    data.ReactC_Temp = double.Parse(item["ReactC_Temp"].ToString());
                    data.ReactD_Temp = double.Parse(item["ReactD_Temp"].ToString());
                    data.ReactE_Temp = double.Parse(item["ReactE_Temp"].ToString());
                    data.ReactF_Temp = double.Parse(item["ReactF_Temp"].ToString());
                    data.ReactF_PH = double.Parse(item["ReactF_PH"].ToString());
                    data.Power = double.Parse(item["Power"].ToString());
                    data.CurrentA = double.Parse(item["CurrentA"].ToString());
                    data.CurrentB = double.Parse(item["CurrentB"].ToString());
                    data.CurrentC = double.Parse(item["CurrentC"].ToString());
                    datasP.Add(data);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                PrintLog(ex.StackTrace + "from Load");
            }
            finally
            {

            }
        }

        // QData 전체 불러오기
        public static void LoadQ()
        {
            try
            {
                // QC_Data
                mssql.DoQueryRQ(); // QC_Data 전체 조회
                datasQ.Clear(); // datas2 초기화
                foreach (DataRow item in mssql.dt.Rows)
                {
                    QData data = new QData();
                    // datetime2 값을 밀리세컨드까지 포함하여 가져옴
                    if (item["date"] != DBNull.Value)
                    {
                        data.date = (DateTime)item["date"];
                    }
                    else
                    {
                        data.date = new DateTime(); // 또는 다른 기본값 설정
                    }

                    data.weight = item["weight"] != DBNull.Value && item["weight"] != null ? double.Parse(item["weight"].ToString()) : 0; // 혹은 다른 기본값 사용
                    data.water = item["water"] != DBNull.Value && item["water"] != null ? double.Parse(item["water"].ToString()) : 0;
                    data.material = item["material"] != DBNull.Value && item["material"] != null ? double.Parse(item["material"].ToString()) : 0;
                    data.HSO = item["HSO"] != DBNull.Value && item["HSO"] != null ? double.Parse(item["HSO"].ToString()) : 0;
                    data.pH = item["pH"] != DBNull.Value && item["pH"] != null ? double.Parse(item["pH"].ToString()) : 0;
                    datasQ.Add(data);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                PrintLog(ex.StackTrace + "from Load");
            }
            finally
            {

            }
        }

        // PData 조건 불러오기
        public static void LoadP(string sql)
        {
            try
            {
                // Process_Data
                mssql.DoQueryRP(sql); // 전달된 SQL 쿼리 실행
                datasP.Clear(); // datas 초기화
                foreach (DataRow item in mssql.dt.Rows)
                {
                    PData data = new PData();
                    // datetime2 값을 밀리세컨드까지 포함하여 가져옴
                    if (item["datetime"] != DBNull.Value)
                    {
                        data.datetime = (DateTime)item["datetime"];
                    }
                    else
                    {
                        data.datetime = new DateTime(); // 또는 다른 기본값 설정
                    }

                    data.ReactA_Temp = double.Parse(item["ReactA_Temp"].ToString());
                    data.ReactB_Temp = double.Parse(item["ReactB_Temp"].ToString());
                    data.ReactC_Temp = double.Parse(item["ReactC_Temp"].ToString());
                    data.ReactD_Temp = double.Parse(item["ReactD_Temp"].ToString());
                    data.ReactE_Temp = double.Parse(item["ReactE_Temp"].ToString());
                    data.ReactF_Temp = double.Parse(item["ReactF_Temp"].ToString());
                    data.ReactF_PH = double.Parse(item["ReactF_PH"].ToString());
                    data.Power = double.Parse(item["Power"].ToString());
                    data.CurrentA = double.Parse(item["CurrentA"].ToString());
                    data.CurrentB = double.Parse(item["CurrentB"].ToString());
                    data.CurrentC = double.Parse(item["CurrentC"].ToString());
                    datasP.Add(data);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                PrintLog(ex.StackTrace + "from Load");
            }
            finally
            {

            }
        }

        // QData 조건 불러오기
        public static void LoadQ(string sql)
        {
            try
            {
                // QC_Data
                mssql.DoQueryRQ(sql); // QC_Data 전체 조회
                datasQ.Clear(); // datas2 초기화
                foreach (DataRow item in mssql.dt.Rows)
                {
                    QData data = new QData();
                    // datetime2 값을 밀리세컨드까지 포함하여 가져옴
                    if (item["date"] != DBNull.Value)
                    {
                        data.date = (DateTime)item["date"];
                    }
                    else
                    {
                        data.date = new DateTime(); // 또는 다른 기본값 설정
                    }

                    data.weight = item["weight"] != DBNull.Value && item["weight"] != null ? double.Parse(item["weight"].ToString()) : 0; // 혹은 다른 기본값 사용
                    data.water = item["water"] != DBNull.Value && item["water"] != null ? double.Parse(item["water"].ToString()) : 0;
                    data.material = item["material"] != DBNull.Value && item["material"] != null ? double.Parse(item["material"].ToString()) : 0;
                    data.HSO = item["HSO"] != DBNull.Value && item["HSO"] != null ? double.Parse(item["HSO"].ToString()) : 0;
                    data.pH = item["pH"] != DBNull.Value && item["pH"] != null ? double.Parse(item["pH"].ToString()) : 0;
                    datasQ.Add(data);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                PrintLog(ex.StackTrace + "from Load");
            }
            finally
            {

            }
        }

        // 데이터 추가 PData
        public static void Save(PData data)
        {
            mssql.DoQueryCP(data);
            PrintLog(data.datetime.ToString() + " 데이터 추가");
        }

        // 데이터 추가 QData
        public static void Save(QData data)
        {
            mssql.DoQueryCQ(data);
            PrintLog(data.date.ToString() + " 데이터 추가");
        }

        // 데이터 수정 PData
        public static void Update(PData data, string select)
        {
            mssql.DoQueryUP(data, select);
            PrintLog(data.datetime.ToString() + " 데이터 수정");
        }

        // 데이터 수정 QData
        public static void Update(QData data, string select)
        {
            mssql.DoQueryUQ(data, select);
            PrintLog(data.date.ToString() + " 데이터 수정");
        }


        // 데이터 삭제 PData
        public static void Delete(PData data)
        {
            mssql.DoQueryDP(data);
            PrintLog(data.datetime.ToString() + " 데이터 삭제");
        }

        // 데이터 삭제 QData
        public static void Delete(QData data)
        {
            mssql.DoQueryDQ(data);
            PrintLog(data.date.ToString() + " 데이터 삭제");
        }


        // 로그 저장, contents = 로그 기록용
        public static void PrintLog(string contents)
        {
            DirectoryInfo di = new DirectoryInfo("LogFolder");
            if (di.Exists == false) // 해당 폴더 없으면
            {
                di.Create(); // 폴더 생성
            }

            using (StreamWriter w = new StreamWriter(@"LogFolder\History.txt", true))
            {
                w.Write($"({DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
                w.WriteLine(contents);
            }
        }

    }
}
