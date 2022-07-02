using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IMemberRepository
    {
        IEnumerable<MemberObject> GetMembers();
        MemberObject GetMemberByID(string theId);
        void InsertMember(MemberObject member);
        void UpdateMember(MemberObject member);
        void DeleteMember(string theId);
    }
}
