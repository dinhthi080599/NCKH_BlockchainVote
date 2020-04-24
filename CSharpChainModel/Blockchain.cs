using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpChainModel
{
	public class Blockchain
	{
		public List<Block> Chain;               // list các khối
		public List<string> Nodes;              // list các node có trong mạng
		public int Difficulty;                  // độ khó (giúp tránh việc thêm block 1 cách ồ ạt)
                                                // độ khó càng lớn thì thời gian tìm ra block càng lâu
        public List<Vote> PendingVote;          // các vote chưa được tạo khối
        public int MiningReward;
        public List<Transaction> PendingTransactions;

        public Blockchain()                     // phương thức khởi tạo của blockchain
		{
			this.Chain = new List<Block>();
			this.Chain.Add(CreateGenesisBlock());
			this.Nodes = new List<string>();
            this.PendingVote = new List<Vote>();
            this.MiningReward = 100;
            this.PendingTransactions = new List<Transaction>();
            this.Difficulty = 5;
        }

		private Block CreateGenesisBlock()      // tạo khối nguyên thủy
		{
			Block genesis = new Block(new DateTime(2000, 01, 01), new List<Vote>(), "0");
			return genesis;
		}
    }



}
