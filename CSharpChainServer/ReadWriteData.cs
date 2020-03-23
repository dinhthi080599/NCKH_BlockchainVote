using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using CSharpChainModel;

namespace CSharpChainServer
{
    public class ReadWriteData
    {
        public void write(Block block)
        {
            IFormatter formatter = new BinaryFormatter();
            var dataFile = "blockchain";
            Stream stream = new FileStream(dataFile, FileMode.Append, FileAccess.Write);
            formatter.Serialize(stream, block);
            stream.Close();
        }
        public List<Block> read()
        {
            List<Block> blockchain = new List<Block>();
            IFormatter formatter = new BinaryFormatter();
            //var dataFile = System.Web.Hosting.HostingEnvironment.MapPath("~/blockchain");
            var dataFile = "blockchain";
            Stream stream = new FileStream(dataFile, FileMode.Open, FileAccess.Read);
            while (stream.Position != stream.Length)
            {
                Block block = (Block)formatter.Deserialize(stream);
                blockchain.Add(block);
            }
            stream.Close();
            return blockchain;
        }
    }
}