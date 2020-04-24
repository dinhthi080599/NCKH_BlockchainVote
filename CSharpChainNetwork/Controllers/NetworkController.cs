using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CSharpChainNetwork;

namespace CSharpChainNetwork.Controllers
{
	public class NetworkController : ApiController
	{
		[HttpGet]
		public string Ping() // kiểm tra kết nối
		{
			return " Network Contoller Ping";
		}
		[HttpPost]
		public void Register([FromBody]string NewNode) // đăng ký node mới
		{
			Console.ForegroundColor = ConsoleColor.Cyan ;
			Console.WriteLine($"Node mới được thêm vào mạng: {NewNode} ");
			Program1.nodeServices.AddNode(NewNode);
			Console.ResetColor();
		}
		[HttpPost]
		public void Unregister(string RemoveNode)   // xóa node khỏi mạng
		{
			Console.ForegroundColor = ConsoleColor.Cyan ;
			Console.WriteLine($"Node đã bị xóa khỏi mạng: {RemoveNode}");
			Program1.nodeServices.RemoveNode(RemoveNode);
			Console.ResetColor();
		}
	}
}
