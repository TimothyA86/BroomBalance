namespace Helpers
{
	public static class MathHelper
	{
		public static float Sign(float value)
		{
			return value == 0f ? 0f : System.Math.Sign(value);
		}
	}
}