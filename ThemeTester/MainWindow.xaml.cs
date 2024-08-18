using System.Collections.ObjectModel;
using System.Windows;

namespace ThemeTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Person> SampleData { get; set; }
        private bool isLightTheme = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            SampleData = new ObservableCollection<Person>
            {
                new Person { Id = 1, Name = "John Doe", Age = 30, Occupation = "Developer" },
                new Person { Id = 2, Name = "Jane Smith", Age = 28, Occupation = "Designer" },
                new Person { Id = 3, Name = "Mike Johnson", Age = 35, Occupation = "Manager" },
                new Person { Id = 4, Name = "Emily Brown", Age = 25, Occupation = "Analyst" }
            };
        }

        private void ToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            isLightTheme = !isLightTheme;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            var uri = new System.Uri(isLightTheme ? "LightTheme.xaml" : "DarkTheme.xaml", System.UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);

            this.UpdateLayout();
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }
    }
}