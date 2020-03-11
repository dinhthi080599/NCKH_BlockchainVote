
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpChainModel
{
	public class Block
	{
		public string PreviousHash;
		public DateTime TimeStamp;
		public List<Transaction> Transactions;
        public List<Vote> Vote;
		public string Hash;
		public long Nonce;
        public Block(DateTime timeStamp, List<Transaction> transactions, string previousHash)
        {
            this.Vote = new List<Vote>();
            this.TimeStamp = timeStamp;
            this.PreviousHash = previousHash;
            this.Transactions = transactions;
            this.Hash = "";
            this.Nonce = 0;
        }
        public Block(DateTime timeStamp, List<Vote> Vote, string previousHash)
        {
            this.TimeStamp = timeStamp;
            this.PreviousHash = previousHash;
            this.Transactions = new List<Transaction>();
            this.Vote = Vote;
            this.Hash = "";
            this.Nonce = 0;
        }
    }
}
