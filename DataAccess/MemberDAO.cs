using BusinessObject;
using DataAccess.extensions;
using DataAccess.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MemberDAO
    {
        private static DbContext? _context;

        public MemberDAO(DbContext context)
        {
            _context = context;
        }

        private static readonly Dictionary<string, string> Commands = new()
        {
            { "GetAll", "SELECT MemberID, MemberName, Email, Password, City, Country FROM Member" },
            { "GetByID", "SELECT MemberID, MemberName, Email, Password, City, Country FROM Member WHERE MemberID = @MemberID" },
            { "Insert", "INSERT Member VALUES(@MemberID, @MemberName, @Email, @Password, @City, @Country)" },
            { "Update", "UPDATE Member SET MemberName=@MemberName, Email=@Email, Password=@Password, City=@City, Country=@Country WHERE MemberID=@MemberID" },
            { "Delete", "DELETE Member WHERE MemberID=@MemberID" }
        };

        public IEnumerable<MemberObject> GetMembers()
        {
            IDataReader reader = null;
            var membersList = new List<MemberObject>();
            try
            {
                reader = _context.provider.GetDataReader(Commands["GetAll"], CommandType.Text, out _context.connection);
                while (reader.Read())
                {
                    membersList.Add(new()
                    {
                        MemberID = reader.GetGuid(0).ToString(),
                        MemberName = reader.GetString(1),
                        Email = reader.GetString(2),
                        Password = reader.GetString(3),
                        City = reader.GetString(4),
                        Country = reader.GetString(5)
                    }
                    );
                }
            } 
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                reader.Close();
                _context.provider.CloseConnection(_context.connection);
            }
            return membersList;
        }

        public MemberObject? GetMemberById(string guid)
        {
            MemberObject found = null;
            IDataReader reader = null;

            try
            {
                var param = _context.provider.CreateParameter("@MemberID", 16, Guid.Parse(guid), DbType.Guid);
                reader = _context.provider.GetDataReader(Commands["GetByID"], CommandType.Text, out _context.connection, param);

                if (reader.Read())
                {
                    found = new()
                    {
                        MemberID = reader.GetGuid(0).ToString(),
                        MemberName = reader.GetString(1),
                        Email = reader.GetString(2),
                        Password = reader.GetString(3),
                        City = reader.GetString(4),
                        Country = reader.GetString(5)
                    };
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                _context.provider.CloseConnection(_context.connection);
            }
            return found;
        }

        public void AddNewMember(MemberObject member)
        {
            try
            {
                var found = GetMemberById(member.MemberID);
                List<string> errorList = member.Validate(Validation.MEMBER_VALDIATION).Where(item => item != null).ToList();
                if (errorList.Count == 0)
                {
                    if (found == null)
                    {
                        var parameters = new List<SqlParameter>()
                        {
                            _context.provider.CreateParameter("@MemberID", 16, Guid.Parse(member.MemberID), DbType.Guid),
                            _context.provider.CreateParameter("@MemberName", 50, member.MemberName, DbType.String),
                            _context.provider.CreateParameter("@Email", 50, member.Email, DbType.String),
                            _context.provider.CreateParameter("@Password", 50, member.Password, DbType.String),
                            _context.provider.CreateParameter("@City", 20, member.City, DbType.String),
                            _context.provider.CreateParameter("@Country", 20, member.Country, DbType.String),
                        };
                        _context.provider.ModifyData(Commands["Insert"], CommandType.Text, parameters.ToArray());
                    }
                    else
                    {
                        throw new Exception($"This member with ID: {member.MemberID} is already exist");
                    }
                }
                else
                {
                    throw new Exception(errorList.MergeErrors());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            } 
            finally
            {
                _context.provider.CloseConnection(_context.connection);
            }
        }

        public void Update(MemberObject member)
        {

            try
            {
                var found = GetMemberById(member.MemberID);
                List<string> errorList = member.Validate(Validation.MEMBER_VALDIATION).Where(item => item != null).ToList();
                if (errorList.Count == 0)
                {
                    if (found != null)
                    {
                        var parameters = new List<SqlParameter>()
                        {
                            _context.provider.CreateParameter("@MemberID", 16, Guid.Parse(member.MemberID), DbType.Guid),
                            _context.provider.CreateParameter("@MemberName", 50, member.MemberName, DbType.String),
                            _context.provider.CreateParameter("@Email", 50, member.Email, DbType.String),
                            _context.provider.CreateParameter("@Password", 50, member.Password, DbType.String),
                            _context.provider.CreateParameter("@City", 20, member.City, DbType.String),
                            _context.provider.CreateParameter("@Country", 20, member.Country, DbType.String),
                        };
                        _context.provider.ModifyData(Commands["Update"], CommandType.Text, parameters.ToArray());
                    }
                    else
                    {
                        throw new Exception($"This member with ID: {member.MemberID} dose not exist");
                    }
                }
                else
                {
                    throw new Exception(errorList.MergeErrors());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                _context.provider.CloseConnection(_context.connection);
            }
        }

        public void Remove(string theId)
        {
            try
            {
                var found = GetMemberById(theId);
                if (found != null)
                {
                    var param = _context.provider.CreateParameter("@MemberID", 16, Guid.Parse(theId), DbType.Guid);
                    _context.provider.ModifyData(Commands["Delete"], CommandType.Text, param);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                _context.provider.CloseConnection(_context.connection);
            }
        }
    }
}
