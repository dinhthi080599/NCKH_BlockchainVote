using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CSharpChainServer;
using CSharpChainModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CSharpchainWebAPI.Controllers;

namespace CSharpchainWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        static ReadWriteData wd = new ReadWriteData();
        static List<Block> blockchain = wd.read();
        static BlockchainServices blockchainServices;
        static NodeServices nodeServices;
        static string baseAddress = "http://localhost:44394/";
        protected void Application_Start()
        {
            if(blockchain.Count() == 0)
            {
                blockchainServices = new BlockchainServices();
            }
            else
            {
                blockchainServices = new BlockchainServices(blockchain);
            }
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        public static int CommandBlockchainLength() // lấy độ dài hiện tại của block
        {
            var length = blockchainServices.BlockchainLength();
            return length;
        }

        public static Block CommandBlock(int Index = 0) // lấy block được chỉ định
        {
            Block block = blockchainServices.get_block(Index);
            return block;
        }// cần bổ sung thêm kiểm tra nếu mà block k tồn tại thì return block 0 hoặc báo về block k tồn tại.

        public static void CommandBlockchainMine(string RewardAddress, Block block) // tạo blockchain mới
        {
            //Console.WriteLine($"  Mining new block... Difficulty {blockchainServices.Blockchain.Difficulty}.");
            blockchainServices.MineBlock(RewardAddress, block);
            //Console.WriteLine($"  Block has been added to blockchain. Blockhain length is {blockchainServices.BlockchainLength().ToString()}.");
            Block lastestBlock = blockchainServices.LatestBlock();
            NetworkBlockchainMine(lastestBlock);
        } // cần cập nhật rewardaddress

        static async Task<string> NetworkBlockchainMine(Block NewBlock)
        {
            // automatically notify node you are registering about this node
            using (var client = new HttpClient())
            {
                try
                {
                    // iterate all registered nodes
                    foreach (string node in nodeServices.Nodes)
                    {
                        //Console.WriteLine($"  Initiating API call: calling {node} blockchain-newBlock {NewBlock.Hash}");
                        //var response = await client.PostAsJsonAsync<Block>(node + "api/blockchain/newblock", NewBlock);
                        Console.WriteLine($"  Initiating API call: calling {node} blockchain-newBlock {NewBlock.Hash}");
                        var response = await client.PostAsJsonAsync<Block>("https://localhost:44394/api/blockchain/newblock", NewBlock);
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  " + ex.Message);
                    Console.ResetColor();
                }
            }
            return "ok";
        }

        public static void CommandNodeAdd(string NodeUrl) // thêm node mới
        {
            if (!NodeUrl.EndsWith("/")) NodeUrl += "/";
            nodeServices.AddNode(NodeUrl);
            Console.WriteLine($"  Node {NodeUrl} added to list of blockchain peer nodes.");
            NetworkRegister(NodeUrl);
            CommandBlockchainUpdate();
            Console.WriteLine("");
        }// cần cập nhật node thành 

        static async Task<string> NetworkRegister(string NewNodeUrl)
        {
            // automatically notify node you are registering about this node
            using (var client = new HttpClient())
            {
                try
                {
                    Console.WriteLine($"  Initiating API call: calling {NewNodeUrl} node-register {baseAddress}");

                    var content = new FormUrlEncodedContent(
                        new Dictionary<string, string>
                        {
                            {"" , baseAddress}
                        }
                    );

                    Console.Error.WriteLine(content.ToString());

                    var response = await client.PostAsync(NewNodeUrl + "api/network/register", content);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  " + ex.Message);
                    Console.ResetColor();
                }
                return "ok";
            }
        }
        
        static void CommandBlockchainUpdate()
        {
            //Console.WriteLine($"  Updating blockchain with the longest found on network.");
            NetworkBlockchainUpdate();
        }

        static async void NetworkBlockchainUpdate()
        {
            int maxLength = 0;
            string maxNode = "";

            // find the node with the longest blockchain
            using (var client = new HttpClient())
            {
                try
                {

                    // iterate all registered nodes
                    foreach (string node in nodeServices.Nodes)
                    {
                        Console.Write($"  Initiating API call: calling {node} blockchain-length.");
                        var response = await client.GetAsync(node + "api/blockchain/length");

                        int newLength = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                        Console.Write("New Length: " + newLength.ToString());
                        if (newLength > maxLength)
                        {
                            maxNode = node;
                            maxLength = newLength;
                        }
                        Console.WriteLine(response);
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  " + ex.Message);
                    Console.ResetColor();
                }

                Console.WriteLine($"    Max blockchain length found on {maxNode} with length {maxLength}.");
                if (blockchainServices.BlockchainLength() >= maxLength)
                {
                    Console.WriteLine($"    No blockchain found larger than existing one.");
                    Console.WriteLine();
                    return;
                }


                // get missing blocks
                try
                {
                    for (int i = blockchainServices.BlockchainLength(); i < maxLength; i++)
                    {
                        Block newBlock;
                        Console.WriteLine($"      Requesting block {i} from {maxNode}...");
                        var response = await client.GetAsync(maxNode + $"api/blockchain/getblock?id={i}");
                        newBlock = JsonConvert.DeserializeObject<Block>(await response.Content.ReadAsStringAsync());
                        Console.Write(await response.Content.ReadAsStringAsync());

                        blockchainServices.Blockchain.Chain.Add(newBlock);
                        if (!blockchainServices.isBlockchainValid())
                        {
                            blockchainServices.Blockchain.Chain.Remove(newBlock);
                            Console.WriteLine($"    After adding block {i} blockchain is not valid anymore. Canceling...");
                            break;
                        }

                    }

                }
                catch (Exception)
                {

                    throw;
                }

                Console.WriteLine($"    Updated!");
                Console.WriteLine();
            }
        }
        static List<string> CommandListNodes() // trả về tất cả các node đang được kết nối :)
        {
            return nodeServices.Nodes;
        }


        static void CommandTransactionsAdd(string SenderAddress, string ReceiverAddress, string Amount, string Description) // thêm transaction
        {
            Transaction transaction = new Transaction(SenderAddress, ReceiverAddress, Decimal.Parse(Amount), Description);
            blockchainServices.AddTransaction(transaction);
            NetworkTransactionAdd(transaction);
        }

        static async Task<string> NetworkTransactionAdd(Transaction Transaction)
        {
            // automatically notify node you are registering about this node
            using (var client = new HttpClient())
            {
                try
                {
                    // iterate all registered nodes
                    foreach (string node in nodeServices.Nodes)
                    {
                        Console.WriteLine($"  Initiating API call: calling {node} transaction-add {Transaction.Description}");
                        var response = await client.PostAsJsonAsync<Transaction>(node + "api/transactions/add", Transaction);
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  " + ex.Message);
                    Console.ResetColor();
                }
                return "ok";
            }
        }
        static List<Transaction> CommandListPendingTransactions() // trả về các transaction chưa được giao dịch :)
        {
            return blockchainServices.Blockchain.PendingTransactions;
        }

        public static Dictionary<string, int> number_of_vote(int electorID) // get vote
        {
            Dictionary<string, int> number_of_vote = new Dictionary<string, int>();
            Block[] blockchain = new Block[WebApiApplication.CommandBlockchainLength()];
            for (var i = 0; i < WebApiApplication.CommandBlockchainLength(); i++)
            {
                blockchain[i] = WebApiApplication.CommandBlock(i);
            }
            foreach (Block block in blockchain)
            {
                if (block.Vote.Count > 0)
                {
                    foreach (Vote vote in block.Vote)
                    {
                        if (vote.electorID == electorID)
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
            return number_of_vote;
        }

        public static HashSet<long> list_voted_byID(int id)
        {
            HashSet<long> list_voted_byID = new HashSet<long>();
            Block[] blockchain = new Block[WebApiApplication.CommandBlockchainLength()];
            for (var i = 0; i < WebApiApplication.CommandBlockchainLength(); i++)
            {
                blockchain[i] = WebApiApplication.CommandBlock(i);
            }
            foreach (Block block in blockchain)
            {
                if (block.Vote.Count > 0)
                {
                    foreach (Vote vote in block.Vote)
                    {
                        if(vote.voterID == id.ToString())
                        {
                            list_voted_byID.Add(vote.electorID); 
                        }
                    }
                }
            }
            return list_voted_byID; 
        }
    }
}
