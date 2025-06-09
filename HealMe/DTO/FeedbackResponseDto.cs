namespace HealMe.DTO
{
    public class FeedbackResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string FeedbackText { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
