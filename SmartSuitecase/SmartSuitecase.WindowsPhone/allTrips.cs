using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSuitecase
{
   public class allTrips
    {
        public int id { get; set; }
        public int id_user { get; set; }
        public string country { get; set; }
        public string airport { get; set; }
    }

   public class RootObject
   {
       public List<allTrips> trips { get; set; }
   }
}
