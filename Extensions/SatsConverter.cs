namespace SimpLN.Extensions;

public static class SatsConverter
{
	/// <summary>
	/// Converts milli-satoshis (msats) to satoshis (sats).
	/// </summary>
	/// <param name="msats">The value in milli-satoshis.</param>
	/// <returns>The value in satoshis.</returns>
	public static long ToSats(this long msats)
	{
		return msats / 1000;
	}

	public static long ToSats(this long? msats)
	{
		return (long)(msats / 1000)!;
	}

	/// <summary>
	/// Converts milli-satoshis (msats) to satoshis (sats).
	/// </summary>
	/// <param name="msats">The value in milli-satoshis.</param>
	/// <returns>The value in satoshis.</returns>
	public static double ToSats(this double msats)
	{
		return msats / 1000.0;
	}

	public static double ToSats(this double? msats)
	{
		return (double)(msats / 1000.0)!;
	}
}