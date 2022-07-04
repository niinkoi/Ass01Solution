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

        public void DeleteMember(string theId)
        {
            try
            {
                repository.DeleteMember(theId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<MemberObject> FetchList()
        {
            return repository.GetMembers().ToList();
        }

        public bool IsLogined(string email, string password)
        {
            return FetchList().Any(e => e.Email.Equals(email) && e.Password.Equals(password));
        }

        public void ModifyMember(MemberObject member, ModifidationType type)
        {
            try
            {
                switch(type)
                {
                    case ModifidationType.INSERT:
                        repository.InsertMember(member);
                        break;
                    case ModifidationType.UPDATE:
                        repository.UpdateMember(member);
                        break;
                }
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
