using System;

namespace ToDoList
{
    public class Task : PropChanged
    {
        private string body;

        private bool isDone;

        private string date = DateTime.Now.ToShortDateString();

        public int Id { get; set; }

        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }

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
