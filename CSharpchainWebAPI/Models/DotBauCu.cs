using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;


namespace CSharpchainWebAPI.Models
{
    public partial class DotBauCu
    {
        public long ma_dot { get; set; }
        public string sTenDot { get; set; }
        public string dThoiGianBD { get; set; }
        public string dThoiGianKT { get; set; }
        public string sNoiDung { get; set; }
        public long iTrangThai { get; set; }
        public string sGhiChu { get; set; }

        public List<DotBauCu> get_dotbaucu()
        {
            List<DotBauCu> dotBauCu = null;
            using (var ctx = new admin_voteEntities())
            {
                dotBauCu = ctx.tbl_dotbaucu
                            .Select(s => new DotBauCu()
                            {
                                ma_dot = s.ma_dot,
                                dThoiGianBD = SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("dd", s.dThoigianbd).Trim().Length)
                                            + SqlFunctions.DateName("dd", s.dThoigianbd).Trim()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dThoigianbd.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)s.dThoigianbd.Value.Month).TrimStart().Length)
                                            + SqlFunctions.StringConvert((double)s.dThoigianbd.Value.Month).TrimStart()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dThoigianbd.Value.Month).TrimStart().Length)
                                            + SqlFunctions.DateName("year", s.dThoigianbd),
                                dThoiGianKT = SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("dd", s.dThoigiankt).Trim().Length)
                                            + SqlFunctions.DateName("dd", s.dThoigiankt).Trim()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dThoigiankt.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)s.dThoigiankt.Value.Month).TrimStart().Length)
                                            + SqlFunctions.StringConvert((double)s.dThoigiankt.Value.Month).TrimStart()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dThoigiankt.Value.Month).TrimStart().Length)
                                            + SqlFunctions.DateName("year", s.dThoigiankt),
                                sNoiDung = s.sNoiDung.ToString(),
                                iTrangThai = s.iTrangThai,
                                sTenDot = s.sTendot.ToString(),
                                sGhiChu = s.sGhichu.ToString(),
                            }).ToList<DotBauCu>();
            }
            return dotBauCu;
        }

        public DotBauCu get_dotbaucu_by_ID(int ma_dot)
        {
            DotBauCu dotBauCu = new DotBauCu();
            using (var ctx = new admin_voteEntities())
            {
                dotBauCu = ctx.tbl_dotbaucu
                    .Select(s => new DotBauCu()
                    {
                        ma_dot = s.ma_dot,
                        dThoiGianBD = SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("dd", s.dThoigianbd).Trim().Length)
                                            + SqlFunctions.DateName("dd", s.dThoigianbd).Trim()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dThoigianbd.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)s.dThoigianbd.Value.Month).TrimStart().Length)
                                            + SqlFunctions.StringConvert((double)s.dThoigianbd.Value.Month).TrimStart()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dThoigianbd.Value.Month).TrimStart().Length)
                                            + SqlFunctions.DateName("year", s.dThoigianbd),
                        dThoiGianKT = SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("dd", s.dThoigiankt).Trim().Length)
                                            + SqlFunctions.DateName("dd", s.dThoigiankt).Trim()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dThoigiankt.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)s.dThoigiankt.Value.Month).TrimStart().Length)
                                            + SqlFunctions.StringConvert((double)s.dThoigiankt.Value.Month).TrimStart()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dThoigiankt.Value.Month).TrimStart().Length)
                                            + SqlFunctions.DateName("year", s.dThoigiankt),
                        sNoiDung = s.sNoiDung.ToString(),
                        iTrangThai = s.iTrangThai,
                        sTenDot = s.sTendot.ToString(),
                        sGhiChu = s.sGhichu.ToString(),
                    })
                    .Where(s => s.ma_dot == ma_dot).FirstOrDefault<DotBauCu>();
            }
            return dotBauCu;
        }

    }
}