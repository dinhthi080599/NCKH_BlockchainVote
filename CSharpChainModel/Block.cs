
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpChainModel
{
    [Serializable]
    public class Block
	{
		public string PreviousHash;             // hash của khối truocứ
		public DateTime TimeStamp;              // đấu thời gian
        public List<Vote> Vote;                 // list các phiếu bầu
		public string Hash;                     // hash
		public long Nonce;                      // bằng chứng công việc
        public List<Transaction> Transactions;

        public Block(DateTime timeStamp, List<Vote> Vote, string previousHash) // phương thức khởi tạo
        {
            this.TimeStamp = timeStamp;
            this.PreviousHash = previousHash;
            this.Transactions = new List<Transaction>();
            this.Vote = Vote;
            this.Hash = "";
            this.Nonce = 0;
        }


        //public Block(DateTime timeStamp, List<Transaction> transactions, string previousHash)
        //{
        //    this.Vote = new List<Vote>();
        //    this.TimeStamp = timeStamp;
        //    this.PreviousHash = previousHash;
        //    this.Transactions = transactions;
        //    this.Hash = "";
        //    this.Nonce = 0;
        //}
    }
}
