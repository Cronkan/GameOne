using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace GameOne
{
    class DataHandler
    {
        public static object importFromJson<T>(string path)
        {
            try
            {
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not import file!", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Could not import file!", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw e;
            }
        }
        public static void exportToJson(object data, String filename)
        {

            try
            {
                var json = JsonConvert.SerializeObject(data);
                writeTextToFile(json, filename);
              
            }
            catch (JsonSerializationException e)
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
