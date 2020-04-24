using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using CSharpchainWebAPI.Models;
namespace CSharpchainWebAPI.Controllers
{
    public class NodeListController : ApiController
    {
        [System.Web.Http.HttpGet]
        public List<string> Index()
        {
            return WebApiApplication.node_list;
        }
    }
}