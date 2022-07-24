namespace Scenario.Contracts.Exceptions.Tokenizers
{
    public static class Keywords
    {
        public const string When = "WHEN";
        public const string Then = "THEN";
        public const string Where = "WHERE";
        
        public const string Created = "CREATED";
        public const string Updated = "UPDATED";
        public const string Deleted = "DELETED";

        public static string[] StateEventTypes => new[] { Created, Updated, Deleted};

    }
}