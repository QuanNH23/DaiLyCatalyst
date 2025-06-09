using HealMe.Models;

namespace HealMe.DTO
{
    public class TaskDetailDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public List<Models.Task> Tasks { get; set; }
    }
}
