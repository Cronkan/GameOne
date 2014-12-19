using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Windows;

namespace GameOne
{
    public class Watcher
    {
        public Watcher(Game game)
        {
            this.game = game;
        }

        public Game game { get; set; }

        public FileSystemWatcher watcher { get; set; }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Run()
        {
            // Create a new FileSystemWatcher and set its properties.
            watcher = new FileSystemWatcher();
            watcher.Path = Directory.GetCurrentDirectory();
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "game.json";

            // Add event handlers.
            watcher.Changed += OnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
        }

        // Define the event handlers. 
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Debug.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            //MainWindow.Dispatcher.BeginInvoke(new Action(() =>{ }));
            Application.Current.Dispatcher.Invoke(() => game.ContinueGame());

            watcher.EnableRaisingEvents = false;
            //  Invoke((MethodInvoker)delegate { MyFunction(); }); this.game.ContinueGame();
            watcher.EnableRaisingEvents = true;
        }
    }
}