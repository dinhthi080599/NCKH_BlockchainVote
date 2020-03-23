using CSharpChainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CSharpChainNetwork.Controllers
{
    public class VoteController : ApiController
    {
        [HttpGet]
        public string Ping()
        {
            return "  Transactions Contoller Ping";
        }

        [HttpPost]
        public string VoteAdd(List<Vote> list_vote)
        {
            int count_list = list_vote.Count();
            foreach(Vote vote in list_vote)
            {
                Program.blockchainServices.AddVote(vote);
            }
            Console.WriteLine("Added: " + count_list.ToString() + " vote");
            return count_list.ToString() + "added";
        }

        [HttpGet]
        public List<Vote> GetPenddingVote()
        {
            return Program.blockchainServices.Blockchain.PendingVote;
        }
        [HttpGet]
        public Dictionary<int, int> result_of_vote(int electorID)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            List<Block> chain = Program.blockchainServices.Blockchain.Chain;
            foreach(Block block in chain)
            {
                foreach(Vote vote in block.Vote)
                {
                    if(vote.electorID == electorID)
                    {
                        if(result.ContainsKey(vote.voteParty))
                        {
                            result[vote.voteParty]++;
                        }
                        else
                        {
                            result[vote.voteParty] = 1;
                        }
                    }
                }
            }
            return result;
        }

    }
}
