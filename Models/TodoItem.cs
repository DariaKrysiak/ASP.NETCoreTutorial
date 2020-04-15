namespace TutorialApi.Models
{
    public class TodoItem : ITodo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
