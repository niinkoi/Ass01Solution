using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStoreWinApp.services
{
    public interface IMemberService
    {
        public List<MemberObject> FetchList();
        public bool IsLogined(string email, string password);
    }
}
