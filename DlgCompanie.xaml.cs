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

    public partial class DlgCompany : Window
    {
        public ProjectDB content { get; set; }
        public Companies item { get; set; }

        public DlgCompany(ProjectDB content)
        {
            InitializeComponent();
            this.content = content;
            item = new Companies();
            Title = "Добавить компанию";
            ButtonUpdate.Visibility = Visibility.Hidden;
            ButtonNew.Visibility = Visibility.Visible;
            WindowsConfig();
        }

        public DlgCompany(ProjectDB content, Companies item)
        {
            InitializeComponent();
            this.content = content;
            this.item = item;
            Title = "Изменить компанию";
            ButtonUpdate.Visibility = Visibility.Visible;
            ButtonNew.Visibility = Visibility.Hidden;
            WindowsConfig();
        }

        public void WindowsConfig()
        {
            DataContext = item;
            BoxCity.ItemsSource = (from c in content.Companies select c.City).Distinct().ToList();
            BoxCountry.ItemsSource = (from c in content.Companies select c.Country).Distinct().ToList();
            BoxIndustry.ItemsSource = (from c in content.Companies select c.Industry).Distinct().ToList();
            BoxOwner.ItemsSource = (from c in content.Persons select c).ToList();
        }



        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;
            try
            {
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
            if (item.id != 0) content.Entry(item).State = EntityState.Unchanged;
            DialogResult = false;
            Close();
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
              content.Companies.Add(item);
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
        }
    }
}
