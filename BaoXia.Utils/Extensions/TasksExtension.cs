using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions
{
        public static class TasksExtension
        {
                public static bool IsAllTaskNotStarted(this ICollection<Task> tasks)
                {
                        if (tasks == null
                                || tasks.Count < 1)
                        {
                                return true;
                        }

                        foreach (var task in tasks)
                        {
                                if (task.IsStarted() == true)
                                {
                                        return false;
                                }
                        }
                        return true;
                }

                public static bool IsAllTaskStarted(this ICollection<Task> tasks)
                {
                        if (tasks == null
                                || tasks.Count < 1)
                        {
                                return false;
                        }

                        foreach (var task in tasks)
                        {
                                if (task.IsStarted() != true)
                                {
                                        return false;
                                }
                        }
                        return true;
                }
        }
}
