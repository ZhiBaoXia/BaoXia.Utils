using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions
{
	public static class TaskExtension
	{
		public static bool IsStarted(this Task task)
		{
			if (task != null)
			{
				switch (task.Status)
				{
					default:
					case TaskStatus.Created:
					case TaskStatus.WaitingForActivation:
					case TaskStatus.WaitingToRun:
						{ }
						break;
					case TaskStatus.Running:
					case TaskStatus.WaitingForChildrenToComplete:
					case TaskStatus.RanToCompletion:
					case TaskStatus.Canceled:
					case TaskStatus.Faulted:
						{
							return true;
						}
				}
			}
			return false;
		}
	}
}
