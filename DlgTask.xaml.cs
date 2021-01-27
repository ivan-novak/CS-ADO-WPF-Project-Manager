
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
    /// Логика взаимодействия для DlgTask.xaml
    /// </summary>
    public partial class DlgTask : Window
    {
        public ProjectDB content { get; set; }
        public Tasks item { get; set; }


        public DlgTask(ProjectDB content)
        {
            InitializeComponent();
            this.content = content;
            item = new Tasks();
            item.Tags = "";
            Title = "Добавить задачу";
            Box_StartDate.SelectedDate = DateTime.Now.AddDays(1);
            Box_EndDate.SelectedDate = DateTime.Now.AddDays(3);
            Window_Config();
        }

        public DlgTask(ProjectDB content, Tasks item)
        {
            InitializeComponent();
            this.content = content;
            this.item = item;
            Title = "Изменить задачу";
            Window_Config();
        }

        private void Window_Config()
        {
            DataContext = item;
            BoxOwner.ItemsSource = content.Persons.ToList();
            BoxList.ItemsSource = content.Lists.ToList();
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
                if (item.Id == 0) content.Tasks.Add(item);
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


        private void ButtonAddList_Click(object sender, RoutedEventArgs e)
        {
            DlgList dlg = new DlgList(content);
            if (dlg.ShowDialog() == false) return;
            Window_Config();
            BoxList.SelectedItem = dlg.item;
        }
    }
}
