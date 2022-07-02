using BusinessObject;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStoreWinApp.services
{
    public class MemberService : IMemberService
    {
        private static IMemberRepository repository = new MemberRepository();

        public List<MemberObject> FetchList()
        {
            return repository.GetMembers().ToList();
        }

        public bool IsLogined(string email, string password)
        {
            return FetchList().Any(e => e.Email.Equals(email) && e.Password.Equals(password));
        }
    }
}
