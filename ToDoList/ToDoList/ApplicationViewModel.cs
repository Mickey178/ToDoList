using System.Collections.ObjectModel;

namespace ToDoList
{
    public class ApplicationViewModel : PropChanged
    {
        private readonly ApplicationContext db;
        public RelayCommand AddCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public ObservableCollection<Task> Tasks { get; }

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
            var task = new Task();           
            var taskWindow = new TaskWindow();
            taskWindow.DataContext = task;
            if (taskWindow.ShowDialog() == true)
            {
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
    }
}
