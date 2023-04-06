namespace ChatGTPDemo.Models
{
    /// <summary>
    /// Class ChatResponse.
    /// </summary>
    public class ChatResponse
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string? Id { get; set; }
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        public string? Object { get; set; }
        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public int Created { get; set; }
        /// <summary>
        /// Gets or sets the choices.
        /// </summary>
        /// <value>The choices.</value>
        public List<Choice>? Choices { get; set; }
        /// <summary>
        /// Gets or sets the usage.
        /// </summary>
        /// <value>The usage.</value>
        public Usage? Usage { get; set; }
    }
}
