using AppProjects;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для DlgLogin.xaml
    /// </summary>
    public partial class DlgLogin : Window
    {
        public ProjectDB content { get; set; }

        public DlgLogin(ProjectDB content)
        {
            InitializeComponent();
            this.content = content;
            BoxDate.SelectedDate = DateTime.Now;
            BoxUser.ItemsSource = content.Persons.ToList();       
            this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
