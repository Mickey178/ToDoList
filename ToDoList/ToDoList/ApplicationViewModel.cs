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

        public RelayCommand MoveDownCommand { get; }

        public RelayCommand MoveTopCommand { get; }

        public ObservableCollection<Task> Tasks { get; }

        public DateTime CurrentDate { get; private set; }

        public ApplicationViewModel()
        {
            CurrentDate = DateTime.Now.Date;
            db = new ApplicationContext();
            Tasks = new ObservableCollection<Task>();
            UpdateTasks();
            AddCommand = new RelayCommand(Add);
            DeleteCommand = new RelayCommand(Delete);
            EditCommand = new RelayCommand(Edit);
            PreviousDateCommand = new RelayCommand(PreviousDate);
            NextDateCommand = new RelayCommand(NextDate);
            MoveDownCommand = new RelayCommand(MoveDown);
            MoveTopCommand = new RelayCommand(MoveTop);
        }

        private void UpdateTasks()
        {
            foreach (var task in Tasks)
            {
                task.PropertyChanged -= OnTaskPropertyChange;
            }
            Tasks.Clear();

            foreach (var task in db.Tasks.Where(i => i.Date == CurrentDate).OrderBy(y => y.OrderBy))
            {
                Tasks.Add(task);
                task.PropertyChanged += OnTaskPropertyChange;
            }
        }

        private void Add(object obj)
        {
            var task = new Task();
            var countTasks = Tasks.Count();
            task.OrderBy = countTasks + 1;
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

        private void Delete(object obj)
        {
            if (obj == null)
                return;
            Task task = obj as Task;
            db.Tasks.Remove(task);
            var index = Tasks.IndexOf(task);
            for (int i = index + 1; i < Tasks.Count; i++)
            {
                Tasks[i].OrderBy = Tasks[i].OrderBy - 1;
            }
            db.SaveChanges();
            Tasks.Remove(task);
            task.PropertyChanged -= OnTaskPropertyChange;
        }

        private void Edit(object obj)
        {
            var task = obj as Task;
            task.PropertyChanged -= OnTaskPropertyChange;
            var taskWindow = new TaskWindow();
            taskWindow.DataContext = task;
            var oldBody = task.Body;
            if(taskWindow.ShowDialog() == true)
            {
                SaveChangeInDataBase(task);
                task.PropertyChanged += OnTaskPropertyChange;
            }
            else
            {
                task.Body = oldBody;
                task.PropertyChanged += OnTaskPropertyChange;
            }
        }

        private void PreviousDate(object obj)
        {
            CurrentDate = CurrentDate.AddDays(-1);
            OnPropertyChanged(nameof(CurrentDate));
            UpdateTasks();
        }

        private void NextDate(object obj)
        {
            CurrentDate = CurrentDate.AddDays(1);
            OnPropertyChanged(nameof(CurrentDate));
            UpdateTasks();
        }

        private void SwapPriority(int index1, int index2)
        {
            var selectedPriority = Tasks[index1].OrderBy;
            var bottomPriority = Tasks[index2].OrderBy;
            var phantomPriority = selectedPriority;
            Tasks[index1].OrderBy = bottomPriority;
            Tasks[index2].OrderBy = phantomPriority;
            UpdateTasks();
        }

        private void MoveDown(object obj)
        {
            var indexObj = Tasks.IndexOf(obj as Task);
            if (indexObj + 1 < Tasks.Count())
                SwapPriority(indexObj, indexObj + 1);
            else
                return;
        }

        private void MoveTop(object obj)
        {
            var indexObj = Tasks.IndexOf(obj as Task);
            if (indexObj > 0)
                SwapPriority(indexObj, indexObj -1);
            else
                return;
        }

        private void SaveChangeInDataBase(object obj)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        private void OnTaskPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            SaveChangeInDataBase(sender);
        }
    }
}
