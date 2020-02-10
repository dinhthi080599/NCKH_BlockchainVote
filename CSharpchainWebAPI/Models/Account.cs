using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;


namespace CSharpchainWebAPI.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }
    }
}