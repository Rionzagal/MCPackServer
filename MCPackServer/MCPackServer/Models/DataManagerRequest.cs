namespace MCPackServer.Models
{
    public class DataManagerRequest
    {
        public DataManagerRequest() { }
        public List<WhereFilter>? Where { get; set; }
        public List<string>? Select { get; set; }
        public bool RequiresCounts { get; set; }
        public string Table { get; set; } = string.Empty;
        public int Take { get; set; }
        public int Skip { get; set; }
        public KeyValuePair<string, string> Sorted { get; set; }
    }
}
