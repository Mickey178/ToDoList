using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace ToDoList
{
    public class ApplicationViewModel : PropChanged
    {
        private readonly ApplicationContext db;

        public RelayCommand AddCommand { get; }

        public RelayCommand DeleteCommand { get; }

        public RelayCommand EditCommand { get; }

        public RelayCommand PreviousDateCommand { get; }

        public RelayCommand NextDateCommand { get; }

        public ObservableCollection<Task> Tasks { get; }

        public ApplicationViewModel()
        {
            var dt = DateTime.Now.ToShortDateString();
            db = new ApplicationContext();
            Tasks = new ObservableCollection<Task>();
            foreach (var task in db.Tasks.Where(i => i.Date == dt))
            {
                Tasks.Add(task);
                task.PropertyChanged += OnTaskPropertyChange;
            }
            AddCommand = new RelayCommand(Add);
            DeleteCommand = new RelayCommand(Delete);
            EditCommand = new RelayCommand(Edit);
            PreviousDateCommand = new RelayCommand(PreviousDate);
            NextDateCommand = new RelayCommand(NextDate);
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
            var taskChangeWindow = new TaskChangeWindow();
            taskChangeWindow.DataContext = task;
            taskChangeWindow.ShowDialog();
        }

        public void PreviousDate(object obj)
        {
            var taskDate = new Task();
            taskDate = Tasks.FirstOrDefault();
            if (taskDate == null)
            {
                Application.Current.Shutdown();
                return;
            }
            DateTime dateTimeOld = DateTime.Parse(taskDate.Date);
            var dateTimeNew = dateTimeOld.AddDays(-1).ToShortDateString();
            Tasks.Clear();
            foreach (var task in db.Tasks.Where(i => i.Date == dateTimeNew))
            {
                Tasks.Add(task);
                task.PropertyChanged += OnTaskPropertyChange;
            }
        }

        public void NextDate(object obj)
        {
            var taskDate = new Task();
            taskDate = Tasks.FirstOrDefault();
            if (taskDate == null)
            {
                Application.Current.Shutdown();
                return;
            }
            DateTime dateTimeOld = DateTime.Parse(taskDate.Date);
            var dateTimeNew = dateTimeOld.AddDays(+1).ToShortDateString();
            Tasks.Clear();
            foreach (var task in db.Tasks.Where(i => i.Date == dateTimeNew))
            {
                Tasks.Add(task);
                task.PropertyChanged += OnTaskPropertyChange;
            }
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
