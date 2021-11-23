using System.Windows;

namespace ToDoList
{
    public partial class TaskWindow : Window
    {
        public TaskWindow()
        {
            InitializeComponent();
        }
        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
