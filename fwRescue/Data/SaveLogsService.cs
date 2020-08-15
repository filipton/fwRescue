using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fwRescue.Data
{
	public class SaveLogsService
	{
		public static List<string> Logs = new List<string>();

		public static void AddToLogs(string content)
		{
			Logs.Insert(0, $"[{DateTime.Now.ToString()}] {content}");
			
			foreach(var c in fwRescue.Pages.Logs.indexes)
			{
				c.Refresh();
			}
		}

		public static void ClearLogs()
		{
			Logs.Clear();

			foreach (var c in fwRescue.Pages.Logs.indexes)
			{
				c.Refresh();
			}
		}
	}
}
