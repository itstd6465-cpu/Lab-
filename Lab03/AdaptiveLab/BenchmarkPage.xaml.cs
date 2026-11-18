using System.Diagnostics;

namespace AdaptiveLab;

public partial class BenchmarkPage : ContentPage
{
    public BenchmarkPage() => InitializeComponent();

    int Rows => int.TryParse(RowsEntry.Text, out var n) && n > 0 ? n : 30;

    // From the Week 3 slides: warm up once, then average many runs.
    static double MsPerBuild(Func<View> create, int runs = 200)
    {
        create(); // warm-up: JIT and resources
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < runs; i++)
            create();
        sw.Stop();
        return sw.Elapsed.TotalMilliseconds / runs;
    }

    void OnTimeBuildClicked(object? sender, EventArgs e)
    {
        int rows = Rows;
        double nested = MsPerBuild(() => FormFactory.BuildNested(rows).Root);
        double flat = MsPerBuild(() => FormFactory.BuildFlat(rows).Root);
        Report($"Build {rows} rows: nested {nested:F3} ms, flat {flat:F3} ms");
    }

    async void OnTimeShowClicked(object? sender, EventArgs e)
    {
        int rows = Rows;
        var nested = new List<double>();
        var flat = new List<double>();
        await TimeShowAsync(() => FormFactory.BuildFlat(rows)); // warm-up run, discarded
        for (int run = 0; run < 5; run++)
        {
            nested.Add(await TimeShowAsync(() => FormFactory.BuildNested(rows)));
            flat.Add(await TimeShowAsync(() => FormFactory.BuildFlat(rows)));
        }
        Host.Content = null;
        Report($"Show {rows} rows, median of 5: nested {Median(nested):F1} ms, flat {Median(flat):F1} ms");
    }

    async Task<double> TimeShowAsync(Func<(View Root, View Last)> build)
    {
        Host.Content = null;
        await Task.Delay(150); // let the previous form go away
        var (root, last) = build();
        var done = new TaskCompletionSource<double>();
        var sw = new Stopwatch();

        void OnSized(object? s, EventArgs e)
        {
            if (last.Width <= 0) return;
            last.SizeChanged -= OnSized;
            sw.Stop();
            done.TrySetResult(sw.Elapsed.TotalMilliseconds);
        }

        last.SizeChanged += OnSized;
        sw.Start();
        Host.Content = root; // native views are created, then measured and arranged
        var finished = await Task.WhenAny(done.Task, Task.Delay(5000));
        return finished == done.Task ? done.Task.Result : -1; // -1 = timed out
    }

    static double Median(List<double> values)
    {
        var sorted = values.OrderBy(v => v).ToList();
        return sorted[sorted.Count / 2];
    }

    void Report(string line)
    {
        ResultLabel.Text += line + Environment.NewLine;
        Console.WriteLine($"[Bench] {line}"); // Debug.WriteLine is removed from Release builds
    }
}
