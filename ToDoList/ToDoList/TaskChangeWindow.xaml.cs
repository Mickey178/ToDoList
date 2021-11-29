using System.Windows;

namespace ToDoList
{
    public partial class TaskChangeWindow : Window
    {
        public TaskChangeWindow()
        {
            InitializeComponent();
        }

        private void AcceptChangeClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
