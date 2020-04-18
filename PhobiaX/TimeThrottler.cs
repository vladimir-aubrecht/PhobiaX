using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX
{
	public class TimeThrottler
	{
		private IDictionary<Action, DateTimeOffset> lastDateTimeOffsets = new Dictionary<Action, DateTimeOffset>();

		public TimeThrottler()
		{

		}

		public void Execute(TimeSpan delay, Action method)
		{
			if (!lastDateTimeOffsets.ContainsKey(method))
			{
				lastDateTimeOffsets.Add(method, DateTimeOffset.MinValue);
			}

			if ((DateTimeOffset.UtcNow - lastDateTimeOffsets[method]).TotalMilliseconds > delay.TotalMilliseconds)
			{
				method();
				lastDateTimeOffsets[method] = DateTimeOffset.UtcNow;
			}
		}
	}
}
