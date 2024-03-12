using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.Core.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime date)
        {
            PersianCalendar persian = new PersianCalendar();
            return persian.GetYear(date) + "/" + persian.GetMonth(date).ToString("00") + "/" + persian.GetDayOfMonth(date).ToString("00");
        }
    }
}
