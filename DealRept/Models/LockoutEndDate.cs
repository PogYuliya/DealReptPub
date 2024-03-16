using System;

namespace DealRept.Models
{
    public class LockoutEndDate
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        public LockoutEndDate(DateTimeOffset? lockEndDate)
        {
            Date = lockEndDate.HasValue?lockEndDate.Value.Date:new DateTime(2000,1,1);
            Time = lockEndDate.HasValue?lockEndDate.Value.TimeOfDay:default;
        }

        public LockoutEndDate()
        {

        }

        public DateTimeOffset GetDateTimeOffset()
        {
            return new DateTimeOffset((Date+ Time), new TimeSpan(0,0,0));
        }
    }
}
