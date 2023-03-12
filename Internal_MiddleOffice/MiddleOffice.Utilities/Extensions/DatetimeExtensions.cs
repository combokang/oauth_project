using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleOffice.Utilities.Extensions
{
    public static class DatetimeExtensions
    {
        public static bool TimeBetween(this DateTime value, TimeSpan start, TimeSpan end)
        {
            TimeSpan now = value.TimeOfDay;

            if (start < end) { return start <= now && now <= end; }

            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }

        public static (DateTime minDate, int minOrderDeliverBatchTimePeriodId) GetMinOrderDeliverBatchTimePeriod(this DateTime value)
        {
            DateTime resultDate = value;
            int minPeriodId = 0;

            if (TimeBetween(value, new TimeSpan(0, 0, 0), new TimeSpan(5, 59, 59)))
            {
                minPeriodId = 1;
                resultDate = value;
            }

            if (TimeBetween(value, new TimeSpan(6, 0, 0), new TimeSpan(9, 59, 59)))
            {
                minPeriodId = 2;
                resultDate = value;
            }

            if (TimeBetween(value, new TimeSpan(10, 0, 0), new TimeSpan(13, 59, 59)))
            {
                minPeriodId = 3;
                resultDate = value;
            }

            if (TimeBetween(value, new TimeSpan(14, 0, 0), new TimeSpan(17, 59, 59)))
            {
                minPeriodId = 4;
                resultDate = value;
            }

            if (TimeBetween(value, new TimeSpan(18, 0, 0), new TimeSpan(23, 59, 59)))
            {
                minPeriodId = 1;
                resultDate = value.AddDays(1);
            }

            return (resultDate.Date, minPeriodId);
        }

        public static int? GetCurrentDeliverBatchTimePeriod(this DateTime value)
        {
            if (TimeBetween(value, new TimeSpan(6, 0, 0), new TimeSpan(9, 59, 59)))
            {
               return 1;
            }

            if (TimeBetween(value, new TimeSpan(10, 0, 0), new TimeSpan(13, 59, 59)))
            {
                return 2;
            }

            if (TimeBetween(value, new TimeSpan(14, 0, 0), new TimeSpan(17, 59, 59)))
            {
                return 3;
            }

            if (TimeBetween(value, new TimeSpan(18, 0, 0), new TimeSpan(23, 59, 59)))
            {
                return 4;
            }

            return null;
        }

        public static (DateTime minDate, int minOrderAppDeliverBatchTimePeriodId) GetMinOrderAppDeliverBatchTimePeriod(this DateTime value)
        {
            DateTime resultDate = value;
            int minPeriodId = 0;

            if (TimeBetween(value, new TimeSpan(0, 0, 0), new TimeSpan(5, 59, 59)))
            {
                minPeriodId = 1;
                resultDate = value;
            }

            if (TimeBetween(value, new TimeSpan(6, 0, 0), new TimeSpan(9, 59, 59)))
            {
                minPeriodId = 2;
                resultDate = value;
            }

            if (TimeBetween(value, new TimeSpan(10, 0, 0), new TimeSpan(13, 59, 59)))
            {
                minPeriodId = 3;
                resultDate = value;
            }

            if (TimeBetween(value, new TimeSpan(14, 0, 0), new TimeSpan(17, 59, 59)))
            {
                minPeriodId = 4;
                resultDate = value;
            }

            if (TimeBetween(value, new TimeSpan(18, 0, 0), new TimeSpan(23, 59, 59)))
            {
                minPeriodId = 1;
                resultDate = value.AddDays(1);
            }

            return (resultDate.Date, minPeriodId);
        }

        public static int? GetCurrentAppDeliverBatchTimePeriod(this DateTime value)
        {
            if (TimeBetween(value, new TimeSpan(6, 0, 0), new TimeSpan(9, 59, 59)))
            {
                return 1;
            }

            if (TimeBetween(value, new TimeSpan(10, 0, 0), new TimeSpan(13, 59, 59)))
            {
                return 2;
            }

            if (TimeBetween(value, new TimeSpan(14, 0, 0), new TimeSpan(17, 59, 59)))
            {
                return 3;
            }

            if (TimeBetween(value, new TimeSpan(18, 0, 0), new TimeSpan(23, 59, 59)))
            {
                return 4;
            }

            return null;
        }
    }
}
