using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Xml.Linq;
using Infragistics.Controls.Schedules;


namespace AppProjects
{
    /// <summary>
    /// Логика взаимодействия для Gantt.xaml
    /// </summary>
    public partial class Gantt : Window
    {

       public ListBackedProjectViewModel viewmodel { get; set; }
        public Gantt(ProjectDB content)
        {
         //   InitializeComponent();

            viewmodel = new ListBackedProjectViewModel();
            viewmodel.DownloadDataSource(content);
            gantt.Project.DataContext = viewmodel;

        //    ListBackedProject l = new ListBackedProject();
        //  l.TaskItemsSource = model;
        //   Gantt1.Project = new ListBackedProject() ;
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
        public void DownloadDataSource(ProjectDB content)
        {
            ObservableCollection<TaskModel> dataSource = new ObservableCollection<TaskModel>();
            var TaskList = (from c in content.Tasks select c).ToList() ; 
            foreach (var item in TaskList)
            {
                TaskModel task = new TaskModel();
                task.TaskID = item.Id.ToString();
                task.Name = item.Name.ToString();
                task.IsInProgress = (item.Status != "В ожидании");
                if (item.Due_Date != null && item.Start_Date != null) task.Start = ((DateTime)item.Start_Date).ToUniversalTime();
                task.IsMilestone = false;
                if(item.Due_Date != null && item.Start_Date != null) task.DurationInHours = (TimeSpan)(item.Due_Date - item.Start_Date); //TimeSpan.FromHours(el.Element("DurationInHours").GetDouble());
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
