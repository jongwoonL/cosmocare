using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamProject
{
    public abstract class DBHelper
    {
        protected SqlConnection conn = new SqlConnection();
        protected SqlDataAdapter da;
        protected DataSet ds;
        public DataTable dt;

        protected abstract void ConnectDB();

        public abstract void DoQueryRP(string sql = "-1"); // PData select 용
        public abstract void DoQueryRQ(string sql = "-1"); // QData select 용
        public abstract void DoQueryUP(PData data, string select); // PData update 용
        public abstract void DoQueryUQ(QData data, string select); // QData update 용
        public abstract void DoQueryCP(PData data); // PData insert용
        public abstract void DoQueryCQ(QData data); // QData insert용

        public abstract void DoQueryDP(PData data); // PData delete 용
        public abstract void DoQueryDQ(QData data); // QData delete 용
    }
}
