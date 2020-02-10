using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSharpchainWebAPI.Models
{
    public class ElectionStatus
    {

        public long ma_dm_trangthai_dotbaucu { get; set; }
        public string tenTrangThaiDotBauCU { get; set; }
        public string icon{ get; set; }
        public string title { get; set; }

        public Dictionary<string, ElectionStatus> get_trangthai_dotbaucu()
        {
            List<ElectionStatus> trangThaiDBC = null;
            using (var ctx = new admin_voteEntities())
            {
                trangThaiDBC = ctx.dm_trangthai_dotbaucu
                            .Select(s => new ElectionStatus()
                            {
                                ma_dm_trangthai_dotbaucu = s.ma_dm_trangthai_dotbaucu,
                                tenTrangThaiDotBauCU = s.tenTrangThaiDotBauCu.ToString(),
                                icon = s.icon.ToString(),
                                title = s.title.ToString(),
                            }).ToList<ElectionStatus>();
            }
            var dict = new Dictionary<string, ElectionStatus>();
            foreach (var ES in trangThaiDBC)
            {
                dict[ES.ma_dm_trangthai_dotbaucu.ToString()] = ES;
            }
            return dict;
        }

    }
}