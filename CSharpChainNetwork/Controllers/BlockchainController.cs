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
		public string Ping()
		{
			return "  Blockchain Contoller Ping";
		}

		[HttpGet]
		public int Length()
		{
			return Program.blockchainServices.BlockchainLength();
		}

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
        }

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
        }

        [HttpPost]
        public string MineBlock()
        {
            Program.blockchainServices.MineBlock();
            Console.WriteLine("Mined Block!");
            return "MinedBlock";
        }

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

        [HttpPost]
        public string AddNode(String node)
        {
            string add_node = Program.nodeServices.AddNodeAPI(node);
            if(add_node == "NodeAdded")
            {
                Console.WriteLine("New node added: " + node);
            }
            return add_node;
        }

        [HttpGet]
        public List<String> NodeList()
        {
            return Program.nodeServices.Nodes;
        }
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
                        Console.WriteLine("ID: " + voterID + "was Voted elector " + electorID.ToString()+ "!");
                        return true;
                    }
                }
            }
            return false;
        }
	}
}
