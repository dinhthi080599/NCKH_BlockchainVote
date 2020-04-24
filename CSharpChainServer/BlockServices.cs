using CSharpChainModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CSharpChainServer
{
	public class BlockServices
	{
		private Block block;
		public BlockServices(Block Block)
		{
			this.block = Block;
		}
		private string ByteArrayToString(byte[] ba)
		{
			return BitConverter.ToString(ba).Replace("-", "");
		}
		//public string BlockHash()
		//{
  //          string hashString = "";
  //          // chuyển khối thành string
  //          hashString = block.PreviousHash 
  //                      + block.TimeStamp.ToString() 
  //                      + JsonConvert.SerializeObject(block.Vote, Formatting.Indented) 
  //                      + block.Nonce; 
  //          // mã hóa chuỗi thành 
  //          byte[] hashBytes = Encoding.ASCII.GetBytes(hashString);
  //          Byte[] hashResult = CryptoService.GetHash(hashBytes);
  //          using (var algorithm = SHA256.Create())
  //          {
  //              hashBytes = algorithm.ComputeHash(hashBytes);
  //          }
  //          return ByteArrayToString(hashResult);
		//}
        public void MineBlock(int difficulty)
		{
			string startsWith = "";
			for (int n = 0; n < difficulty; n++)
			{
				startsWith += "0";
			}
            // đến khi nào mã hash thỏa mãn điều kiện của PoW 
            // đạt đủ số lượng ký tự 0 ở đầu cho tương ứng với difficulty
			while (block.Hash == "" || block.Hash.Substring(0, difficulty) != startsWith)
			{
				block.Nonce = block.Nonce + 1;
				block.Hash = BlockHash();
			};
		}

        #region
        public string BlockHash(long n = 0)
        {
            string hashString = "";
            if (block.Transactions.Count != 0)
            {
                hashString = block.PreviousHash
                           + block.TimeStamp.ToString()
                           + JsonConvert.SerializeObject(block.Transactions, Formatting.Indented)
                           + block.Nonce;
            }
            else
            {
                hashString = block.PreviousHash
                           + block.TimeStamp.ToString()
                           + JsonConvert.SerializeObject(block.Vote, Formatting.Indented)
                           + block.Nonce;
            }
            byte[] hashBytes = Encoding.ASCII.GetBytes(hashString);

            Byte[] hashResult = CryptoService.GetHash(hashBytes);
            return ByteArrayToString(hashResult);
        }
        #endregion


    }
}
