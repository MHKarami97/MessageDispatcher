namespace Util
{
    public class Time : ITime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}