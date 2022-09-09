using BenchmarkDotNet.Attributes;

namespace ImpreciseDateTimeBenchmark;

[SimpleJob]
[MemoryDiagnoser]
public class Runner
{
    [Params(1, 1_000_000, 100_000_000)]
    public uint Count { get; set; }

    [Benchmark(Baseline = true)]
    public void UseDateTime()
    {
        DateTime time;
        for (uint i = 0; i < Count; i++)
        {
            time = DateTime.UtcNow;
        }
    }

    [Benchmark]
    public void UseImpreciseDateTime()
    {
        DateTime time;
        for (uint i = 0; i < Count; i++)
        {
            time = ImpreciseDateTime.UtcNow();
        }
    }
}
