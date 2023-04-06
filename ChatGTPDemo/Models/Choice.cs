namespace ChatGTPDemo.Models
{
    /// <summary>
    /// Class Choice.
    /// </summary>
    public class Choice
    {
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public Message? Message { get; set; }
        /// <summary>
        /// Gets or sets the finish reason.
        /// </summary>
        /// <value>The finish reason.</value>
        public string? Finish_Reason { get; set; }
    }
}
