using System;

namespace ToDoList
{
    public class Task : PropChanged
    {
        private string body;

        private bool isDone;

        private DateTime date = DateTime.Now.Date;

        public int Id { get; set; }

        private int orderBy;

        public int OrderBy
        {
            get { return orderBy; }
            set
            {
                orderBy = value;
                OnPropertyChanged();
            }
        }

        public DateTime Date
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
