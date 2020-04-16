using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class HomeViewModel
    {
        public string Username { get; set; }
        public IDictionary<int, string> Plans { get; set; }
    }
}
