using BusinessObject;
using DataAccess.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {

        private static readonly DbContext context = DbContext.Instance;

        private static readonly MemberDAO dao = new MemberDAO(context);

        public void DeleteMember(string theId)
        {
            try
            {
                dao.Remove(theId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public MemberObject? GetMemberByID(string theId)
        {
            MemberObject member = null;
            try
            {
                member = dao.GetMemberById(theId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return member;
        }

        public IEnumerable<MemberObject> GetMembers() => dao.GetMembers();

        public void InsertMember(MemberObject member)
        {
            try
            {
                dao.AddNewMember(member);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdateMember(MemberObject member)
        {
            try
            {
                dao.Update(member);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
