using CSharpChainModel;
using CSharpChainServer;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace CSharpChainNetwork
{
	static class Program1
	{
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static string baseAddress;
        public static string node_id;
		public static BlockchainServices blockchainServices;
		public static NodeServices nodeServices;
		static bool useNetwork = true;
		public static void ShowCommandLine()
		{
			Console.WriteLine("");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("CSharpChain> ");
			Console.ResetColor();
		}
		static void Main1(string[] args)
        {
            #region
            Console.OutputEncoding = Encoding.UTF8;
            // ShowWindow(handle, SW_HIDE);
            //if(args.Length == 0)
            //{
            //args = new string[1];
            //Random rnd = new Random();
            //args[0] = @"http://localhost:" + rnd.Next(2000, 8000).ToString();
            //sargs[0] = @"http://localhost:44312";

            //}
            //baseAddress = args[0];
            //if (!baseAddress.EndsWith("/")) baseAddress += "/";
            //
            #endregion
            // Start OWIN host 
            baseAddress = "http://localhost:8081/";
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                #region
                ReadWriteData rw = new ReadWriteData();
                List<Block> chain = new List<Block>();
                chain = rw.read();
                Console.WriteLine("CSharpChain node đang chạy trên " + baseAddress);
                if (chain.Count() > 0)
                {
                    blockchainServices = new BlockchainServices(chain);
                }
                else
                {
                    blockchainServices = new BlockchainServices();
                }
                nodeServices = new NodeServices(blockchainServices.Blockchain);
				string commandLine;
				do
				{
                    ShowCommandLine();
					commandLine = Console.ReadLine().ToLower();
					commandLine += " ";
                    var command = commandLine.Split(' ');
					switch (command[0])
					{
						case "quit":
						case "q":
							commandLine = "q";
                            break;
						case "trogiup":
						case "?":
							CommandHelp();
							break;

						case "node-add":
						case "na":
                            CommandNodeAdd("http://localhost:" + command[1].ToString());
							break;

						case "node-remove":
						case "nr":
                            CommandNodeRemove("http://localhost:" + command[1].ToString());
                            break;
						case "nodes-list":
						case "nl":
							CommandListNodes(nodeServices.Nodes);
							break;

						case "transactions-add":
						case "ta":
							CommandTransactionsAdd(command[1], command[2], command[3], command[4]);
							break;

						case "transactions-pending":
						case "tp":
							CommandListPendingTransactions(blockchainServices.Blockchain.PendingTransactions);
							break;

						case "blockchain-mine":
						case "bm":
							CommandBlockchainMine(command[1]);
							break;

						case "bv":
						case "blockchain-valid":
							CommandBlockchainValidity();
							break;

						case "blockchain-length":
						case "bl":
							CommandBlockchainLength();
							break;

						case "block":
						case "b":
							GetBlock(int.Parse(command[1]));
							break;

						case "balance-get":
						case "bal":
							CommandBalance(command[1]);
							break;

						case "blockchain-update":
						case "update":
						case "bu":
							CommandBlockchainUpdate();
							break;

						default:
							Console.WriteLine("Lệnh sai...");
							Console.WriteLine("");
							break;
					}
				} while (commandLine != "q");
                #endregion
            }
        }
		#region Commands 

		static void CommandListNodes(List<string> Nodes)
		{
			foreach (string node in Nodes)
			{
				Console.WriteLine($"  Node: {node}");
			}
			Console.WriteLine("");
		}

		static void CommandListPendingTransactions(List<Transaction> PendingTransactions)
		{
			foreach (Transaction transaction in PendingTransactions)
			{
				Console.WriteLine($"  Transaction: {transaction.Amount} from {transaction.SenderAddress} to {transaction.ReceiverAddress} with description {transaction.Description}");
			}
			Console.WriteLine("");
		}

		static void CommandNodeAdd(string NodeUrl)
		{
			if (!NodeUrl.EndsWith("/")) NodeUrl += "/";
			nodeServices.AddNode(NodeUrl);
			Console.WriteLine($"  Node {NodeUrl} added to list of blockchain peer nodes.");
			if (useNetwork)
			{
				NetworkRegister(NodeUrl);
				CommandBlockchainUpdate();
			}
			Console.WriteLine("");
		}

		static void CommandHelp()
		{
            Console.OutputEncoding = Encoding.UTF8;
			Console.WriteLine("Commands:");
			Console.WriteLine("h, help = Hỗ trợ.");
			Console.WriteLine("q, quit = Thoát khỏi chương trình.");
			Console.WriteLine("na, node-add [port] = Kết nối đến 1 node khác.");
			Console.WriteLine("nr, node-remove [port] = Ngắt kết nối với 1 node hiện có.");
			Console.WriteLine("nl, nodes-list = Danh sách các node đã kết nối.");
			Console.WriteLine("ta, transaction-add [dia_chi_nguoi_gui] [dia_chi_nguoi_nhan] [so_tien] [mieu_ta] = Tạo mới giao dịch.");
			Console.WriteLine("np, transactions-pending = Danh sách các giao dịch chưa được tạo khối.");
			Console.WriteLine("bm, blockchain-mine [rewardAddress] = Tạo khối cho các giao dịch đang chờ xử lý,");
			Console.WriteLine("bv, blockchain-valid = Xác thực blockchain.");
			Console.WriteLine("bl, blockchain-length = Số lượng khối trong blockchain.");
			Console.WriteLine("b, block [index] = Liệt kê thông tin của khối được chỉ định.");
			Console.WriteLine("bu, update, blockchain-update = Cập nhật blockchain lên mạng lâu nhất??.");
			Console.WriteLine("bal, balance-get [address] = Lấy số dư cho các địa chỉ được chỉ định??.");
			Console.WriteLine();

		}

		static void CommandBlockchainUpdate()
		{
			Console.WriteLine($"Cập nhật blockchain theo node có blockchain dài nhất trong mạng.");
			if (useNetwork)
			{
				NetworkBlockchainUpdate();
			}
			Console.WriteLine("");
		}

		static void CommandNodeRemove(string NodeUrl)
		{
			if (!NodeUrl.EndsWith("/")) NodeUrl += "/";
			nodeServices.RemoveNode(NodeUrl);
			Console.WriteLine($"  Node {NodeUrl} removed to list of blockchain peer nodes.");
			Console.WriteLine("");
		}

		static void CommandTransactionsAdd(string SenderAddress, string ReceiverAddress, string Amount, string Description)
		{
			Transaction transaction = new Transaction(SenderAddress, ReceiverAddress, Decimal.Parse(Amount), Description);
			blockchainServices.AddTransaction(transaction);
			Console.WriteLine($"  {Amount} from {SenderAddress} to {ReceiverAddress} transaction added to list of pending transactions.");
			Console.WriteLine("");

			if (useNetwork)
			{
				NetworkTransactionAdd(transaction);
			}
			Console.WriteLine("");
		}

		static void CommandBlockchainMine(string RewardAddress)
		{
			Console.WriteLine($"  Mining new block... Difficulty {blockchainServices.Blockchain.Difficulty}.");
			//blockchainServices.MineBlock(RewardAddress);

			Console.WriteLine($"  Block has been added to blockchain. Blockhain length is {blockchainServices.BlockchainLength().ToString()}.");
			Console.WriteLine("");
			if (useNetwork)
			{
				NetworkBlockchainMine(blockchainServices.LatestBlock());
			}
			Console.WriteLine("");
		}

		static void CommandBlockchainValidity()
		{
			var result = blockchainServices.isBlockchainValid();
			if (result == true)
			{
				Console.WriteLine($"Blockchain có hiệu lực.");
			}
			else
			{
				Console.WriteLine($"Blockchain không có hiệu lực.");
			}
			Console.WriteLine("");
		}

		static void CommandBlockchainLength()
		{
			var length = blockchainServices.BlockchainLength();
			Console.WriteLine($"  Blockchain length is {length.ToString()}.");
			Console.WriteLine("");
		}

		static void GetBlock(int Index)
		{
			var block = blockchainServices.get_block(Index);
			Console.WriteLine($"Block {Index}:");
			Console.WriteLine($"Hash: {block.Hash}");
            Console.WriteLine($"Nonce: {block.Nonce}");
            Console.WriteLine($"TimeStamp: {block.TimeStamp}");
            Console.WriteLine($"Previous hash: {block.PreviousHash}");
			Console.WriteLine($"Vote: {block.Vote.Count}");
		}

		static void CommandBalance(string Address)
		{
			var balance = blockchainServices.Balance(Address);
			Console.WriteLine($"  Balance for address {Address} is {balance.ToString()}.");
			Console.WriteLine("");
		}
		#endregion

		#region NetworkSend

		public static async void NetworkRegister(string NewNodeUrl)
		{
			// automatically notify node you are registering about this node
			using (var client = new HttpClient())
			{
				try
				{
					//Console.WriteLine($"  Initiating API call: calling {NewNodeUrl} node-register {baseAddress}");

					var content = new FormUrlEncodedContent(
						new Dictionary<string, string>
						{
							{"" , baseAddress}
						}
					);

                    Console.Error.WriteLine(content.ToString());

					var response = await client.PostAsync("http://localhost:" +NewNodeUrl + "api/network/register", content);
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("  " + ex.Message);
					Console.ResetColor();
				}
			}
		}

		static async void NetworkUnregister(string UnregisterNodeUrl)
		{
			// automatically notify node you are registering about this node
			using (var client = new HttpClient())
			{
				try
				{
					Console.WriteLine($"  Initiating API call: calling {UnregisterNodeUrl} node-unregister {baseAddress}");

					var content = new FormUrlEncodedContent(
						new Dictionary<string, string>
						{
							{"" , baseAddress}
						}
					);

					var response = await client.PostAsync(UnregisterNodeUrl + "api/network/unregister", content);
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("  " + ex.Message);
					Console.ResetColor();
				}
			}
		}

		static async void NetworkTransactionAdd(Transaction Transaction)
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
			}
		}

		public static async void NetworkBlockchainUpdate()
		{
			int maxLength = 0;
			string maxNode = "";
			// tìm node có blockchain dài nhất
			using (var client = new HttpClient())
			{
				try
				{
					// duyệt qua tất cả các node đã đăng ký
					foreach (string node in nodeServices.Nodes)
					{
                        if(node == node_id)
                        {
                            continue;
                        }
						var response = await client.GetAsync("http://localhost:808"+node + "/api/blockchain/length");
						int newLength = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                        //Console.Write("New Length: " + newLength.ToString());
                        //Console.Write($"Node: {node} có độ dài: {newLength}");
                        if (newLength > maxLength)
						{
							maxNode = node;
							maxLength = newLength;
						}
						// Console.WriteLine(response);
					}
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("  " + ex.Message);
					Console.ResetColor();
				}

				Console.WriteLine($"Blockchain dài nhất được tìm thấy tại node {maxNode} có chiều dài: {maxLength}.");
				if (blockchainServices.BlockchainLength() >= maxLength)
				{
					Console.WriteLine($"Không tìm thấy node nào có độ dài lớn hơn.");
					Console.WriteLine();
					return;
				}

                // get missing blocks
                int lengthBlock = blockchainServices.BlockchainLength();
                try
				{
					for (int i = lengthBlock; i < maxLength; i++)
					{
                        ReadWriteData rWD = new ReadWriteData();
						Block newBlock;
						Console.WriteLine($"Get block {i} tại node: {maxNode}...");
						var response = await client.GetAsync("http://localhost:808" + maxNode + $"/api/blockchain/getblock?id={i}");
						newBlock = JsonConvert.DeserializeObject<Block>(await response.Content.ReadAsStringAsync());
                        // Console.Write(await response.Content.ReadAsStringAsync());
                        blockchainServices.Blockchain.Chain.Add(newBlock);
						if (!blockchainServices.isBlockchainValid())
						{
							blockchainServices.Blockchain.Chain.Remove(newBlock);
							Console.WriteLine($"Sau khi thêm block[{i}] blockchain không còn có hiệu lực thêm nữa. Hủy cập nhật...");
							break;
						}
                        else
                        {
                            rWD.write(newBlock);
                        }
					}
				}
				catch (Exception)
				{

					throw;
				}
				Console.WriteLine($"Đã cập nhật: " + (maxLength - lengthBlock) + " khối");
			}
		}


		public static async void NetworkBlockchainMine(Block NewBlock)
		{
			using (var client = new HttpClient())
			{
				try
				{
					// lặp qua tất cả các node có trong mạng
					foreach (string node in nodeServices.Nodes)
					{
						Console.WriteLine($"API: gọi đến node {node} thêm khối mới {NewBlock.Hash}");
						var response = await client.PostAsJsonAsync<Block>("http://localhost:808" + node + "/api/blockchain/newblock", NewBlock);
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
		}
        #endregion
    }
}

