using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamProject
{
    public class DBHelper_MSSQL : DBHelper
    {
        // 싱글톤 적용
        private static DBHelper_MSSQL instance;
        public static DBHelper_MSSQL getInstance
        {
            get
            {
                if (instance == null) // 없으면 인스턴스 하나 만들고
                {
                    instance = new DBHelper_MSSQL();
                }
                return instance; // 있으면 계속 재활용
            }
        }
        private DBHelper_MSSQL() { } // 외부에서 인스턴스 못 만들게 private

        protected override void ConnectDB()
        {
            conn.ConnectionString = $"Data Source=({"local"}); " +
                $"Initial Catalog = {"ProjectDataBase"}; Integrated Security = {"SSPI"}; Timeout={3}";
            conn = new SqlConnection(conn.ConnectionString);
            conn.Open();
        }

        // DoQueryR() // 추가 조건 설정 없으면, ps 값은 자동으로 "-1"을 대입
        // select 전체 or 특정 데이터 정보
        // 데이터 불러오기 PData
        public override void DoQueryRP(string sql = "-1")
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                
                if (sql.Equals("-1")) // 추가 조건 없는 경우
                {
                    cmd.CommandText = "select * from Process_Data";
                }
                else { 
                    cmd.CommandText = "select * from Process_Data where " + sql; // sql로 직접 검색
                }

                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds, "Process_Data");
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                DataManager.PrintLog(ex.Message);
                DataManager.PrintLog(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        // 데이터 불러오기 QData
        public override void DoQueryRQ(string sql = "-1")
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                if (sql.Equals("-1")) // 추가 조건 없는 경우
                {
                    cmd.CommandText = "select * from QC_Data";
                }
                else { 
                    cmd.CommandText = "select * from QC_Data where " + sql; // sql로 직접 검색
                }

                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds, "QC_Data");
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                DataManager.PrintLog(ex.Message);
                DataManager.PrintLog(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        // 데이터 추가 PData
        public override void DoQueryCP(PData data)
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string sql = "";
                sql = "insert into Process_Data values (@datetime, @ReactA_Temp, @ReactB_Temp, @ReactC_Temp, "
                        + "@ReactD_Temp, @ReactE_Temp, @ReactF_Temp, @ReactF_PH, @Power, @CurrentA, "
                        + "@CurrentB, @CurrentC);";
                cmd.Parameters.AddWithValue("@datetime", data.datetime);
                cmd.Parameters.AddWithValue("@ReactA_Temp", data.ReactA_Temp);
                cmd.Parameters.AddWithValue("@ReactB_Temp", data.ReactB_Temp);
                cmd.Parameters.AddWithValue("@ReactC_Temp", data.ReactC_Temp);
                cmd.Parameters.AddWithValue("@ReactD_Temp", data.ReactD_Temp);
                cmd.Parameters.AddWithValue("@ReactE_Temp", data.ReactE_Temp);
                cmd.Parameters.AddWithValue("@ReactF_Temp", data.ReactF_Temp);
                cmd.Parameters.AddWithValue("@ReactF_PH", data.ReactF_PH);
                cmd.Parameters.AddWithValue("@Power", data.Power);
                cmd.Parameters.AddWithValue("@CurrentA", data.CurrentA);
                cmd.Parameters.AddWithValue("@CurrentB", data.CurrentB);
                cmd.Parameters.AddWithValue("@CurrentC", data.CurrentC);

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                DataManager.PrintLog(ex.Message);
                DataManager.PrintLog(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        // 데이터 추가 QData
        public override void DoQueryCQ(QData data)
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string sql = "";
                sql = "insert into QC_Data values (@date, @weight, @water, @material, @HSO, @pH);";
                cmd.Parameters.AddWithValue("@date", data.date);
                cmd.Parameters.AddWithValue("@weight", data.weight);
                cmd.Parameters.AddWithValue("@water", data.water);
                cmd.Parameters.AddWithValue("@material", data.material);
                cmd.Parameters.AddWithValue("@HSO", data.HSO);
                cmd.Parameters.AddWithValue("@pH", data.pH);

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                DataManager.PrintLog(ex.Message);
                DataManager.PrintLog(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        // 데이터 수정 PData
        public override void DoQueryUP(PData data, string select)
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string sql = "";
                sql = "update Process_Data set ReactA_Temp=@ReactA_Temp, ReactB_Temp=@ReactB_Temp, ReactC_Temp=@ReactC_Temp, "
                        + "ReactD_Temp=@ReactD_Temp, ReactE_Temp=@ReactE_Temp, ReactF_Temp=@ReactF_Temp, ReactF_PH=@ReactF_PH, Power=@Power, "
                        + "CurrentA=@CurrentA, CurrentB=@CurrentB, CurrentC=@CurrentC where datetime=@datetime;";
                cmd.Parameters.AddWithValue("@ReactA_Temp", data.ReactA_Temp);
                cmd.Parameters.AddWithValue("@ReactB_Temp", data.ReactB_Temp);
                cmd.Parameters.AddWithValue("@ReactC_Temp", data.ReactC_Temp);
                cmd.Parameters.AddWithValue("@ReactD_Temp", data.ReactD_Temp);
                cmd.Parameters.AddWithValue("@ReactE_Temp", data.ReactE_Temp);
                cmd.Parameters.AddWithValue("@ReactF_Temp", data.ReactF_Temp);
                cmd.Parameters.AddWithValue("@ReactF_PH", data.ReactF_PH);
                cmd.Parameters.AddWithValue("@Power", data.Power);
                cmd.Parameters.AddWithValue("@CurrentA", data.CurrentA);
                cmd.Parameters.AddWithValue("@CurrentB", data.CurrentB);
                cmd.Parameters.AddWithValue("@CurrentC", data.CurrentC);
                cmd.Parameters.AddWithValue("@datetime", select);

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                DataManager.PrintLog(ex.Message);
                DataManager.PrintLog(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        // 데이터 수정 QData
        public override void DoQueryUQ(QData data, string select)
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string sql = "";
                sql = "update QC_Data set weight=@weight, water=@water, material=@material, HSO=@HSO, pH=@pH where date=@date;";
                cmd.Parameters.AddWithValue("@weight", data.weight);
                cmd.Parameters.AddWithValue("@water", data.water);
                cmd.Parameters.AddWithValue("@material", data.material);
                cmd.Parameters.AddWithValue("@HSO", data.HSO);
                cmd.Parameters.AddWithValue("@pH", data.pH);
                cmd.Parameters.AddWithValue("@date", select);

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                DataManager.PrintLog(ex.Message);
                DataManager.PrintLog(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        // 데이터 삭제 PData
        public override void DoQueryDP(PData data)
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string sql = "";
                sql = "delete from Process_Data where datetime=@datetime;";
                cmd.Parameters.Add("@datetime", SqlDbType.DateTime2).Value = data.datetime;

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                DataManager.PrintLog(ex.Message);
                DataManager.PrintLog(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        // 데이터 삭제 QData
        public override void DoQueryDQ(QData data)
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string sql = "";
                sql = "delete from QC_Data where date=@date;";
                cmd.Parameters.Add("@date", SqlDbType.DateTime2).Value = data.date;

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                DataManager.PrintLog(ex.Message);
                DataManager.PrintLog(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
