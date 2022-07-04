using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using BusinessObject;

namespace DataAccess.extensions
{
    public class Validation
    {
        private string _message;
        private Expression<Func<MemberObject, bool>> _predicate;

        public Validation(string message, Expression<Func<MemberObject, bool>> predicate)
        {
            _message = message;
            _predicate = predicate;
        }

        public string? GetMessage(MemberObject obj) => _predicate.Compile()(obj) ? _message : null;

        public static readonly string EMAIL_REGEX = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]"
                                + @"+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9]"
                                + @"(?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

        public static readonly Validation[] MEMBER_VALDIATION = new Validation[] {
            new Validation("Email should be in right format", x =>  !Regex.IsMatch(x.Email, EMAIL_REGEX)),
            new Validation("Name should be in range from 1 to 50", x => x.MemberName.Length > 50 || x.MemberName.Length < 1),
            new Validation("Password should be in range from 8 to 20", x => x.Password.Length > 20 || x.Password.Length < 8),
            new Validation("City should be in range from 1 to 20", x => x.City.Length > 20 || x.City.Length < 1),
            new Validation("Country should be in range from 1 to 20", x => x.Country.Length > 20 || x.Country.Length < 1),
        };
    }
    public static class DaoExtensions
    {
        public static List<string> Validate(this MemberObject obj, Validation[] validations)
        {
            List<string> errorList = new();
            foreach(var val in validations)
            {
                errorList.Add(val.GetMessage(obj));
            }
            return errorList;
        }

        public static string MergeErrors(this List<string> errorList)
        {
            var builder = new StringBuilder();
            foreach(var error in errorList)
            {
                builder.AppendLine(error);
            }
            return builder.ToString();
        }
    }
}
