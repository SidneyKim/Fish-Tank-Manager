using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UtilityLibrary.Misc;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Ftm.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string path;
        SQLite.Net.SQLiteConnection conn;
        DispatcherTimer timer;
        int count = 0;
        DateTime nisTime;
        
        public MainPage()
        {
            this.InitializeComponent();

            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");

            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            conn.CreateTable<DatabaseModel>();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // 1 sec
            timer.Tick += Timer_Tick;
            timer.Start();

            nisTime = NisTime.GetFastestNISTDate();
            System.Diagnostics.Debug.WriteLine(nisTime.ToString());
        }

        private void Timer_Tick(object sender, object e)
        {
            textBox.Text = (count++).ToString();

            var s = conn.Insert(new DatabaseModel()
            {
                Content = textBox.Text
            });

            System.Diagnostics.Debug.WriteLine(textBox.Text);
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            if(this.timer.IsEnabled)
            {
                timer.Stop();
                this.count = 0;
                this.button.Content = "Start";
            }
            else
            {
                timer.Start();
                this.button.Content = "Stop";
            }
        }

        private void btnRetrieve_Click(object sender, RoutedEventArgs e)
        {            
            var query = conn.Table<DatabaseModel>();

            listView.ItemsSource = query.Select(itm => itm.Content).ToList();
        }

        private void btnTruncate_Click(object sender, RoutedEventArgs e)
        {
            conn.DeleteAll<DatabaseModel>();

            listView.ItemsSource = null;                        
        }
    }
}
