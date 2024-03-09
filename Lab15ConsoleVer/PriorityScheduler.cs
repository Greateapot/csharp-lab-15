using System.Collections.Concurrent;

namespace Lab15ConsoleVer
{
    public class PriorityScheduler(ThreadPriority priority) : TaskScheduler
    {
#pragma warning disable CA2211 // Поля, не являющиеся константами, не должны быть видимыми
        public static PriorityScheduler Highest = new(ThreadPriority.Highest);
        public static PriorityScheduler AboveNormal = new(ThreadPriority.AboveNormal);
        public static PriorityScheduler Normal = new(ThreadPriority.Normal);
        public static PriorityScheduler BelowNormal = new(ThreadPriority.BelowNormal);
        public static PriorityScheduler Lowest = new(ThreadPriority.Lowest);
#pragma warning restore CA2211 // Поля, не являющиеся константами, не должны быть видимыми

        private readonly BlockingCollection<Task> _tasks = [];
        private Thread[]? _threads;
        private readonly ThreadPriority _priority = priority;
        private readonly int _maximumConcurrencyLevel = Math.Max(1, Environment.ProcessorCount);

        public override int MaximumConcurrencyLevel => _maximumConcurrencyLevel;
        protected override IEnumerable<Task> GetScheduledTasks() => _tasks;
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => false;
        protected override void QueueTask(Task task)
        {
            _tasks.Add(task);

            if (_threads == null)
            {
                _threads = new Thread[_maximumConcurrencyLevel];
                for (int i = 0; i < _threads.Length; i++)
                {
                    int local = i;
                    _threads[i] = new Thread(() =>
                    {
                        foreach (Task t in _tasks.GetConsumingEnumerable())
                            _ = TryExecuteTask(t);
                    })
                    {
                        Name = $"PriorityScheduler: {i}",
                        Priority = _priority,
                        IsBackground = true
                    };
                    _threads[i].Start();
                }
            }
        }

        public override string ToString() => $"PriorityScheduler(priority={_priority})";
    }
}