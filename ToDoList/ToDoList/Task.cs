using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoList
{
    public class Task : INotifyPropertyChanged
    {
        private string task_body;
        public int Id { get; set; }
        public string Task_body
        {
            get { return task_body; }
            set
            {
                task_body = value;
                OnPropertyChanged();
            }
        }
        public Task() { }
        public Task(string task_body)
        {
            Task_body = task_body;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
