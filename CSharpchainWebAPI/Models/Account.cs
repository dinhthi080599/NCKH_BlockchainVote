using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;


namespace CSharpchainWebAPI.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }
        public string userAccount { get; set; }
        public string sNgaysinh { get; set; }
        public string sSodienthoai { get; set; }
        public string sGioitinh { get; set; }
        public string sEmail { get; set; }
        public string sDiachi { get; set; }
        public int ma_quyen { get; set; }

        public Account GetAccount_byID(int id)
        {
            Account a = new Account();
            using (var ctx = new admin_voteEntities())
            {
                a = ctx.tbl_taikhoan
                    .Select(s => new Account()
                    {
                        userAccount = s.sHovaten.ToString(),
                        sDiachi = s.sDiachi.ToString(),
                        sGioitinh = s.bGgioitinh == true ? "Nam" : "Nu",
                        sNgaysinh = s.dNgaysinh.ToString(),
                        sEmail = s.sEmail.ToString(),
                        sSodienthoai = s.sSdt.ToString()
                    }).FirstOrDefault<Account>();
            }
            return a;
        }

        public HashSet<long> list_voted_byID(int id)
        {
            HashSet<long> list_voted = new HashSet<long>();
            return list_voted;
        }
    }
}