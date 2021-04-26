using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApi.Helpers
{
    public static class ListExtensions
    {
        public static List<T> Paginate<T> (this List<T> list, int pageNumber, int pageSize)
        {
            return list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
