using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Extensions
{
    public static class ListExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Random<T>(this List<T> list)
        {
            var listCount = list.Count;
            if (listCount == 0) return default;
            var randomIndex = UnityEngine.Random.Range(0, listCount);
            return list[randomIndex];
        }
    }
}
