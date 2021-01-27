using AppProjects;
using AppProjects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppProjects
{
    /// <summary>
    /// Логика взаимодействия для DlgList.xaml
    /// </summary>
    public partial class DlgList : Window
    {
        public ProjectDB content { get; set; }
        public Lists item { get; set; }


        public DlgList(ProjectDB content)
        {
            InitializeComponent();
            this.content = content;
            item = new Lists();
            Title = "Добавить список задач";
        }

        public DlgList(ProjectDB content, Lists item)
        {
            InitializeComponent();
            this.content = content;
            this.item = item;
            Title = "Изменить список задач";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = item;
            BoxProjects.ItemsSource = content.Projects.ToList();
            BoxProjects.DisplayMemberPath = "Name";
            BoxProjects.SelectedValuePath = "Id";
            BoxOwner.ItemsSource = (from c in content.Persons select c).ToList();
        }


        bool Validate()
        {
            return true;
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;
            try
            {
                if (item.Id == 0) content.Lists.Add(item);
                content.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch
            {
                MessageBox.Show("Что-то не так с данными. Исправте ошибки и повторите ввод", "Ошибки в данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (item.Id != 0) content.Entry(item).State = EntityState.Unchanged;
            DialogResult = false;
            Close();
        }

        private void ButtonAddProject_Click(object sender, RoutedEventArgs e)
        {
            DlgProject dlg = new DlgProject(content);
            if (dlg.ShowDialog() == false) return;
            Window_Loaded(sender, e);
            BoxProjects.SelectedItem = dlg.item;
        }
    }
}
