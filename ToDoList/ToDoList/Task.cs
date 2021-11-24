namespace ToDoList
{
    public class Task : PropChanged
    {
        private string body;

        private bool isDone;

        public int Id { get; set; }

        public string Body
        {
            get { return body; }
            set
            {
                body = value;
                OnPropertyChanged();
            }
        }
        public bool IsDone
        {
            get { return isDone; }
            set
            {
                isDone = value;
                OnPropertyChanged();
            }
        }
    }
}
