using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;

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

        public RelayCommand UpdateTasksCommand { get; }

        public RelayCommand SwapTaskCommand { get; }

        public RelayCommand SwapTaskTopCommand { get; }

        public ObservableCollection<Task> Tasks { get; }

        public ApplicationViewModel()
        {
            var dt = DateTime.Now.Date;
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
            UpdateTasksCommand = new RelayCommand(UpdateTasksAccordingToDate);
            SwapTaskCommand = new RelayCommand(SwapTaskDown);
            SwapTaskTopCommand = new RelayCommand(SwapTaskTop);
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
        public void UpdateTasksAccordingToDate(object obj)
        {
            var buttonState = 0;
            if (obj == null)
                buttonState = -1;
            else
                buttonState = 1;

            var taskDate = new Task();
            taskDate = Tasks.FirstOrDefault();

            foreach (var task in db.Tasks.Where(i => i.Date == taskDate.Date))
            {
                task.PropertyChanged -= OnTaskPropertyChange;
            }
            Tasks.Clear();

            var dateTimeNewPage = taskDate.Date.AddDays(buttonState);
            foreach (var task in db.Tasks.Where(i => i.Date == dateTimeNewPage))
            {
                Tasks.Add(task);
                task.PropertyChanged += OnTaskPropertyChange;
            }

            var varificationDateTime = dateTimeNewPage.Date.AddDays(buttonState);
            var checkingFutureTask = db.Tasks.Where(i => i.Date == varificationDateTime).Count();
            if (checkingFutureTask == 0)
            {
                Task emptyTask = new Task { Date = varificationDateTime };
                db.Tasks.Add(emptyTask);
                db.SaveChanges();
            }
        }

        public void SwapTaskDown(object obj)
        {
            var indexObj = Tasks.IndexOf(obj as Task);
            Task selectedTask = Tasks[indexObj];
            Task bottomTask = Tasks[indexObj + 1];
            Task phantomTask = new Task { Body = selectedTask.Body, IsDone = selectedTask.IsDone };
            selectedTask.Body = bottomTask.Body;
            selectedTask.IsDone = bottomTask.IsDone;
            bottomTask.Body = phantomTask.Body;
            bottomTask.IsDone = phantomTask.IsDone;
        }

        public void SwapTaskTop(object obj)
        {
            var indexObj = Tasks.IndexOf(obj as Task);
            Task selectedTask = Tasks[indexObj];
            Task bottomTask = Tasks[indexObj - 1];
            Task phantomTask = new Task { Body = selectedTask.Body, IsDone = selectedTask.IsDone };
            selectedTask.Body = bottomTask.Body;
            selectedTask.IsDone = bottomTask.IsDone;
            bottomTask.Body = phantomTask.Body;
            bottomTask.IsDone = phantomTask.IsDone;
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
