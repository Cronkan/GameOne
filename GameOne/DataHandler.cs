using System;
using System.IO;
using System.Threading;
using System.Windows;
using Newtonsoft.Json;

namespace GameOne
{
    internal class DataHandler
    {
        public static object importFromJson<T>(string path)
        {
            try

            {
                Thread.Sleep(50);
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not load gamestate! Creating new game", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static T jsonToObject<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not import fileee!", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw e;
            }
        }

        public static void exportToJson(object data, String filename)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data);
                Thread.Sleep(50);
                writeTextToFile(json, filename);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to serialize JSON: " + e, "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void writeTextToFile(String text, String path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                File.WriteAllText(path, text);
            }
            catch (IOException ioex)
            {
                MessageBox.Show("Save unsuccessful: " + ioex, "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}