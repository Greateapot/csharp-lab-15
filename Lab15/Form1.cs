using System.ComponentModel;

namespace Lab15
{
    public partial class Form1 : Form
    {
        private readonly BindingList<Runner> runners = [];
        private readonly BindingList<ThreadPriority> threadPriorities = [
            ThreadPriority.Highest,
            ThreadPriority.AboveNormal,
            ThreadPriority.Normal,
            ThreadPriority.BelowNormal,
            ThreadPriority.Lowest,
        ];

        private readonly BindingSource comboBox1BindingSource = [];
        private readonly BindingSource comboBox2BindingSource = [];


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            NumericUpDown1_ValueChanged(null, new());

            comboBox1BindingSource.DataSource = runners;
            comboBox1.DataSource = comboBox1BindingSource;
            // comboBox1.DisplayMember = "Name";

            comboBox2BindingSource.DataSource = threadPriorities;
            comboBox2.DataSource = comboBox2BindingSource;
            // comboBox2.DisplayMember = "Name";
        }

        private void NumericUpDown1_ValueChanged(object? sender, EventArgs e)
        {
            if (numericUpDown1.Value > runners.Count)
            {
                for (var i = runners.Count; i < numericUpDown1.Value; i++)
                    runners.Add(new Runner($"Runner #{i + 1}"));
            }
            else if (numericUpDown1.Value < runners.Count)
            {
                for (var i = runners.Count; i > numericUpDown1.Value; i--)
                    runners.RemoveAt(i - 1);
            }
        }

        private void ComboBox1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            comboBox2.SelectedIndex = threadPriorities.IndexOf(
                runners.ElementAt(comboBox1.SelectedIndex).ThreadPriority);
        }

        private void ComboBox2_SelectedIndexChanged(object? sender, EventArgs e)
        {
            runners.ElementAt(comboBox1.SelectedIndex).ThreadPriority
                = threadPriorities.ElementAt(comboBox2.SelectedIndex);
        }

        private void OnRunnerMeetsBarrier(RunnerState state)
        {
            lock (richTextBox1)
            {
                richTextBox1.AppendText($"{state.Runner.Name} meets barrier\n");
            }
        }

        private void OnRunnerMoved(RunnerState state)
        {
            lock (richTextBox1)
            {
                richTextBox1.AppendText($"{state.Runner.Name} moved\n");
            }
        }

        private void OnRunnerFinished(RunnerState state)
        {
            lock (richTextBox1)
            {
                richTextBox1.AppendText($"{state.Runner.Name} finished on position {state.Position}\n");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            richTextBox1.Clear();
            var process = new Process([.. runners], 20);
            process.OnRunnerFinished += OnRunnerFinished;
            process.OnRunnerMoved += OnRunnerMoved;
            process.OnRunnerMeetsBarrier += OnRunnerMeetsBarrier;
            process.Start();
            button1.Enabled = true;
        }


    }
}
