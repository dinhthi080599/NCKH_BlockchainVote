using CSharpChainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CSharpChainServer;

namespace CSharpChainNetwork.Controllers
{
	public class BlockchainController : ApiController
	{
		[HttpGet]
		public string Ping() // kiểm tra kết nối
		{
			return "Blockchain Contoller Ping";
		}
		[HttpGet]
		public int Length()
		{
			return Program.blockchainServices.BlockchainLength();
		} // lấy độ dài blockchain
		[HttpGet]
		public Block GetBlock(int Id)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("");
			Console.WriteLine($"  API notification received: blockchain-getBlock {Id} ");
			Console.WriteLine("    Get block.");
			var block = Program.blockchainServices.get_block(Id);
			Console.ResetColor();
			Program.ShowCommandLine();
			return block;
        } // lấy khối chỉ định
        [HttpGet]
        public Blockchain GetBlockchain()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("");
            Console.WriteLine($"  API notification received: blockchain-getBlockchain() ");
            Console.WriteLine("    Get entire blockchain.");
            var blockChain = Program.blockchainServices.Blockchain;
            Console.ResetColor();
            Program.ShowCommandLine();
            return blockChain;
        } // lấy toàn bộ blockchain
        [HttpGet]
        public List<String> NodeList()
        {
            return Program.nodeServices.Nodes;
        } // danh sách các node trong mạng
        
        [HttpGet]
        public int BlockchainLength()
        {
            return Program.blockchainServices.Blockchain.Chain.Count();
        }
        [HttpGet]
        public Block Block(int id)
        {
            return Program.blockchainServices.Blockchain.Chain[id];
        }

        [HttpPost]
        public Boolean checkVoted(string voterID, int electorID)
        {
            List<Block> chain = Program.blockchainServices.Blockchain.Chain;
            foreach(Block block in chain)
            {
                foreach(Vote vote in block.Vote)
                {
                    if(vote.voterID == voterID && vote.electorID == electorID)
                    {
                        //Console.WriteLine("ID: " + voterID + "was Voted elector " + electorID.ToString()+ "!");
                        return true;
                    }
                }
            }
            return false;
        }
        [HttpGet]
        public Dictionary<string, int> number_of_vote(int electorID)
        {
            Digital_Signature digital_Signature = new Digital_Signature();
            Dictionary<string, int> number_of_vote = new Dictionary<string, int>();
            foreach (Block block in Program.blockchainServices.Blockchain.Chain)
            {
                if (block.Vote.Count > 0)
                {
                    foreach (Vote vote in block.Vote)
                    {
                        if (vote.electorID == electorID)
                        {
                            if (digital_Signature.VerifySignature(vote.vote_tostring(), vote.public_key, vote.signature))// xác thực chữ ký
                            {
                                if (number_of_vote.ContainsKey(vote.voteParty.ToString()))
                                {
                                    number_of_vote[vote.voteParty.ToString()]++;
                                }
                                else
                                {
                                    number_of_vote[vote.voteParty.ToString()] = 1;
                                }
                            }
                        }
                    }
                }
            }
            return number_of_vote;
        }
        [HttpPost]
        public string AddNode(String node)
        {
            // Program.start_newhost(node);
            List<string> add_node = Program.nodeServices.AddNodeAPI(node);
            Program.NetworkBlockchainUpdate();
            foreach(var _node in add_node)
            {
                Program.NetworkRegister(_node);
            }
            Program.node_id = node;
            //if (add_node == "NodeAdded")
            //{
            //    Console.WriteLine("Connected: " + node);
            //}
            return "true";
        }
        [HttpPost]
        public string MineBlock()
        {
            Block block = Program.blockchainServices.MineBlock();
            Program.NetworkBlockchainMine(block);
            Console.WriteLine("Mined Block!");
            return "MinedBlock";
        } // tạo khối
        [HttpPost]
        public void NewBlock(Block NewBlock)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("");
            Console.WriteLine($"  API notification received: blockchain-newBlock {NewBlock.Hash} ");
            Console.WriteLine("    Add new block.");

            Program.blockchainServices.Blockchain.Chain.Add(NewBlock);
            // check if the blockchain is valid
            if (!Program.blockchainServices.isBlockchainValid())
            {
                Console.WriteLine("    Blockchain with new block added is not valid. Undoing block.");
                Program.blockchainServices.Blockchain.Chain.Remove(NewBlock);
                Console.WriteLine("    Try refresing with the longest blockchain");
            }
            Console.ResetColor();
            Program.ShowCommandLine();
        }
    }
}
