# ImpreciseDateTime
非精确的DateTime，在Windows上使用非精确时间，获取速度更快

```C#
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
```

```
BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.918/21H2)
Intel Core i7-8700 CPU 3.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.100-preview.7.22377.5
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2


|               Method |     Count |                 Mean |             Error |            StdDev | Ratio | Allocated | Alloc Ratio |
|--------------------- |---------- |---------------------:|------------------:|------------------:|------:|----------:|------------:|
|          UseDateTime |         1 |            21.017 ns |         0.0735 ns |         0.0688 ns |  1.00 |         - |          NA |
| UseImpreciseDateTime |         1 |             1.654 ns |         0.0070 ns |         0.0065 ns |  0.08 |         - |          NA |
|                      |           |                      |                   |                   |       |           |             |
|          UseDateTime |   1000000 |    21,184,871.250 ns |    48,524.7914 ns |    45,390.1199 ns |  1.00 |      15 B |        1.00 |
| UseImpreciseDateTime |   1000000 |     2,169,896.725 ns |     8,717.8754 ns |     7,279.8208 ns |  0.10 |       2 B |        0.13 |
|                      |           |                      |                   |                   |       |           |             |
|          UseDateTime | 100000000 | 2,124,748,286.667 ns | 2,827,364.3181 ns | 2,644,718.3374 ns |  1.00 |    3568 B |        1.00 |
| UseImpreciseDateTime | 100000000 |   188,379,548.889 ns |   391,439.8575 ns |   366,153.0855 ns |  0.09 |     160 B |        0.04 |
```