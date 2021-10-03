using System;

namespace IdentityGuard.Worker.Functions
{
    public class TimerScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
