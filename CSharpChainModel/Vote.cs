using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSharpChainModel
{
    [Serializable]
    public class Vote
    {
        public String voterID { get; set; }
        public int voteParty { get; set; }
        public int electorID { get; set; }
    }
}