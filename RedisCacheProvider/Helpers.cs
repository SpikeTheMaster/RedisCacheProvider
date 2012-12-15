using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheProvider
{
    internal static class Helpers
    {
        public static long GetExpiryTimeFromNow(DateTime? utcExpiry)
        {
            if (utcExpiry != null)
                return Math.Abs(Convert.ToInt64(TimeSpan.FromTicks(DateTime.Now.Ticks - utcExpiry.Value.Ticks).TotalSeconds));
            else
                return 0;
        }
    }
}
