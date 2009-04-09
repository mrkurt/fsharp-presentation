using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;

namespace CSharpExamples
{
    public class Concurrency
    {
        public static void ShowList(string sort)
        {
            var dv = (DataView)HttpContext.Current.Cache["list"];
            dv.Sort = sort;

            ShowList(dv);
        }

        private static void ShowList(DataView dv)
        {
            // print each row
        }
    }
}
