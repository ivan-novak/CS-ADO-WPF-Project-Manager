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
    /// Логика взаимодействия для DlgRole.xaml
    /// </summary>
    public partial class DlgRole : Window
    {
        public DlgRole()
        {
            InitializeComponent();
        }
        public ProjectDB content;
        public Roles item { get; set; }


        bool Validate()
        {
            return true;
        }


        public DlgRole(ProjectDB content)
        {
            InitializeComponent();
            this.content = content;
            item = new Roles();
            Title = "Добавить роль";
        }


        public DlgRole(ProjectDB content, Roles item)
        {
            InitializeComponent();
            this.content = content;
            this.item = item;
            Title = "Изменить роль";
        }


        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (item.Id != 0) content.Entry(item).State = EntityState.Unchanged;
            DialogResult = false;
            Close();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;
            try
            {
                if (item.Id == 0) content.Roles.Add(item);
                content.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch
            {
                MessageBox.Show("Что-то не так с данными. Исправте ошибки и повторите ввод", "Ошибки в данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = item;
        }
    }
}
