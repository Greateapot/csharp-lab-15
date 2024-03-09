namespace Lab15
{
    public class Process(Runner[] runners, int distance, int moveDelay = 1000, int barrierDelay = 2000, Random? random = null)
    {
        public IEnumerable<Runner> Runners => RunnerStates.Select(e => e.Runner);
        public readonly RunnerState[] RunnerStates = runners.Select(e => new RunnerState(e)).ToArray();
        public readonly Trace Trace = new(runners.Length, distance, random);
        public readonly int MoveDelay = moveDelay;
        public readonly int BarrierDelay = barrierDelay;

        public delegate void RunnerFinished(RunnerState state);
        public delegate void RunnerMoved(RunnerState state);
        public delegate void RunnerMeetsBarrier(RunnerState state);

        public event RunnerFinished? OnRunnerFinished;
        public event RunnerMoved? OnRunnerMoved;
        public event RunnerMeetsBarrier? OnRunnerMeetsBarrier;

        private int Position = 0;

        private void Prepare()
        {
            Position = 0;
            Trace.PrepareRunners();
            for (int index = 0; index < RunnerStates.Length; index++)
            {
                RunnerStates[index].Index = index;
                RunnerStates[index].Position = -1;
            }
        }

        public void Start()
        {
            Prepare();
            var tasks = new Task[RunnerStates.Length];
            for (int index = 0; index < RunnerStates.Length; index++)
            {
                var state = RunnerStates[index];
                tasks[index] = Task.Factory.StartNew(
                    () => Run(state),
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    PriorityScheduler.GetPriorityScheduler(state.Runner.ThreadPriority)
                );
            }
            Task.WaitAll(tasks);
        }

        private void Run(RunnerState state)
        {
            for (int i = 0; i < Trace.Distance - 1; i++)
            {
                Thread.Sleep(MoveDelay);
                lock (Trace.Runners)
                {
                    Trace.Runners[i, state.Index] = false;
                    Trace.Runners[i + 1, state.Index] = true;
                }
                OnRunnerMoved?.Invoke(state);
                if (Trace.Map[i + 1, state.Index])
                {
                    Thread.Sleep(BarrierDelay);
                    OnRunnerMeetsBarrier?.Invoke(state);
                }
            }
            lock (this) state.Position = ++Position;
            OnRunnerFinished?.Invoke(state);
        }
    }
}