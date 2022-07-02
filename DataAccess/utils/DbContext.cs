using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.utils
{
    public class DbContext : BaseDAL
    {

        private static DbContext _instance = null;
        private static readonly object instanceLock = new object();

        private DbContext() { }

        public static DbContext Instance
        {
            get
            {
                lock(instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new DbContext();
                    }
                    return _instance;
                }
            }
        }
    }
}
