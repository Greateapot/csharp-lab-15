namespace Lab15
{
    public class Runner(string name, ThreadPriority threadPriority = ThreadPriority.Normal)
    {
        public readonly string Name = name;
        public ThreadPriority ThreadPriority { get; set; } = threadPriority;

        public override string ToString() => Name;
    }

    public class RunnerState(Runner runner)
    {
        public readonly Runner Runner = runner;
        public int Position { get; set; }
        public int Index { get; set; }

        public override string ToString() => $"RunnerState(Runner={Runner}, Position={Position}, Index={Index})";
    }
}