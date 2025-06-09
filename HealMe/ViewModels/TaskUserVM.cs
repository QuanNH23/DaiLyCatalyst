namespace HealMe.ViewModels
{
    public class TaskUserVM
    {
        public int UserId { get; set; }
        public List<Models.Task> TaskHaveAdded { get; set; }
        public List<Models.Task> TaskHaveCompleted { get; set; }
    }
}
