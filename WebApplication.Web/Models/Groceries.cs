using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Groceries
    {

        public string PlanName { get; set; }

        public IList<string> GroceryList { get; set; }
    }
}
