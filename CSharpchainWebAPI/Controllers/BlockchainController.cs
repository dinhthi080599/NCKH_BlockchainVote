using CSharpChainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CSharpChainNetwork;

namespace CSharpchainWebAPI.Controllers
{
    public class BlockchainController : BaseController
    {

        [HttpGet]
        public string Ping()
        {
            return "  Blockchain Contoller Ping";
        }

        [HttpGet]
        public int Length()
        {
            return Models.Program.blockchainServices.BlockchainLength();
        }

        [HttpGet]
        public Block GetBlock(int Id) { 
            var block = Models.Program.blockchainServices.get_block(Id);
            return block;
        }

        [HttpGet]
        public Blockchain GetBlockchain()
        {
            var blockChain = Models.Program.blockchainServices.Blockchain;
            return blockChain;
        }

        [HttpGet]
        public string TestApi()
        {
            string blockChain = "Connected";
            return blockChain;
        }

        [HttpPost]
        public void NewBlock(Block NewBlock)
        {
            Models.Program.blockchainServices.Blockchain.Chain.Add(NewBlock);
            if (!Models.Program.blockchainServices.isBlockchainValid())
            {
                Models.Program.blockchainServices.Blockchain.Chain.Remove(NewBlock);
            }
        }
    }
}