using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System;

/// <summary>
/// 非精确的DateTime，在Windows上使用非精确时间，获取速度更快
/// </summary>
public unsafe static partial class ImpreciseDateTime
{
    private const long TicksPerMillisecond = 10000;
    private const long TicksPerSecond = TicksPerMillisecond * 1000;
    private const long TicksPerMinute = TicksPerSecond * 60;
    private const long TicksPerHour = TicksPerMinute * 60;
    private const long TicksPerDay = TicksPerHour * 24;

    private const int DaysTo1601 = 584388;
    private const int DaysTo1970 = 719162;

    private const long FileTimeOffset = DaysTo1601 * TicksPerDay;

    private const ulong KindUtc = 0x4000000000000000;

    /// <inheritdoc cref="DateTime.UtcNow"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime UtcNow()
    {
        var ticks = UtcNowTicks();
        return *(DateTime*)&ticks;
    }
    
    /// <summary>
    /// <inheritdoc cref="DateTime.UtcNow"/>
    /// <inheritdoc cref="DateTime.Ticks"/>
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong UtcNowTicks() => OperatingSystem.IsWindows() ? Windows.GetTicks() : (ulong)DateTime.UtcNow.Ticks;

    /// <summary>
    /// <inheritdoc cref="DateTimeOffset.UtcNow"/>
    /// <inheritdoc cref="DateTimeOffset.ToUnixTimeMilliseconds"/>
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong UtcNowUnixTimeMilliseconds() => OperatingSystem.IsWindows() ? Windows.UtcNowUnixTimeMilliseconds() : (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    private static partial class Windows
    {
        [DllImport("kernel32", EntryPoint = "GetSystemTimeAsFileTime")]
        [SuppressGCTransition]
        public static extern void GetSystemTimeAsFileTime(ulong* time);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong GetTicks()
        {
            ulong fileTime;
            GetSystemTimeAsFileTime(&fileTime);
            return fileTime + (FileTimeOffset | KindUtc);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong UtcNowUnixTimeMilliseconds()
        {
            ulong fileTime;
            GetSystemTimeAsFileTime(&fileTime);
            return (fileTime - ((DaysTo1970 - DaysTo1601) * TicksPerDay)) / TicksPerMillisecond;
        }
    }
}
