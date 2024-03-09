namespace Lab15ConsoleVer
{
    public readonly struct Runner(string name, PriorityScheduler? priorityScheduler = null)
    {
        public readonly string Name = name;
        public readonly PriorityScheduler PriorityScheduler = priorityScheduler ?? PriorityScheduler.Normal;

        public override readonly string ToString() => $"Runner(Name={Name}, PriorityScheduler={PriorityScheduler})";
    }

    public struct RunnerState(Runner runner)
    {
        public readonly Runner Runner = runner;
        public int Position { get; set; }
        public int Index { get; set; }

        public override readonly string ToString() => $"RunnerState(Runner={Runner}, Position={Position}, Index={Index})";
    }
}