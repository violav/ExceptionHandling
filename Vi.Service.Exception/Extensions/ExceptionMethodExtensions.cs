namespace Vi.Service.Exception.Extensions
{
    public static class ExceptionMethodExtensions
    {
        public static void Add(this System.Collections.IDictionary ex, (string, object?) added)
        {
            ex.Add(added.Item1, added.Item2);
        }
    }
}
