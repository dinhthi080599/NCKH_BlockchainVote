using CSharpChainModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSharpChainServer
{
	public class NodeServices
	{
		private Blockchain blockchain;
        List<string> list_node = new List<string>();
        public List<string> Nodes
		{
			get
			{
				return blockchain.Nodes;
			}
		}

		public NodeServices(Blockchain Blockchain)
		{
			this.blockchain = Blockchain;
		}


        public void AddNode(string nodeUrl)
        {
            if (!blockchain.Nodes.Contains(nodeUrl))
            {
                blockchain.Nodes.Add(nodeUrl);
            }
        }

        public async Task add_list_nodeAsync()
        {
            
            using (var client = new HttpClient())
            {
                try
                {
                    string url = "http://localhost:44394/api/nodelist/index";
                    var response = client.GetAsync(url).Result;
                    list_node = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
                catch (Exception)
                {
                    throw;
                }
            }
            foreach (var node in list_node)
            {
                if (!blockchain.Nodes.Contains(node))
                {
                    blockchain.Nodes.Add(node);
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            //var response = await client.PostAsJsonAsync<Block>("http://localhost:808" + node + "/api/blockchain/newblock", node);
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("  " + ex.Message);
                            Console.ResetColor();
                        }
                    }
                }
            }
        }
        public List<string> AddNodeAPI(string nodeUrl)
        {
            bool add = false;
            if (!blockchain.Nodes.Contains(nodeUrl))
            {
                blockchain.Nodes.Add(nodeUrl);
                add = true;   
            }
            add_list_nodeAsync();
            return list_node;
        }
        public void RemoveNode(string nodeUrl)
		{
			blockchain.Nodes.Remove(nodeUrl);
		}
	}
}
