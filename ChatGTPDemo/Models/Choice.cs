namespace ChatGTPDemo.Models
{
    public class Choice
    {
        public int Index { get; set; }
        public Message? Message { get; set; }
        public string? Finish_Reason { get; set; }
    }
}
