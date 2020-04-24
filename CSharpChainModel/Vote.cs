using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSharpChainModel
{
    [Serializable]
    public class Vote
    {
        public String voterID;          // mã cử tri
        public int voteParty;           // mã ứng cử viên
        public int electorID;           // mã cuộc bầu cử
        public String public_key;       // khóa công khai dùng để xác thực chữ ký số
        public String signature;        // chữ ký
        public String vote_tostring()   // phương thức chuyển đối tượng thành string
        {
            return $"{this.voterID}:{this.voteParty}:{this.electorID}:{this.public_key}";
        }
    }
}