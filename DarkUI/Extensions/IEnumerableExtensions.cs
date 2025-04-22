using System;
using System.Collections.Generic;
using System.Linq;

namespace DarkUI.Extensions
{
	// Token: 0x02000017 RID: 23
	internal static class IEnumerableExtensions
	{
		// Token: 0x0600008C RID: 140 RVA: 0x000056C8 File Offset: 0x000038C8
		internal static bool IsLast<T>(this IEnumerable<T> items, T item)
		{
			T last = items.LastOrDefault<T>();
			bool flag = last == null;
			return !flag && item.Equals(last);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005708 File Offset: 0x00003908
		internal static bool IsFirst<T>(this IEnumerable<T> items, T item)
		{
			T first = items.FirstOrDefault<T>();
			bool flag = first == null;
			return !flag && item.Equals(first);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005748 File Offset: 0x00003948
		internal static bool IsFirstOrLast<T>(this IEnumerable<T> items, T item)
		{
			return items.IsFirst(item) || items.IsLast(item);
		}
	}
}
