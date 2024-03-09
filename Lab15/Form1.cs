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

        private void OnRunnerMeetsBarrier(RunnerState state)
        {
            lock (richTextBox1)
            {
                richTextBox1.AppendText($"{state.Runner.Name} встретил барьер\n");
            }
        }

        private void OnRunnerMoved(RunnerState state)
        {
            lock (richTextBox1)
            {
                richTextBox1.AppendText($"{state.Runner.Name} сдвинулся\n");
            }
        }

        private void OnRunnerFinished(RunnerState state)
        {
            lock (richTextBox1)
            {
                richTextBox1.AppendText($"{state.Runner.Name} финишировал на позиции {state.Position}\n");
            }
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            NumericUpDown1_ValueChanged(null, new());

            comboBox1BindingSource.DataSource = runners;
            comboBox1.DataSource = comboBox1BindingSource;

            comboBox2BindingSource.DataSource = threadPriorities;
            comboBox2.DataSource = comboBox2BindingSource;

            numericUpDown2.Value = 10;
            checkBox3.Checked = true;
        }

        private void NumericUpDown1_ValueChanged(object? sender, EventArgs e)
        {
            if (numericUpDown1.Value > runners.Count)
            {
                for (var i = runners.Count; i < numericUpDown1.Value; i++)
                    runners.Add(new Runner($"Бегун №{i + 1}"));
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

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            richTextBox1.Clear();

            var process = new Process([.. runners], (int)numericUpDown2.Value);
            if (checkBox1.Checked)
                process.OnRunnerMoved += OnRunnerMoved;
            if (checkBox2.Checked)
                process.OnRunnerMeetsBarrier += OnRunnerMeetsBarrier;
            if (checkBox3.Checked)
                process.OnRunnerFinished += OnRunnerFinished;
            process.Start();

            button1.Enabled = true;
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
            checkBox3.Enabled = true;
        }
    }
}
