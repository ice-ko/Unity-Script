using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache
{
    public static class Caches
    {
        public static AccountCache Account { get; set; }
    

        static Caches()
        {
            Account = new AccountCache();
          
        }
    }
}
