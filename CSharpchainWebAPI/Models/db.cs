using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace CSharpchainWebAPI.Models
{
    public class SqlConnectionData
    {
        public static SqlConnection connect()
        {
            string datasource = @"Data Source=DESKTOP-THVERTV;Initial Catalog=admin_vote;Persist Security Info=True;User ID=sa;Password=1";
            SqlConnection conn = new SqlConnection(datasource);
            return conn;
        }

    }

    public class db
    {
        public static long sp_login(string taikhoan, string matkhau)
        {
            long ma_quyen;
            try
            {
                SqlConnection conn = SqlConnectionData.connect();
                SqlCommand cmd = new SqlCommand("sp_login", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@sTendangnhap", taikhoan);
                cmd.Parameters.AddWithValue("@sMatKhau", matkhau);
                cmd.Parameters.Add("@ma_taikhoan", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ma_quyen", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                conn.Open();

                long ma_taikhoan = Convert.ToInt32(cmd.Parameters["@ma_taikhoan"].Value);
                ma_quyen = Convert.ToInt32(cmd.Parameters["@ma_quyen"].Value);
                conn.Close();
            }
            catch (Exception)
            {
                return 0;
            }
            return ma_quyen;
        }// end of sp_login
    }
}