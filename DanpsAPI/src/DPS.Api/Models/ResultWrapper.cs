namespace DPS.Api.Models
{
    public class ResultWrapper
    {
        public IEnumerable<string> Errors;

        public bool Success { get; set; }
        public object Data { get; set; }
    }
}