using AppProjects;
using AppProjects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppProjects
{
    /// <summary>
    /// Логика взаимодействия для DlgComennts.xaml
    /// </summary>
    public partial class DlgComments : Window
    {
        public ProjectDB content { get; set; }
        public Comments item { get; set; }


        public DlgComments(ProjectDB content)
        {
            InitializeComponent();
            this.content = content;
            item = new Comments();
            Title = "Добавить комментарий";
            Window_Config();

        }

        public DlgComments(ProjectDB content, Comments item)
        {
            InitializeComponent();
            this.content = content;
            this.item = item;
            Title = "Изменить комментарий";
            Window_Config();
        }

        private void Window_Config()
        {
            DataContext = item;
            Box_Author.ItemsSource = content.Persons.ToList();
            BoxTasks.ItemsSource = content.Tasks.ToList();
            this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
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
                if (item.Id == 0) content.Comments.Add(item);
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

        private void ButtonAddAuthor_Click(object sender, RoutedEventArgs e)
        {
            DlgPerson dlg = new DlgPerson(content);
            if (dlg.ShowDialog() == false) return;
            Window_Config();
            Box_Author.SelectedItem = dlg.item;
        }

        private void ButtonAddTask_Click(object sender, RoutedEventArgs e)
        {
            DlgTask dlg = new DlgTask(content);
            if (dlg.ShowDialog() == false) return;
            Window_Config();
            BoxTasks.SelectedItem = dlg.item;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
