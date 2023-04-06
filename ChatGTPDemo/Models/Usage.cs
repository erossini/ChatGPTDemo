namespace ChatGTPDemo.Models
{
    /// <summary>
    /// Class Usage.
    /// </summary>
    public class Usage
    {
        /// <summary>
        /// Gets or sets the prompt tokens.
        /// </summary>
        /// <value>The prompt tokens.</value>
        public int Prompt_Tokens { get; set; }
        /// <summary>
        /// Gets or sets the completion tokens.
        /// </summary>
        /// <value>The completion tokens.</value>
        public int Completion_Tokens { get; set; }
        /// <summary>
        /// Gets or sets the total tokens.
        /// </summary>
        /// <value>The total tokens.</value>
        public int Total_Tokens { get; set; }
    }
}
