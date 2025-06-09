namespace HealMe.DTO
{
    public class AdminTaskDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int CompletedTasksCount { get; set; }
        public int UncompletedTasksCount { get; set; }
    }
}
