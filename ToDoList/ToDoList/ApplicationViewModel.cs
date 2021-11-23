using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoList
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        ApplicationContext db;
        public RelayCommand AddCommand { get; }
        public RelayCommand DeleteCommand { get; }
        private ObservableCollection<Task> _Tasks = new ObservableCollection<Task>();
        public ObservableCollection<Task> Tasks
        {
            get
            {
                return _Tasks;
            }
            set
            {
                _Tasks = value;
                OnPropertyChanged();
            }
        }
        public ApplicationViewModel()
        {
            db = new ApplicationContext();
            Tasks = new ObservableCollection<Task>();
            foreach (var task in db.Tasks)
            {
                Tasks.Add(task);
            }
            AddCommand = new RelayCommand(Add);
            DeleteCommand = new RelayCommand(Delete);
        }
        public void Add(object obj)
        {
            var taskWindow = new TaskWindow();
            if (taskWindow.ShowDialog() == true)
            {
                Task task = new Task(taskWindow.addTask.Text);
                db.Tasks.Add(task);
                db.SaveChanges();
                Tasks.Add(task);
            }
        }
        public void Delete(object obj)
        {
            if (obj == null)
                return;
            Task task = obj as Task;
            db.Tasks.Remove(task);
            db.SaveChanges();
            Tasks.Remove(task);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
