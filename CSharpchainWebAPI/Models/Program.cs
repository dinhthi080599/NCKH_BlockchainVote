using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSharpChainModel;
using CSharpChainServer;

namespace CSharpchainWebAPI.Models
{
    public class Program
    {
        public static BlockchainServices blockchainServices = new BlockchainServices();
        public static NodeServices nodeServices = new NodeServices(blockchainServices.Blockchain);
    }
}