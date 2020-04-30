using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;


namespace CSharpchainWebAPI.Models
{
    public class Account
    {
        public long Id { get; set; }
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
                        Id = s.ma_taikhoan,
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

        public bool EditAccountInfo(long id, string name, string gender, string birthday, string sdt, string email, string address)
        {
            try
            {
                using (admin_voteEntities db = new admin_voteEntities())
                {
                    var account = db.Set<tbl_taikhoan>().FirstOrDefault(x => x.ma_taikhoan == id);
                    if (name.Length != 0)
                        account.sHovaten = name;
                    account.bGgioitinh = gender == "Nam" ? true : false;
                    if (birthday.Length != 0)
                        account.dNgaysinh = DateTime.ParseExact(birthday, "dd/mm/yyyy", CultureInfo.InvariantCulture);
                    if (sdt.Length != 0)
                        account.sSdt = sdt;
                    if (email.Length != 0)
                        account.sEmail = email;
                    if (address.Length != 0)
                        account.sDiachi = address;
                    db.Set<tbl_taikhoan>().AddOrUpdate(account);
                    db.SaveChanges();
                }
            }
            catch (InvalidCastException e)
            {
                return false;
            }
            return true;
        }
    }
}