using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COE131L
{
    public class User
    {
        public int id { get; set; }
        public int userType { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string nickname  { get; set; }
    }

}
