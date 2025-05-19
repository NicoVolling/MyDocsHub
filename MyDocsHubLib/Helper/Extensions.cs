using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDocsHubLib.Helper
{
    public static class Extensions
    {
        public static List<T> SetIds<T>(this List<T> list) where T : BaseObject
        {
            if (list.Count == 0) { return list; }
            for (int i = int.Max(list.Select(o => o.Id).Max(), 0); i < list.Count; i++)
            {
                if (list[i].Id == -1)
                {
                    list[i].Id = i;
                }
            }
            return list;
        }
    }
}