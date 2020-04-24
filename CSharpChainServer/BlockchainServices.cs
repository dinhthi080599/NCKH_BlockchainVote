using CSharpChainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpChainServer;
using System.Net.Http;

namespace CSharpChainServer
{
	public class BlockchainServices
	{
		public Blockchain Blockchain
		{
			get
			{
				return blockchain;
			}
		}
        private Blockchain blockchain;
        public BlockchainServices()
        {
            ReadWriteData rw = new ReadWriteData();     // phương thức giúp ghi dữ liệu vào tệp
            blockchain = new Blockchain();              // khởi tạo đối tượng blockchain
            Block genesisBlock = blockchain.Chain[0];   // sau khi khởi tạo thì chuỗi sẽ có khối nguyên thủy
            // khởi tạo đối tượng block service chứa các đoạn mã sinh ra hash cho khối
            BlockServices blockServices = new BlockServices(genesisBlock);
            // lấy mã cho khối
            string genesisBlockHash = blockServices.BlockHash();  
            blockchain.Chain[0].Hash = genesisBlockHash;
            // tự lưu khối vào file
            rw.write(blockchain.Chain[0]);
        }
        public BlockchainServices(List<Block> chain)
        {
            this.blockchain = new Blockchain();
            this.blockchain.Chain = chain;
        }
        public void UpdateWithLongestBlockchain ()
		{
			string longestBlockchainNode = "";
			int maxBlockchainLength = 0;

		}
        public Block LatestBlock()
		{
			return blockchain.Chain.Last();
		}
        public Block get_block(int index)
		{
            if (BlockchainLength() <= index)
            {
                return blockchain.Chain[0];
            }
			return blockchain.Chain[index];
		}
        public int BlockchainLength()
		{
			return blockchain.Chain.Count();
        }
        public void AddTransaction(Transaction transaction)
        {
            blockchain.PendingTransactions.Add(transaction);
        }
        public void AddVote(Vote vote)
        {
            blockchain.PendingVote.Add(vote);
        }
        public List<Transaction> PendingTransactions()
		{
			return blockchain.PendingTransactions;
		}
  //      public Block MineBlock(string miningRewardAddress)
		//{
		//	// add mining reward transaction to block
		//	Transaction trans = new Transaction("SYSTEM", miningRewardAddress, blockchain.MiningReward, "Mining reward");
		//	blockchain.PendingTransactions.Add(trans);
  //      	Block block = new Block(DateTime.Now, blockchain.PendingTransactions, LatestBlock().Hash);
		//	var blockServices = new BlockServices(block);
		//	blockServices.MineBlock(blockchain.Difficulty);
		//	blockchain.Chain.Add(block);
  //          //clear pending transactions (all pending transactions are in a block
		//	blockchain.PendingTransactions = new List<Transaction>();
		//	return block;
  //      }
        public Block MineBlock()
        {
            ReadWriteData rw = new ReadWriteData();
            Block block = new Block(DateTime.Now, blockchain.PendingVote, LatestBlock().Hash);
            var blockServices = new BlockServices(block);
            blockServices.MineBlock(blockchain.Difficulty);
            blockchain.Chain.Add(block);
            blockchain.PendingTransactions = new List<Transaction>();
            blockchain.PendingVote = new List<Vote>();
            rw.write(block);
            return block;
        }
        public bool isBlockchainValid()
		{
			for (long i = 1; i < blockchain.Chain.LongCount(); i++)
			{
				Block currentBlock = blockchain.Chain[(int)i];
				Block previousBlock = blockchain.Chain[(int)i - 1];
                BlockServices blockServices = new BlockServices(currentBlock);
				if (currentBlock.Hash != blockServices.BlockHash())
				{
					return false;
				}
                if (currentBlock.PreviousHash != previousBlock.Hash)
				{
					return false;
				}
				blockServices = null;
			}
			return true;
        }
        public decimal Balance(string address)
        {
            decimal balance = 0;
            foreach (Block block in blockchain.Chain)
            {
                foreach (Transaction transaction in block.Transactions)
                {
                    if (transaction.SenderAddress == address)
                    {
                        balance = balance - transaction.Amount;
                    }
                    if (transaction.ReceiverAddress == address)
                    {
                        balance = balance + transaction.Amount;
                    }
                }
            }
            return balance;
        }
    }
}
