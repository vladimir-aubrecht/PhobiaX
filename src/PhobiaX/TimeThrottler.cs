using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX
{
	public class TimeThrottler
	{
		private IDictionary<string, DateTimeOffset> lastDateTimeOffsets = new Dictionary<string, DateTimeOffset>();

		public TimeThrottler()
		{

		}

		public void ExecuteThrottled(string key, TimeSpan delay, DateTimeOffset initialStartTime, Action method)
		{
			if (!lastDateTimeOffsets.ContainsKey(key))
			{
				lastDateTimeOffsets.Add(key, initialStartTime);
			}

			if ((DateTimeOffset.UtcNow - lastDateTimeOffsets[key]).TotalMilliseconds > delay.TotalMilliseconds)
			{
				method();
				lastDateTimeOffsets[key] = DateTimeOffset.UtcNow;
			}
		}
	}
}
