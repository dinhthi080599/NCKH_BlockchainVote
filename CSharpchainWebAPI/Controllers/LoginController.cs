using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.EntityFrameworkCore;
using CSharpchainWebAPI.Models;

namespace CSharpchainWebAPI.Controllers
{
    public class LoginController : ApiController
    {
        public IHttpActionResult GetAllAccount()
        {
            IList<Account> students = null;

            if (students.Count == 0)
            {
                return NotFound();
            }

            return Ok(students);
        }
        public IHttpActionResult GetAllStudents(String sTenDangNhap = "", String sMatKhau = "")
        {
            IList<DotBauCu> dotBauCu = null;
            using (var ctx = new admin_voteEntities())
            {
                dotBauCu = ctx.tbl_dotbaucu
                            .Select(s => new DotBauCu()
                            {
                                ma_dot = s.ma_dot,
                                dThoiGianBD = s.dThoigianbd.ToString(),
                                dThoiGianKT = s.dThoigiankt.ToString(),
                                sTenDot = s.sTendot.ToString(),
                                sGhiChu = s.sGhichu.ToString(),
                            }).ToList<DotBauCu>();
            }

            if (dotBauCu.Count == 0)
            {
                return NotFound();
            }

            return Ok(new { results = dotBauCu });
        }
    }
}
