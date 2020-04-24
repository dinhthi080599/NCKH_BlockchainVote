using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSharpchainWebAPI.Models
{
    public partial class DotBauCu
    {
        public long ma_dot { get; set; }
        public string sTenDot { get; set; }
        public string dThoiGianBD { get; set; }
        public string dThoiGianKT { get; set; }

        [AllowHtml]
        public string sNoiDung { get; set; }
        public int iTrangThai { get; set; }
        public string sGhiChu { get; set; }
        public string sHinhThuc { get; set; }
        public string sSoPhieu { get; set; }
        public int iNguoiTao { get; set; }
        public string ten_dauphieu { get; set; }
        public string type { get; set; }
        

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
                                iTrangThai = (int)s.iTrangThai,
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
                    .Join(ctx.dm_dauphieu,
                        tbl_dotbaucu => tbl_dotbaucu.sSoPhieu,
                        dm_dauphieu => dm_dauphieu.ma_dauphieu,
                        (tbl_dotbaucu, dm_dauphieu) => new DotBauCu()
                        {
                            ma_dot = tbl_dotbaucu.ma_dot,
                            dThoiGianBD = SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("dd", tbl_dotbaucu.dThoigianbd).Trim().Length)
                                            + SqlFunctions.DateName("dd", tbl_dotbaucu.dThoigianbd).Trim()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigianbd.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigianbd.Value.Month).TrimStart().Length)
                                            + SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigianbd.Value.Month).TrimStart()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigianbd.Value.Month).TrimStart().Length)
                                            + SqlFunctions.DateName("year", tbl_dotbaucu.dThoigianbd)
                                            + SqlFunctions.Replicate(" ", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigianbd.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigianbd.Value.Hour).TrimStart().Length)
                                            + SqlFunctions.DateName("HH", tbl_dotbaucu.dThoigianbd)
                                            + SqlFunctions.Replicate(":", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigianbd.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigianbd.Value.Minute).TrimStart().Length)
                                            + SqlFunctions.DateName("minute", tbl_dotbaucu.dThoigianbd)
                                            ,
                            dThoiGianKT = SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("dd", tbl_dotbaucu.dThoigiankt).Trim().Length)
                                            + SqlFunctions.DateName("dd", tbl_dotbaucu.dThoigiankt).Trim()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigiankt.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigiankt.Value.Month).TrimStart().Length)
                                            + SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigiankt.Value.Month).TrimStart()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigiankt.Value.Month).TrimStart().Length)
                                            + SqlFunctions.DateName("year", tbl_dotbaucu.dThoigiankt)
                                            + SqlFunctions.Replicate(" ", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigiankt.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigiankt.Value.Hour).TrimStart().Length)
                                            + SqlFunctions.DateName("HH", tbl_dotbaucu.dThoigiankt)
                                            + SqlFunctions.Replicate(":", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigiankt.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)tbl_dotbaucu.dThoigiankt.Value.Minute).TrimStart().Length)
                                            + SqlFunctions.DateName("minute", tbl_dotbaucu.dThoigiankt)
                                            ,
                            iNguoiTao = (int)tbl_dotbaucu.iNguoiTao,
                            sNoiDung = tbl_dotbaucu.sNoiDung.ToString(),
                            sHinhThuc = tbl_dotbaucu.sHinhThuc,
                            iTrangThai = (int)tbl_dotbaucu.iTrangThai,
                            sTenDot = tbl_dotbaucu.sTendot.ToString(),
                            sGhiChu = tbl_dotbaucu.sGhichu.ToString(),
                            ten_dauphieu = dm_dauphieu.ten_dauphieu.ToString(),
                            type = dm_dauphieu.type.ToString().Trim()
                        }
                    )
                    .Where(s => s.ma_dot == ma_dot).FirstOrDefault<DotBauCu>();
            }
            return dotBauCu;
        }

        public List<DotBauCu> get_dotbaucu_by_array(long[] id)
        {
            List<DotBauCu> dotBauCu = null;
            using (var ctx = new admin_voteEntities())
            {
                dotBauCu = ctx.tbl_dotbaucu
                            .Where(s => id.Contains(s.ma_dot))
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
                                iTrangThai = (int)s.iTrangThai,
                                sTenDot = s.sTendot.ToString(),
                                sGhiChu = s.sGhichu.ToString(),
                            }).ToList<DotBauCu>();
            }
            return dotBauCu;
        }

        public string them_dotbaucu(DotBauCu dbc)
        {
            long id;
            try
            {
                using (admin_voteEntities db = new admin_voteEntities())
                {
                    var dotbaucu = db.Set<tbl_dotbaucu>();
                    tbl_dotbaucu _dbc = new tbl_dotbaucu()
                    {
                        sTendot = dbc.sTenDot,
                        dThoigianbd = DateTime.ParseExact(dbc.dThoiGianBD, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture),
                        dThoigiankt = DateTime.ParseExact(dbc.dThoiGianKT, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture),
                        sNoiDung = dbc.sNoiDung,
                        iTrangThai = dbc.iTrangThai,
                        //sHinhThuc = dbc.sHinhThuc,
                        sSoPhieu = dbc.sSoPhieu,
                        iNguoiTao = dbc.iNguoiTao
                    };
                    dotbaucu.Add(_dbc);
                    db.SaveChanges();
                    id = _dbc.ma_dot;
                }
            }
            catch(InvalidCastException e)
            {
                return "them_thatbai";
            }
            return id.ToString();
        }

    }
}