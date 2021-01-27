
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

    public partial class DlgProject : Window
    {
        public ProjectDB content { get; set; }
        public Projects item { get; set; }


        public DlgProject(ProjectDB content)
        {
            InitializeComponent();
            this.content = content;
            item = new Projects();
            item.Tags = "";
            Title = "Добавить проект";
            ViewUpdate();
        }

        public DlgProject(ProjectDB content, Projects item)
        {
            InitializeComponent();
            this.content = content;
            this.item = item;
            Title = "Изменить проект";
            ViewUpdate();
        }

        private void ViewUpdate()
        {
            DataContext = item;
            BoxCompany.ItemsSource = content.Companies.ToList();
            BoxCategory.ItemsSource = content.Categories.ToList();
            BoxOwner.ItemsSource = content.Persons.ToList();
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
                if (item.Id == 0) content.Projects.Add(item);
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

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            DlgCategory dlg = new DlgCategory(content);
            if (dlg.ShowDialog() == false) return;
            ViewUpdate();
            BoxCategory.SelectedItem = dlg.item;
        }

        private void ButtonAddCompany_Click(object sender, RoutedEventArgs e)
        {
            DlgCompany dlg = new DlgCompany(content);
            if (dlg.ShowDialog() == false) return;
            ViewUpdate();
            BoxCategory.SelectedItem = dlg.item;
        }

        private void ButtonAddOwner_Click(object sender, RoutedEventArgs e)
        {
            DlgPerson dlg = new DlgPerson(content);
            if (dlg.ShowDialog() == false) return;
            ViewUpdate();
            BoxOwner.SelectedItem = dlg.item;
        }
    }
}
