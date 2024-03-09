using System.Text;
using ConsoleIOLib;

namespace Lab15ConsoleVer
{
    public class Program(Runner[] runners, int distance)
    {
        private readonly StringBuilder FinishedBuilder = new();
        // private readonly StringBuilder logBuilder = new();

        private readonly Process Process = new(runners, distance);

        private void PrintOutput()
        {
            ConsoleIO.Clear();
            ConsoleIO.WriteLine(Process.Trace);
            ConsoleIO.WriteLine(FinishedBuilder);
            // ConsoleIO.WriteLine(logBuilder);
        }

        // private void OnRunnerMeetsBarrier(RunnerState state)
        // {
        //     lock (logBuilder) logBuilder.AppendLine($"Runner {state.Runner.Name} meets barrier");
        //     PrintOutput();
        // }

        private void OnRunnerMoved(RunnerState state)
        {
            // lock (logBuilder) logBuilder.AppendLine($"Runner {state.Runner.Name} moved");
            PrintOutput();
        }

        private void OnRunnerFinished(RunnerState state)
        {
            // lock (logBuilder) logBuilder.AppendLine($"Runner {state.Runner.Name} finished");
            lock (FinishedBuilder)
                FinishedBuilder.AppendLine($"Runner {state.Runner.Name} finished on position #{state.Position + 1}");
            PrintOutput();
        }

        public static void Main()
        {
            var runnersCount = ConsoleIO.Input<int>(
                $"Input runners count ({Config.MinRunnersConut} <= N <= {Config.MaxRunnersCount}): ",
                v => v < Config.MinRunnersConut || v > Config.MaxRunnersCount ? "Illegal value!" : null
            );
            var distance = ConsoleIO.Input<int>(
                $"Input distance ({Config.MinDistance} <= N <= {Config.MaxDistance}): ",
                v => v < Config.MinDistance || v > Config.MaxDistance ? "Illegal value!" : null
            );

            var program = new Program(
                Enumerable
                    .Range(0, runnersCount)
                    .Select(e => new Runner(
                        $"Runner#{e + 1}",
                        (e % 3) switch
                        {
                            0 => PriorityScheduler.Lowest,
                            1 => PriorityScheduler.Normal,
                            _ => PriorityScheduler.Highest,
                        }
                    ))
                    .ToArray(),
                distance
            );

            program.Process.OnRunnerMoved += program.OnRunnerMoved;
            program.Process.OnRunnerFinished += program.OnRunnerFinished;
            // program.Process.OnRunnerMeetsBarrier += program.OnRunnerMeetsBarrier;

            program.Process.Start();
        }
    }
}