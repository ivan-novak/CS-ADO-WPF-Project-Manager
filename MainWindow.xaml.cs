using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Infragistics.Controls.Schedules;
using AppProjects;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace DBProjects
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ProjectDB content { get; set; }
        public CollectionViewSource projectsViewSource { get; set; }
        public CollectionViewSource categoriesViewSource { get; set; }
        public CollectionViewSource companiesViewSource { get; set; }
        public CollectionViewSource personsViewSource { get; set; }
        public CollectionViewSource tasksViewSource { get; set; }
        public CollectionViewSource categoriesProjectsViewSource { get; set; }
        public CollectionViewSource commentsViewSource { get; set; }
        public CollectionViewSource listsViewSource { get; set; }
        public CollectionViewSource viewObjectViewSource { get; set; }
        public CollectionViewSource myTasksViewSource { get; set; }
        public CollectionViewSource myCommentsViewSource { get; set; }
        public Persons User { get; set; } = null;
        public DateTime CurrentDate { get; set; } = DateTime.Now;
        public ListBackedProjectViewModel viewmodel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            content = new ProjectDB();
            projectsViewSource = ((CollectionViewSource)(this.FindResource("projectsViewSource")));    
            categoriesViewSource = ((CollectionViewSource)(this.FindResource("categoriesViewSource")));
            categoriesProjectsViewSource = (CollectionViewSource)(FindResource("categoriesProjectsViewSource"));
            companiesViewSource = ((CollectionViewSource)(this.FindResource("companiesViewSource")));
            personsViewSource = ((CollectionViewSource)(this.FindResource("personsViewSource")));
            tasksViewSource = ((CollectionViewSource)(this.FindResource("tasksViewSource")));
            viewObjectViewSource = ((CollectionViewSource)(this.FindResource("viewObjectViewSource")));
            commentsViewSource = ((CollectionViewSource)(this.FindResource("commentsViewSource")));
            listsViewSource = ((CollectionViewSource)(this.FindResource("listsViewSource")));
            myTasksViewSource = ((CollectionViewSource)(this.FindResource("listsTasksViewSource")));
            myCommentsViewSource = ((CollectionViewSource)(this.FindResource("tasksCommentsViewSource")));
            viewmodel = new ListBackedProjectViewModel();
            DlgLogin dlgLogin = null;
            while (true)
            {
                dlgLogin = null;
                dlgLogin = new DlgLogin(content);
                dlgLogin.BoxDate.SelectedDate = CurrentDate;
                if (dlgLogin.ShowDialog() == false)
                {
                    Close();
                    return;
                }
                if(dlgLogin.BoxUser.SelectedValue != null)
                {
                    if (((Persons)(dlgLogin.BoxUser.SelectedItem)).Pasword == null)
                    {
                        if (dlgLogin.BoxPassword.Password == "") break;
                    }
                    else if (((Persons)(dlgLogin.BoxUser.SelectedItem)).Pasword == dlgLogin.BoxPassword.Password) break;
                }
                  MessageBox.Show("Не верный пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            User = (from c in content.Persons where c.id == ((Persons)(dlgLogin.BoxUser.SelectedItem)).id select c).First();
            CurrentDate = (DateTime)dlgLogin.BoxDate.SelectedDate;
            MenuUser.Header = User.Email;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Company_ViewUpdate();
            Project_ViewUpdate();
            List_ViewUpdate();
            Task_ViewUpdate();
            MyTask_ViewUpdate();
            MyComment_ViewUpdate();
            Company_BoxIndustry.ItemsSource = IndustryQuery.ToList();
            Company_BoxCity.ItemsSource = CityQuery.ToList();
            MyTask_BoxOwner.ItemsSource = content.Persons.ToList();
            Task_BoxOwner.ItemsSource = content.Persons.ToList();
            Company_BoxOwner.ItemsSource = content.Persons.ToList();
            List_BoxOwner.ItemsSource = content.Persons.ToList();
            categoriesViewSource.Source = content.Categories.ToList();
            personsViewSource.Source = content.Persons.ToList();
            viewmodel.DownloadDataSource(MyTaskQuery);
            gantt.Project.DataContext = viewmodel;
        }

        void Refresh(FrameworkElement item)
        {
            var buf = item.DataContext;
            item.DataContext = item;
            item.DataContext = buf;
        }

        private void MenuAddCategory_Click(object sender, RoutedEventArgs e)
        {
            DlgCategory winAddCategory = new DlgCategory(content);
            if (winAddCategory.ShowDialog() == false) return;
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            DlgCategory winAddCategory = new DlgCategory(content);
            if (winAddCategory.ShowDialog() == false) return;
        }


        private void MenuAddPerson_Click(object sender, RoutedEventArgs e)
        {
            if(User.IsAdmin == null)
            {
                MessageBox.Show("Этот объект могут изменить только администратор", "Не достаточно прав для операции", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return;
            }
            DlgPerson dlg = new DlgPerson(content);
            if (dlg.ShowDialog() == false) return;
            MyTask_BoxOwner.ItemsSource = content.Persons.ToList();
            Task_BoxOwner.ItemsSource = content.Persons.ToList();
            Company_BoxOwner.ItemsSource = content.Persons.ToList();
            List_BoxOwner.ItemsSource = content.Persons.ToList();
            categoriesViewSource.Source = content.Categories.ToList();
        }

        private void MenuAddProject_Click(object sender, RoutedEventArgs e)
        {
            DlgProject dlg = new DlgProject(content);
            if (dlg.ShowDialog() == false) return;
        }

        private void MenuAddTask_Click(object sender, RoutedEventArgs e)
        {
            DlgTask dlg = new DlgTask(content);
            if (dlg.ShowDialog() == false) return;         
        }

        private void MenuAddList_Click(object sender, RoutedEventArgs e)
        {
            DlgList dlg = new DlgList(content);
            if (dlg.ShowDialog() == false) return;
            Company_ViewUpdate();
        }

        private void MenuAddComment_Click(object sender, RoutedEventArgs e)
        {
            DlgComments dlg = new DlgComments(content);
            if (dlg.ShowDialog() == false) return;
        }

        private void MenuAddRole_Click(object sender, RoutedEventArgs e)
        {
            DlgRole dlg = new DlgRole(content);
            if (dlg.ShowDialog() == false) return;
        }


        IQueryable<Companies> CompanyQuery
        {
            get
            {
                var query = from c in content.Companies select c;
                if(Company_BoxName.Text != "") query = from c in query where c.Name.Contains(Company_BoxName.Text) select c;
                if (Company_BoxIndustry.Text != "") query = from c in query where c.Industry == Company_BoxIndustry.Text select c;
                if (Company_BoxCity.Text != "") query = from c in query where c.City == Company_BoxCity.Text select c;
                if (Company_BoxPhone.Text != "") query = from c in query where c.Fax.Contains(Company_BoxPhone.Text) select c;
                if (Company_BoxNotes.Text != "") query = from c in query where c.Notes.Contains(Company_BoxNotes.Text) select c;
                if (Company_BoxCountry.Text != "") query = from c in query where c.Country == Company_BoxCountry.Text select c;
                if (Company_BoxOwner.SelectedItem != null) query = from c in query where c.Id_Owner == ((Persons)Company_BoxOwner.SelectedItem).id select c;
                if (User == null) return query;
                if (User.IsAdmin != null) return query;
                var permission = (from c in content.ViewObject where c.Id_Owner == User.id select new {Id_Company = c.Id_Company }).Distinct();
                query = (from c in query join p in permission on c.id equals p.Id_Company  select c).Distinct();
                return query;
            }
        }

        IQueryable<Tasks> MyTaskQuery
        {

            get
            {
                DateTime CurrentDate10 = CurrentDate.AddDays(10);
                DateTime CurrentDate30 = CurrentDate.AddDays(30);
                var query = from c in content.Tasks where c.Id_Owner == User.id || c.Lists.Id_Owner == User.id || c.Lists.Projects.Id_Owner == User.id || c.Lists.Projects.Companies.Id_Owner == User.id select c;
                if (MyTask_BoxTerm.Text ==  "На сегодня")
                {
                    query = from c in query where c.Start_Date <= CurrentDate select c;
                    query = from c in query where c.Due_Date >= CurrentDate select c;
                }
                if (MyTask_BoxTerm.Text == "На 10 дней")
                {
                    query = from c in query where c.Start_Date <= CurrentDate10 select c;
                    query = from c in query where c.Due_Date >= CurrentDate select c;
                }
                if (MyTask_BoxTerm.Text == "На 30 дней")
                {
                    query = from c in query where c.Start_Date <= CurrentDate30 select c;
                    query = from c in query where c.Due_Date >= CurrentDate select c;
                }
                if (MyTask_BoxTerm.Text == "Просроченые") query = from c in query where c.Due_Date < CurrentDate select c; 
                if(MyTask_BoxStatus.Text != "") query = from c in query where c.Status == MyTask_BoxStatus.Text select c;
                if (MyTask_BoxPriority.Text != "") query = from c in query where c.Priority == MyTask_BoxPriority.Text select c;
                if (MyTask_BoxNotes.Text != "") query = from c in query where c.Description.Contains(MyTask_BoxNotes.Text) select c;
                if (MyTask_BoxName.Text != "") query = from c in query where c.Name.Contains(MyTask_BoxName.Text) select c;
                if (MyTask_BoxOwner.SelectedValue != null) query = from c in query where c.Id_Owner == ((Persons)(MyTask_BoxOwner.SelectedItem)).id select c;
                var permission = (from c in content.ViewObject where c.Id_Owner == User.id select new { Id_Task = c.Id_Task }).Distinct();
                query = (from c in query join p in permission on c.Id equals p.Id_Task select c).Distinct();
                return query;
            }
        }

        IQueryable<Tasks> LateTaskQuery
        {
            get
            {
                var query = from c in content.Tasks select c;
                if (User != null) query = from c in content.Tasks where c.Id_Owner == User.id select c;
                query = from c in content.Tasks where c.Status != "Выполнено" select c;
                query = from c in content.Tasks where c.Due_Date < CurrentDate select c;
                return query;
            }
        }

        IQueryable<Projects> ProjectQuery
        {
            get
            {
                if (Company_DataGrid.SelectedValue== null) return from c in content.Projects where false select c; ;
                var query = from c in content.Projects where c.Id_Company == ((Companies)(Company_DataGrid.SelectedItem)).id select c;
                if (Project_BoxName.Text != "") query = from c in query where c.Name.Contains(Project_BoxName.Text) select c;
                if (Project_BoxNotes.Text != "") query = from c in query where c.Description.Contains(Project_BoxNotes.Text) select c;
                if (Project_BoxYear.Text != "") query = from c in query where c.Year.ToString() == Project_BoxYear.Text select c;
                if (Project_BoxCategory.SelectedItem != null) query = from c in query where c.Id_Category == ((Categories)Project_BoxCategory.SelectedItem).Id select c;
                if (Project_BoxOwner.SelectedItem != null) query = from c in query where c.Id_Owner == ((Persons)Project_BoxOwner.SelectedItem).id select c;
                if (User == null) return query;
                if (User.IsAdmin != null) return query;
                if (User.id == ((Companies)(Company_DataGrid.SelectedItem)).Id_Owner) return query;
                var permission = (from c in content.ViewObject where c.Id_Owner == User.id select new { Id_Project = c.Id_Project }).Distinct();
                query = (from c in query join p in permission on c.Id equals p.Id_Project  select c).Distinct();
                return query;
            }
        }

        IQueryable<Lists> ListQuery
        {
            get
            {
                if (Project_DataGrid.SelectedValue == null) return from c in content.Lists where false select c;
                var Parent = (Projects)(Project_DataGrid.SelectedItem);
                var query = from c in content.Lists where c.Id_Project == Parent.Id select c;
                if (List_BoxName.Text != "") query = from c in query where c.Name.Contains(List_BoxName.Text) select c;
                if (List_BoxNotes.Text != "") query = from c in query where c.Note.Contains(List_BoxNotes.Text) select c;
                if (List_BoxOwner.SelectedItem != null) query = from c in query where c.Id_Owner == ((Persons)List_BoxOwner.SelectedItem).id select c;
                if (User == null) return query;
                if (User.IsAdmin != null) return query;
                if (User.id == Parent.Companies.Id_Owner) return query;
                if (User.id == Parent.Id_Owner) return query;
                var permission = (from c in content.ViewObject where c.Id_Owner == User.id select new { Id_List = c.Id_List }).Distinct();
                query = (from c in query join p in permission on c.Id equals p.Id_List select c).Distinct();
                return query;
            }
        }

        IQueryable<Comments> CommentQuery
        {
            get
            {
                if (Task_DataGrid.SelectedValue == null) return from c in content.Comments where false select c;
                var Parent = (Tasks)(Task_DataGrid.SelectedItem);
                var query = from c in content.Comments where c.Id_Task == Parent.Id select c;
                if (User == null) return query;
                if (User.IsAdmin != null) return query;
                if (User.id == Parent.Lists.Projects.Companies.Id_Owner) return query;
                if (User.id == Parent.Lists.Projects.Id_Owner) return query;
                if (User.id == Parent.Lists.Id_Owner) return query;
                if (User.id == Parent.Id_Owner) return query;
                var permission = (from c in content.ViewObject where c.Id_Owner == User.id select new { Id_Comment = c.Id_Comment }).Distinct();
                query = (from c in query join p in permission on c.Id equals p.Id_Comment select c).Distinct();
                return query;
            }
        }

        IQueryable<Comments> MyCommentQuery
        {
            get
            {
                if (MyTask_DataGrid.SelectedValue == null) return from c in content.Comments where false select c;
                var query = from c in content.Comments where c.Id_Task == ((Tasks)(MyTask_DataGrid.SelectedItem)).Id select c;
                query = from c in query where c.Id_Person == User.id || 
                        c.Tasks.Id_Owner == User.id ||
                        c.Tasks.Lists.Id_Owner == User.id || 
                        c.Tasks.Lists.Projects.Id_Owner == User.id ||
                        c.Tasks.Lists.Projects.Companies.Id_Owner == User.id select c;
                if (User == null) return query;
                if (User.IsAdmin != null) return query;
                var permission = (from c in content.ViewObject where c.Id_Owner == User.id select new { Id_Comment = c.Id_Comment }).Distinct();
                query = (from c in query join p in permission on c.Id equals p.Id_Comment select c).Distinct();
                return query;
            }
        }

        IQueryable<Tasks> TaskQuery
        {
            get
            {
                DateTime CurrentDate10 = CurrentDate.AddDays(10);
                DateTime CurrentDate30 = CurrentDate.AddDays(30);
                if (List_DataGrid.SelectedValue == null) return from c in content.Tasks where false select c;
                var query = from c in content.Tasks where c.Id_List == ((Lists)(List_DataGrid.SelectedItem)).Id select c;
                if (Task_BoxTerm.Text == "На сегодня")
                {
                    query = from c in query where c.Start_Date <= CurrentDate select c;
                    query = from c in query where c.Due_Date >= CurrentDate select c;
                }
                if (Task_BoxTerm.Text == "На 10 дней")
                {
                    query = from c in query where c.Start_Date <= CurrentDate10 select c;
                    query = from c in query where c.Due_Date >= CurrentDate select c;
                }
                if (Task_BoxTerm.Text == "На 30 дней")
                {
                    query = from c in query where c.Start_Date <= CurrentDate30 select c;
                    query = from c in query where c.Due_Date >= CurrentDate select c;
                }
                if (Task_BoxTerm.Text == "Просроченые")
                {
                    query = from c in query where c.Due_Date < CurrentDate select c;
                }
                if (Task_BoxStatus.Text != "") query = from c in query where c.Status == Task_BoxStatus.Text select c;
                if (Task_BoxPriority.Text != "") query = from c in query where c.Priority == Task_BoxPriority.Text select c;
                if (Task_BoxNotes.Text != "") query = from c in query where c.Description.Contains(Task_BoxNotes.Text) select c;
                if (Task_BoxName.Text != "") query = from c in query where c.Name.Contains(Task_BoxName.Text) select c;
                if (Task_BoxOwner.SelectedItem != null) query = from c in query where c.Id_Owner == ((Persons)(Task_BoxOwner.SelectedItem)).id select c;
                if (User == null) return query;
                if (User.IsAdmin != null) return query;
                var Parent = (Lists)(List_DataGrid.SelectedItem);
                if (User.id == Parent.Projects.Companies.Id_Owner) return query;
                if (User.id == Parent.Projects.Id_Owner) return query;
                if (User.id == Parent.Id_Owner) return query;
                 var permission = (from c in content.ViewObject where c.Id_Owner == User.id select new { Id_Task = c.Id_Task }).Distinct();
                query = from c in query join p in permission on c.Id equals p.Id_Task select c;
                return query;
            }
        }

        IQueryable<String> IndustryQuery
        {
            get
            {
                var query = from c in content.Companies select c.Industry;
                return query.Distinct();
            }
        }

        IQueryable<Persons> PersonQuery
        {
            get
            {
                var query = from c in content.Persons select c;
                return query.Distinct();
            }
        }

        IQueryable<String> CityQuery
        {
            get
            {
                var query = from c in content.Companies select c.City;
                return query.Distinct();
            }
        }

        IQueryable<int?> YearQuery
        {
            get
            {
                var query = from c in content.Projects select c.Year;
                return query.Distinct();
            }
        }

        IQueryable<Categories> CategoryQuery
        {
            get
            {
                return  from c in content.Categories select c;
            }
        }

        IQueryable<Persons> OwnerQuery
        {
            get
            {
                return from c in content.Persons select c;
            }
        }


        IQueryable<String> CountryQuery
        {
            get
            {
                var query = from c in content.Companies select c.Country;
                return query.Distinct();
            }
        }

        public void Company_ViewUpdate()
        {
            int saveId = -1;
            if (Company_DataGrid.SelectedValue != null) saveId = ((Companies)(Company_DataGrid.SelectedItem)).id;
            companiesViewSource.Source = CompanyQuery.ToList();
            Company_DataGrid.SelectedItem = (from c in (List<Companies>)(companiesViewSource.Source) where c.id==saveId select c).FirstOrDefault();
            if (Company_DataGrid.SelectedItem == null) if (Company_DataGrid.Items.Count > 0) Company_DataGrid.SelectedIndex = 0;
        }

        private void Company_ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!Company_CheckPermission()) return;
            DlgCompany dlg = new DlgCompany(content);
            if(User != null) dlg.item.Id_Owner = User.id;
            if (dlg.ShowDialog() == false) return;
            Company_ViewUpdate();
            Company_BoxCity.ItemsSource = CityQuery.ToList();
            Company_BoxIndustry.ItemsSource = IndustryQuery.ToList();
            Company_DataGrid.ItemsSource = CompanyQuery.ToList();
        }

        private void Company_ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Company_DataGrid.SelectedItem == null) return;
            if (!Company_CheckPermission()) return;
            DlgCompany dlg = new DlgCompany(content, (Companies)Company_DataGrid.SelectedItem);
            if (dlg.ShowDialog() == false) return;
            Company_ViewUpdate();
            Company_BoxCity.ItemsSource = CityQuery.ToList();
            Company_BoxIndustry.ItemsSource = IndustryQuery.ToList();
            Company_DataGrid.ItemsSource = CompanyQuery.ToList();
        }

        bool Company_CheckPermission()
        {
            if (User == null) return true;
            if (User.IsAdmin != null) return true;
            Companies item = (Companies)Company_DataGrid.SelectedItem;
            if(Company_DataGrid.SelectedValue != null) if (User.id == item.Id_Owner) return true;
            MessageBox.Show("Этот объект могут изменить только администратор или владелец", "Не достаточно прав для операции", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            return false;
        }

        bool Project_CheckPermission()
        {
            if (User == null) return true;
            if (User.IsAdmin != null) return true;
            Projects item = (Projects)Project_DataGrid.SelectedItem;
            if (Project_DataGrid.SelectedValue != null)
            {
                if (User.id == item.Id_Owner) return true;
                if (User.id == item.Companies.Id_Owner) return true;
            }
            MessageBox.Show("Этот объект могут изменить только администратор или владелец", "Не достаточно прав для операции", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            return false;
        }

        bool List_CheckPermission()
        {
            if (User == null) return true;
            if (User.IsAdmin != null) return true;
            Lists item = (Lists)List_DataGrid.SelectedItem;
            if (List_DataGrid.SelectedValue != null)
            {
                if (User.id == item.Id_Owner) return true;
                if (User.id == item.Projects.Id_Owner) return true;
                if (User.id == item.Projects.Companies.Id_Owner) return true;
            }
            MessageBox.Show("Этот объект могут изменить только администратор или владелец", "Не достаточно прав для операции", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            return false;
        }

        bool Task_CheckPermission()
        {
            if (User == null) return true;
            if (User.IsAdmin != null) return true;
            Tasks item = (Tasks)Task_DataGrid.SelectedItem;
            if (Task_DataGrid.SelectedValue != null)
            {
                if (User.id == item.Id_Owner) return true;
                if (User.id == item.Lists.Id_Owner) return true;
                if (User.id == item.Lists.Projects.Id_Owner) return true;
                if (User.id == item.Lists.Projects.Companies.Id_Owner) return true;
            }
            MessageBox.Show("Этот объект могут изменить только администратор или владелец", "Не достаточно прав для операции", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            return false;
        }

        bool Comment_CheckPermission()
        {
            if (User == null) return true;
            if (User.IsAdmin != null) return true;
            Comments item = (Comments)Comment_DataGrid.SelectedItem;
            if (Comment_DataGrid.SelectedValue != null)
            {
                if (User.id == item.Id_Person) return true;
                if (User.id == item.Tasks.Id_Owner) return true;
                if (User.id == item.Tasks.Lists.Id_Owner) return true;
                if (User.id == item.Tasks.Lists.Projects.Id_Owner) return true;
                if (User.id == item.Tasks.Lists.Projects.Companies.Id_Owner) return true;
            }
            MessageBox.Show("Этот объект могут изменить только администратор или владелец", "Не достаточно прав для операции", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        private void Company_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Company_DataGrid.SelectedItem == null ) return;
            if (!Company_CheckPermission()) return;
            if (User.IsAdmin == null) return;
            if (MessageBox.Show("Удалить компанию " + ((Companies)Company_DataGrid.SelectedItem).Name + " и все связанные с ней объекты?", "Удаление компании", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
                return;
            content.Companies.Remove(((Companies)(Company_DataGrid).SelectedItem));
            content.SaveChanges();
            Company_ViewUpdate();
        }

        private void Company_FilterButton_Click(object sender, EventArgs e)
        {
            Company_ViewUpdate();
            Company_DataGrid.Focus();
        }

        private void Company_ButtonToProject_Click(object sender, RoutedEventArgs e)
        {
            Project_Tab.Focus();
        }

        private void Company_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Project_ViewUpdate();
            Company_ButtonToProject.IsEnabled = Company_DataGrid.SelectedValue != null;
            Project_Tab.IsEnabled = Company_DataGrid.SelectedValue != null;
            Company_ButtonDelete.IsEnabled = Company_DataGrid.SelectedValue != null;
            Company_ButtonEdit.IsEnabled = Company_DataGrid.SelectedValue != null;
        } 

        private void Company_DataGrid_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = e.OriginalSource as Hyperlink;
            if (hyperlink != null)
            {
                Process.Start(hyperlink.NavigateUri.ToString());
            }
        }

        private void Project_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Project_DataGrid.SelectedValue == null) return;
            if (!Company_CheckPermission()) return;
            if (MessageBox.Show("Удалить компанию " + ((Projects)Project_DataGrid.SelectedItem).Name + " и все связанные с ней объекты?",
                "Удаление компании", MessageBoxButton.OK, MessageBoxImage.Warning) == MessageBoxResult.Cancel) return;
            content.Projects.Remove(((Projects)(Project_DataGrid).SelectedItem));
            content.SaveChanges();
            Project_ViewUpdate();
        }

        private void Project_ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!Project_CheckPermission()) return;
            if (Project_DataGrid.SelectedItem == null) return;
            DlgProject dlg = new DlgProject(content, (Projects)Project_DataGrid.SelectedItem);
            if (dlg.ShowDialog() == false) return;
            Project_ViewUpdate();          
        }

        private void Project_ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!Company_CheckPermission()) return;
            DlgProject dlg = new DlgProject(content);
            dlg.item.Id_Company = ((Companies)(Company_DataGrid.SelectedItem)).id;
            if (User != null) dlg.item.Id_Owner = User.id;
            if (dlg.ShowDialog() == false) return;
            Project_ViewUpdate();
        }

        private void Project_FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Project_ViewUpdate();
            Project_DataGrid.Focus();
        }

        private void Project_ButtonToProject_Click(object sender, RoutedEventArgs e)
        {
            List_Tab.Focus();
        }
  
        public void Project_ViewUpdate()
        {
            int saveId = -1;
            if (Project_DataGrid.SelectedValue != null) saveId = ((Projects)(Project_DataGrid.SelectedItem)).Id;
            projectsViewSource.Source = ProjectQuery.ToList();
            Project_DataGrid.SelectedItem = (from c in (List<Projects>)(projectsViewSource.Source) where c.Id == saveId select c).FirstOrDefault();
            if (Project_DataGrid.SelectedItem == null) if (Project_DataGrid.Items.Count > 0) Project_DataGrid.SelectedIndex = 0;
            Project_BoxYear.ItemsSource = YearQuery.ToList();
            Project_BoxCategory.ItemsSource = CategoryQuery.ToList();
            Project_BoxOwner.ItemsSource = OwnerQuery.ToList();
        }

        private void Project_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List_ViewUpdate();
            Project_ButtonToList.IsEnabled = Project_DataGrid.SelectedValue != null;
            List_Tab.IsEnabled = Project_DataGrid.SelectedValue != null;
            Project_ButtonDelete.IsEnabled = Project_DataGrid.SelectedValue != null;
            Project_ButtonEdit.IsEnabled = Project_DataGrid.SelectedValue != null;
        }

        private void List_ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!List_CheckPermission()) return;
            if (List_DataGrid.SelectedItem == null) return;
            DlgList dlg = new DlgList(content, (Lists)List_DataGrid.SelectedItem);
            if (dlg.ShowDialog() == false) return;
            List_ViewUpdate();
        }

        private void List_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!Project_CheckPermission()) return;
            if (List_DataGrid.SelectedItem == null) return;
            if(MessageBox.Show("Удалить этап " + ((Lists)List_DataGrid.SelectedItem).Name + " и все связанные с ним объекты?", "Удаление компании", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel) return;
            content.Lists.Remove(((Lists)(List_DataGrid).SelectedItem));
            content.SaveChanges();
            List_ViewUpdate();
        }

        private void List_FilterButton_Click(object sender, RoutedEventArgs e)
        {
            List_ViewUpdate();
        }

        private void List_ButtonToProject_Click(object sender, RoutedEventArgs e)
        {
            Task_Tab.Focus();
        }

        public void List_ViewUpdate()
        {
            int saveId = -1;
            if (List_DataGrid.SelectedItem != null) saveId = ((Lists)(List_DataGrid.SelectedItem)).Id;
            listsViewSource.Source = ListQuery.ToList();
            List_DataGrid.SelectedItem = (from c in (List<Lists>)(listsViewSource.Source) where c.Id == saveId select c).FirstOrDefault();
            if (List_DataGrid.SelectedItem  == null) if (List_DataGrid.Items.Count > 0) List_DataGrid.SelectedIndex = 0;
        }

        public void Comment_ViewUpdate()
        {
            int saveId = -1;
            if (Comment_DataGrid.SelectedItem != null) saveId = ((Comments)(Comment_DataGrid.SelectedItem)).Id;
            commentsViewSource.Source = CommentQuery.ToList();
            Comment_DataGrid.SelectedItem = (from c in (List<Comments>)(commentsViewSource.Source) where c.Id == saveId select c).FirstOrDefault();
            if (Comment_DataGrid.SelectedItem == null) if (Comment_DataGrid.Items.Count > 0) Comment_DataGrid.SelectedIndex = 0;
        }

        private void List_ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!Project_CheckPermission()) return;
           DlgList dlg = new DlgList(content);
            dlg.item.Id_Project = ((Projects)(Project_DataGrid.SelectedItem)).Id;
            if(User != null) dlg.item.Id_Owner = User.id;
            if (dlg.ShowDialog() == false) return;
            List_ViewUpdate();
        }

        public void Task_ViewUpdate()
        {
            int saveId = -1;
            if (Task_DataGrid.SelectedValue != null) saveId = ((Tasks)(Task_DataGrid.SelectedItem)).Id;
            tasksViewSource.Source = TaskQuery.ToList();
            Task_DataGrid.SelectedItem = (from c in (List<Tasks>)(tasksViewSource.Source) where c.Id == saveId select c).FirstOrDefault();
            if (Task_DataGrid.SelectedItem == null) if (Task_DataGrid.Items.Count > 0) Task_DataGrid.SelectedIndex = 0;
        }


        private void Task_ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!List_CheckPermission()) return;
            DlgTask dlg = new DlgTask(content);
            dlg.item.Id_List = ((Lists)(List_DataGrid.SelectedItem)).Id;
            if (User != null) dlg.item.Id_Owner = User.id;
            if (dlg.ShowDialog() == false) return;
            Task_ViewUpdate();
        }

        private void Task_ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!Task_CheckPermission()) return;
            if (Task_DataGrid.SelectedItem == null) return;
            DlgTask dlg = new DlgTask(content, (Tasks)Task_DataGrid.SelectedItem);
            if (dlg.ShowDialog() == false) return;
            Task_ViewUpdate();
        }


        private void Task_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!List_CheckPermission()) return;
            if (Task_DataGrid.SelectedItem == null) return;
            if (MessageBox.Show("Удалить задачу " + ((Tasks)Task_DataGrid.SelectedItem).Name + " и все связанные с ней объекты?", 
                "Удаление компании", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel) return;
            content.Tasks.Remove(((Tasks)(Task_DataGrid).SelectedItem));
            content.SaveChanges();
            Task_ViewUpdate();
        }


        private void List_DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Task_ViewUpdate();
            List_ButtonToTask.IsEnabled = List_DataGrid.SelectedValue != null;
            List_ButtonDelete.IsEnabled = List_DataGrid.SelectedValue != null;
            List_ButtonEdit.IsEnabled = List_DataGrid.SelectedValue != null;
            Task_Tab.IsEnabled = List_DataGrid.SelectedValue != null;
        }

        private void Company_BoxIndustry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Task_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Comment_ViewUpdate();
            Task_ButtonDelete.IsEnabled = Task_DataGrid.SelectedValue != null;
            Task_ButtonEdit.IsEnabled = Task_DataGrid.SelectedValue != null;
            Comment_ButtonAdd.IsEnabled = Task_DataGrid.SelectedValue != null;
         }


        private void MenuLogIn_Click(object sender, RoutedEventArgs e)
        {
            DlgLogin dlgLogin = new DlgLogin(content);
            dlgLogin.BoxDate.SelectedDate = CurrentDate;
            if (User != null) dlgLogin.BoxUser.SelectedValue = User.id;
            dlgLogin.ShowDialog();
            if (dlgLogin.DialogResult == false) return;
            User = (from c in content.Persons where c.id == ((Persons)(dlgLogin.BoxUser.SelectedItem)).id select c).First();
            CurrentDate = (DateTime)dlgLogin.BoxDate.SelectedDate;
        }

        private void Comment_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Comment_ButtonEdit.IsEnabled = Comment_DataGrid.SelectedValue != null;
            Comment_ButtonDelete.IsEnabled = Comment_DataGrid.SelectedValue != null;
        }

        private void Comment_ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!Task_CheckPermission()) return;
            if (Task_DataGrid.SelectedValue == null) return;
            DlgComments dlg = new DlgComments(content);
            dlg.item.Id_Task = ((Tasks)(Task_DataGrid.SelectedItem)).Id;
            dlg.item.Create_Date = DateTime.Now;
            dlg.item.Edit_Date = DateTime.Now;
            if (User != null) dlg.item.Id_Person = User.id;
            if ( dlg.ShowDialog() == false) return;
            Comment_ViewUpdate();
            MyComment_ViewUpdate();
        }

        private void Comment_ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!Comment_CheckPermission()) return;
            if (Comment_DataGrid.SelectedValue == null) return;
            DlgComments dlg = new DlgComments(content, (Comments)Comment_DataGrid.SelectedItem);           
            dlg.item.Edit_Date = DateTime.Now;
            if (User != null) dlg.item.Id_Person = User.id;
            if (dlg.ShowDialog() == false) return;
            content.SaveChanges();
            Comment_ViewUpdate();
            MyComment_ViewUpdate();
        }

        private void Comment_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!Task_CheckPermission()) return;
        }

        private void Comment_ButtonDelete_Click_1(object sender, RoutedEventArgs e)
        {
            if (!Task_CheckPermission()) return;
            if (Comment_DataGrid.SelectedItem == null) return;
            if (MessageBox.Show("Удалить комментарий?",
                "Удаление компании", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel) return;
            content.Comments.Remove(((Comments)(Comment_DataGrid).SelectedItem));
            content.SaveChanges();
            Comment_ViewUpdate();
        }

       public void MyTask_ViewUpdate()
       {
           int saveId = -1;
           if (MyTask_DataGrid.SelectedValue != null) saveId = ((Tasks)(MyTask_DataGrid.SelectedItem)).Id;
           myTasksViewSource.Source = MyTaskQuery.ToList();
           MyTask_DataGrid.SelectedItem = (from c in (List<Tasks>)(myTasksViewSource.Source) where c.Id == saveId select c).FirstOrDefault();
           if (MyTask_DataGrid.SelectedItem == null) if (MyTask_DataGrid.Items.Count > 0) MyTask_DataGrid.SelectedIndex = 0;
       }

        private void MyTask_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {         
            Comment_ViewUpdate();
            MyComment_ViewUpdate();
            MyTask_ButtonDelete.IsEnabled = MyTask_DataGrid.SelectedValue != null;
            MyTask_ButtonEdit.IsEnabled = MyTask_DataGrid.SelectedValue != null;
            MyComment_ButtonAdd.IsEnabled = MyTask_DataGrid.SelectedValue != null;
        }

        private void MyTask_ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!Task_CheckPermission()) return;
            if (MyTask_DataGrid.SelectedItem == null) return;
            DlgTask dlg = new DlgTask(content, (Tasks)MyTask_DataGrid.SelectedItem);
            if (dlg.ShowDialog() == false) return;
            Task_ViewUpdate();
            MyTask_ViewUpdate();
        }

        private void MyTask_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MyTask_DataGrid.SelectedItem == null) return;
            if (MessageBox.Show("Удалить задачу " + ((Tasks)MyTask_DataGrid.SelectedItem).Name + " и все связанные с ней объекты?",
                "Удаление компании", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel) return;
            content.Tasks.Remove(((Tasks)(MyTask_DataGrid).SelectedItem));
            content.SaveChanges();
            Task_ViewUpdate();
            MyTask_ViewUpdate();
        }

        private void MyComment_ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (MyTask_DataGrid.SelectedValue == null) return;
            if (!Task_CheckPermission()) return;
            DlgComments dlg = new DlgComments(content);
            dlg.item.Id_Task = ((Tasks)(MyTask_DataGrid.SelectedItem)).Id;
            dlg.item.Create_Date = DateTime.Now;
            dlg.item.Edit_Date = DateTime.Now;
            if (User != null) dlg.item.Id_Person = User.id;
            if (dlg.ShowDialog() == false) return;
            Comment_ViewUpdate();
            MyComment_ViewUpdate();
        }

        private void MyComment_ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (MyComment_DataGrid.SelectedValue == null) return;
            if (!Comment_CheckPermission()) return;       
            DlgComments dlg = new DlgComments(content, (Comments)MyComment_DataGrid.SelectedItem);
            dlg.item.Edit_Date = DateTime.Now;
            if (User != null) dlg.item.Id_Person = User.id;
            if (dlg.ShowDialog() == false) return;
            content.SaveChanges();
            Comment_ViewUpdate();
            MyComment_ViewUpdate();
        }

        private void MyComment_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MyComment_DataGrid.SelectedItem == null) return;
            if (!Task_CheckPermission()) return;
            if (MessageBox.Show("Удалить комментарий?",
                "Удаление компании", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel) return;
            content.Comments.Remove(((Comments)(MyComment_DataGrid).SelectedItem));
            content.SaveChanges();
            Comment_ViewUpdate();
            MyComment_ViewUpdate();
        }

        public void MyComment_ViewUpdate()
        {
            int saveId = -1;
            if (MyComment_DataGrid.SelectedItem != null) saveId = ((Comments)(MyComment_DataGrid.SelectedItem)).Id;
            myCommentsViewSource.Source = MyCommentQuery.ToList();
            MyComment_DataGrid.SelectedItem = (from c in (List<Comments>)(myCommentsViewSource.Source) where c.Id == saveId select c).FirstOrDefault();
            if (MyComment_DataGrid.SelectedItem == null) if (MyComment_DataGrid.Items.Count > 0) MyComment_DataGrid.SelectedIndex = 0;
        }

        private void MyTask_FilterButton_Click(object sender, RoutedEventArgs e)
        {
            MyTask_ViewUpdate();
        }

        private void MyComment_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
               MyComment_ButtonEdit.IsEnabled = MyComment_DataGrid.SelectedValue != null;
               MyComment_ButtonDelete.IsEnabled = MyComment_DataGrid.SelectedValue != null;
        }

        private void MenuChangeUser_Click(object sender, RoutedEventArgs e)
        {
            Persons curUser = User;

            DlgLogin dlgLogin = null;
            while (true)
            {
                dlgLogin = new DlgLogin(content);
                dlgLogin.BoxDate.SelectedDate = CurrentDate;
                dlgLogin.ShowDialog();
                if (dlgLogin.DialogResult == false) return;
                if (dlgLogin.BoxUser.SelectedValue != null)
                {
                    if (((Persons)(dlgLogin.BoxUser.SelectedItem)).Pasword == null)
                    {
                        if (dlgLogin.BoxPassword.Password == "") break;
                    }
                    else if (((Persons)(dlgLogin.BoxUser.SelectedItem)).Pasword == dlgLogin.BoxPassword.Password) break;
                }
                MessageBox.Show("Не верный пароль", "Ошибка авторизации", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
            if (curUser.id == ((Persons)(dlgLogin.BoxUser.SelectedItem)).id) return;
            User = (from c in content.Persons where c.id == ((Persons)(dlgLogin.BoxUser.SelectedItem)).id select c).First();
            CurrentDate = (DateTime)dlgLogin.BoxDate.SelectedDate;
            Window_Loaded(sender, e);
            MenuUser.Header = User.Email;
        }

        private void MenProfile_Click(object sender, RoutedEventArgs e)
        {
            DlgPerson dlg = new DlgPerson(content, User);
            dlg.CheckBox_Admin.IsEnabled = (User.IsAdmin != null);
            if (dlg.ShowDialog() == false) return;
            MyTask_BoxOwner.ItemsSource = content.Persons.ToList();
            Task_BoxOwner.ItemsSource = content.Persons.ToList();
            Company_BoxOwner.ItemsSource = content.Persons.ToList();
            List_BoxOwner.ItemsSource = content.Persons.ToList();
            categoriesViewSource.Source = content.Categories.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Gantt gantt = new Gantt(content);
            gantt.Show();
            }
    }

    public class ObservableModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    public class TaskModel : ObservableModel
    {
        private string _taskId;
        public string TaskID
        {
            get
            {
                return _taskId;
            }
            set
            {
                if (_taskId != value)
                {
                    _taskId = value;
                    this.NotifyPropertyChanged("TaskID");
                }
            }
        }
        private string _tasks;
        public string Tasks
        {
            get
            {
                return _tasks;
            }
            set
            {
                if (_tasks != value)
                {
                    _tasks = value;
                    this.NotifyPropertyChanged("Tasks");
                }
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }
        private ProjectTaskConstraintType _constraintType;
        public ProjectTaskConstraintType ConstraintType
        {
            get
            {
                return _constraintType;
            }
            set
            {
                if (_constraintType != value)
                {
                    _constraintType = value;
                    this.NotifyPropertyChanged("ConstraintType");
                }
            }
        }
        private DateTime? _constraintDate;
        public DateTime? ConstraintDate
        {
            get
            {
                return _constraintDate;
            }
            set
            {
                if (_constraintDate != value)
                {
                    _constraintDate = value;
                    this.NotifyPropertyChanged("ConstraintDate");
                }
            }
        }
        private ProjectDurationFormat _durationFormat;
        public ProjectDurationFormat DurationFormat
        {
            get
            {
                return _durationFormat;
            }
            set
            {
                if (_durationFormat != value)
                {
                    _durationFormat = value;
                    this.NotifyPropertyChanged("DurationFormat");
                }
            }
        }
        private TimeSpan _durationInHours;
        public TimeSpan DurationInHours
        {
            get
            {
                return _durationInHours;
            }
            set
            {
                if (_durationInHours != value)
                {
                    _durationInHours = value;
                    this.NotifyPropertyChanged("DurationInHours");
                }
            }
        }
        private DateTime _start;
        public DateTime Start
        {
            get
            {
                return _start;
            }
            set
            {
                if (_start != value)
                {
                    _start = value;
                    this.NotifyPropertyChanged("Start");
                }
            }
        }
        private bool _isMilestone = false;
        public bool IsMilestone
        {
            get
            {
                return _isMilestone;
            }
            set
            {
                if (_isMilestone != value)
                {
                    _isMilestone = value;
                    this.NotifyPropertyChanged("IsMilestone");
                }
            }
        }
        private bool _isInProgress = true;
        public bool IsInProgress
        {
            get
            {
                return _isInProgress;
            }
            set
            {
                if (_isInProgress != value)
                {
                    _isInProgress = value;
                    this.NotifyPropertyChanged("IsInProgress");
                }
            }
        }
        private DateTime? _deadlineDate;
        public DateTime? DeadlineDate
        {
            get
            {
                return _deadlineDate;
            }
            set
            {
                if (_deadlineDate != value)
                {
                    _deadlineDate = value;
                    this.NotifyPropertyChanged("DeadlineDate");
                }
            }
        }
        private bool _isUndetermined = false;
        public bool IsUndetermined
        {
            get
            {
                return _isUndetermined;
            }
            set
            {
                if (_isUndetermined != value)
                {
                    _isUndetermined = value;
                    this.NotifyPropertyChanged("IsUndetermined");
                }
            }
        }
        private string _resourceName;
        public string ResourceName
        {
            get
            {
                return _resourceName;
            }
            set
            {
                if (_resourceName != value)
                {
                    _resourceName = value;
                    this.NotifyPropertyChanged("ResourceName");
                }
            }
        }
    }

    public class ListBackedProjectViewModel : ObservableModel
    {
        public ListBackedProjectViewModel()
        {
            // this.DownloadDataSource();
        }
        private ObservableCollection<TaskModel> _tasks;



        public ObservableCollection<TaskModel> Tasks
        {
            get
            {
                return _tasks;
            }
            set
            {
                if (value != null)
                {
                    _tasks = value;
                }
                NotifyPropertyChanged("Tasks");
            }
        }
        public void DownloadDataSource(IQueryable<Tasks> query)
        {
            ObservableCollection<TaskModel> dataSource = new ObservableCollection<TaskModel>();
            var TaskList =query.ToList();
            foreach (var item in TaskList)
            {
                if (item.Due_Date == null || item.Start_Date == null) continue;
                TaskModel task = new TaskModel();
                task.TaskID = item.Id.ToString();
                task.Name = item.Name.ToString();
                task.IsInProgress = (item.Status != "В ожидании");
                if (item.Due_Date != null && item.Start_Date != null) task.Start = (DateTime)item.Start_Date;
                task.IsMilestone = false;
                if (item.Due_Date != null && item.Start_Date != null) task.DurationInHours = (TimeSpan)(item.Due_Date - item.Start_Date); //TimeSpan.FromHours(el.Element("DurationInHours").GetDouble());
                task.IsUndetermined = (item.Due_Date == null || item.Start_Date == null);
                task.ResourceName = item.Persons.Email;
                task.DurationFormat = ProjectDurationFormat.Days;
                task.DeadlineDate = item.Due_Date;
                dataSource.Add(task);
            }
            this._tasks = dataSource;
        }
    }



}
