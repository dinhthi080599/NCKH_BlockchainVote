using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CSharpchainWebAPI.Models
{
    public partial class Elector
    {
        public long ma_cutri { get; set; }
        public string sHoten { get; set; }
        public string bGioitinh { get; set; }
        public string dNgaysinh { get; set; }
        public string sEmail { get; set; }
        public string sDiachi { get; set; }
        public long ma_dotbaucu { get; set; }

        public List<Elector> getElectorbyId(int i)
        {
            List<Elector> ElectorList = null;
            using (var db = new admin_voteEntities())
            {
                ElectorList = db.tbl_cutri
                            .Select(s => new Elector()
                            {
                                ma_dotbaucu = (long)s.ma_dotbaucu,
                                sHoten = s.sHoten,
                                bGioitinh = s.bGioitinh == true ? "Nam" : "Nữ",
                                dNgaysinh = SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("dd", s.dNgaysinh).Trim().Length)
                                            + SqlFunctions.DateName("dd", s.dNgaysinh).Trim()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart().Length)
                                            + SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart().Length)
                                            + SqlFunctions.DateName("year", s.dNgaysinh),
                                sEmail = s.sEmail,
                                sDiachi = s.sDiachi,
                                ma_cutri = s.ma_cutri
                            }).Where(s => s.ma_dotbaucu == i).ToList<Elector>();
            }
            return ElectorList;
        }

        public bool createNewElector(string sHoten, string bGioitinh, string dNgaysinh, string sEmail, string sDiachi, string ma_dotbaucu)
        {
            try
            {
                using (admin_voteEntities db = new admin_voteEntities())
                {
                    var electorDB = db.Set<tbl_cutri>();
                    electorDB.Add(new tbl_cutri
                    {
                        sHoten = sHoten,
                        dNgaysinh = DateTime.ParseExact(dNgaysinh, "M/d/yyyy", CultureInfo.InvariantCulture),
                        bGioitinh = bGioitinh == "Nam" ? true : false,
                        sEmail = sEmail,
                        sDiachi = sDiachi,
                        ma_dotbaucu = long.Parse(ma_dotbaucu)
                    });
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