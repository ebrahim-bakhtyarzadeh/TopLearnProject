using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.Core.Convertors
{
    public static class FixedText
    {
        public static string FixEmail(this string email)
        {
            return email.Trim().ToLower();
        }
    }
}
