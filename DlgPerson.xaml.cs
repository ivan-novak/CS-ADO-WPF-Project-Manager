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

    public partial class DlgPerson : Window
    {
        public ProjectDB content { get; set; }
        public Persons item { get; set; }


        bool Validate()
        {
            return true;
        }


        public DlgPerson(ProjectDB content)
        {
            InitializeComponent();
            this.content = content;
            item = new Persons();
            Title = "Добавить пользователя";
            Window_Config();
        }

        public DlgPerson(ProjectDB content, Persons item)
        {
            InitializeComponent();
            this.content = content;
            this.item = item;
            Title = "Изменить сведения о пользователе";
           Window_Config();
        }

         private void Window_Config()
        {
            this.DataContext = item;
            BoxCompany.ItemsSource = content.Companies.ToList();
            BoxCompany.DisplayMemberPath = "Name";
            BoxCompany.SelectedValuePath = "id";
            CheckBox_Admin.IsChecked = (item.IsAdmin != null);
        }   


        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            {
                if (!Validate()) return;
                try
                {
                    if(BoxPassword.Password != BoxConferm.Password)
                    {
                        MessageBox.Show("Пароли не совпадает, повторите ввод", "Ошибка пароля", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    item.Pasword = BoxPassword.Password;
                    if ((bool)CheckBox_Admin.IsChecked) item.IsAdmin = 1;
                    else item.IsAdmin = null;
                    if (item.id == 0) content.Persons.Add(item);
                    content.SaveChanges();
                    DialogResult = true;
                    Close();
                }
                catch
                {
                    MessageBox.Show("Что-то не так с данными. Исправте ошибки и повторите ввод", "Ошибки в данных", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (item.id != 0) content.Entry(item).State = EntityState.Unchanged;
            DialogResult = false;
            Close();
        }

        private void ButtonAddCompany_Click(object sender, RoutedEventArgs e)
        {
            DlgCompany dlg = new DlgCompany(content);
            if (dlg.ShowDialog() == false) return;
            Window_Config();
            BoxCompany.SelectedItem = dlg.item;
        }
    }
}
