using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSharpchainWebAPI.Controllers
{
    public partial class tbl_dotbaucu
    {
        public long ma_dot;
        public String sTendot { get; set; }
        public DateTime dThoigianBD { get; set; }
        public DateTime dThoigianKT { get; set; }
        public String sGhichu { get; set; }
    }
}