namespace Scenario.Application.Tokenizers
{
    public class TokenResult
    {
        public string EventId { get; set; }
        
        public string? EventEntity { get; set; }
        
        public Condition WhereCondition { get; set; }
        
        public Consequence Consequence { get; set; }
    }

    public class Condition
    {
        
    }

    public class Consequence
    {
        
    }
}