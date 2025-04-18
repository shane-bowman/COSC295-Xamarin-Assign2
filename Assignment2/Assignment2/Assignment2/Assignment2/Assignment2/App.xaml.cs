using Xamarin.Forms;

namespace Assignment2
{
    public partial class App : Application
    {
        static Game_Database database;

        public static Game_Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Game_Database(DependencyService.Get<IFileHelper>().GetLocalFilePath("games.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new OpponentsPage());
        }

        protected override void OnStart() { }
        protected override void OnSleep() { }
        protected override void OnResume() { }
    }

    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}