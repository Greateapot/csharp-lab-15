using System.Text;

namespace Lab15ConsoleVer
{
    public class Trace(int runnersCount, int distance, Random? random = null)
    {
        public readonly bool[,] Runners = new bool[distance, runnersCount];
        public readonly bool[,] Map = CreateMap(runnersCount, distance, random);
        public readonly int Distance = distance;

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                    if (Runners[i, j])
                        stringBuilder.Append(Config.RunnerIcon);
                    else if (Map[i, j])
                        stringBuilder.Append(Config.BarrierIcon);
                    else
                        stringBuilder.Append(Config.EmptyIcon);
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

        public void PrepareRunners()
        {
            for (int i = 0; i < Runners.GetLength(0); i++)
                for (int j = 0; j < Runners.GetLength(1); j++)
                    Runners[i, j] = i == 0;
        }

        private static bool[,] CreateMap(int runnersCount, int distance, Random? random = null)
        {
            random ??= new Random(DateTimeOffset.Now.Millisecond);
            var trace = new bool[distance, runnersCount];
            bool value = false;
            for (int i = 0; i < distance; i++)
            {
                value = !value && random.Next() % 2 == 0;
                for (int j = 0; j < runnersCount; j++)
                    trace[i, j] = value;
            }
            return trace;
        }
    }
}