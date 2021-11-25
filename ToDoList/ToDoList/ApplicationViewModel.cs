using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;

namespace ToDoList
{
    public class ApplicationViewModel : PropChanged
    {
        private readonly ApplicationContext db;

        public RelayCommand AddCommand { get; }

        public RelayCommand DeleteCommand { get; }

        public RelayCommand EditCommand { get; }

        public ObservableCollection<Task> Tasks { get; }

        public ApplicationViewModel()
        {
            db = new ApplicationContext();
            Tasks = new ObservableCollection<Task>();
            foreach (var task in db.Tasks)
            {
                Tasks.Add(task);
                task.PropertyChanged += OnTaskPropertyChange;
            }
            AddCommand = new RelayCommand(Add);
            DeleteCommand = new RelayCommand(Delete);
            EditCommand = new RelayCommand(Edit);
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
                task.PropertyChanged += OnTaskPropertyChange;
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
            task.PropertyChanged -= OnTaskPropertyChange;
        }

        public void Edit(object obj)
        {
            var task = obj as Task;
            var taskWindow = new TaskWindow();
            taskWindow.DataContext = task;
            taskWindow.ShowDialog();
        }

        public void OnTaskPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Entry(sender).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
