using System;
using System.IO;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Sharp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public class Item
    {
        public string  name { get; set; }

        public string value { get; set; }

    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            items = new List<Item>();

        }

        public List<Item> items = new List<Item>();
        public string randomString = "empty";

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "data";
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML файл|*.xml";
            dataGrid.ItemsSource = null;
            bool? result = dlg.ShowDialog();
            string filename = null;
            if (result == true)
            {
                filename = dlg.FileName;
            }
            else
            {
                MessageBox.Show("File not found");
                return;
            }

            XmlSerializer x = new XmlSerializer(typeof(List<Item>));
            FileStream fs = new FileStream(filename, FileMode.Open);
            items.Clear();
            try
            {
                items = (List<Item>)x.Deserialize(fs);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Неправильный формат файла : \n"+ex.Message);
            }
            dataGrid.ItemsSource = items;

        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "data"; 
            dlg.DefaultExt = ".xml"; 
            dlg.Filter = "XML файл|*.xml";

            bool? result = dlg.ShowDialog();
            string filename = null;
            if (result == true)
            {
                filename = dlg.FileName;
            }
            else
            {
                MessageBox.Show("File not found");
                return;
            }

            XmlSerializer x = new XmlSerializer(typeof(List<Item>));
            TextWriter writer = new StreamWriter(filename);
            x.Serialize(writer, items);
        }

        private void GenerateClick(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            randomString = "";
            items.Clear();
            dataGrid.ItemsSource = null;
            for (int i = 0; i < 20; i++)
            {
                int number = rnd.Next(1, 40) + rnd.Next(1,40);
                Item item = new Item() { name = "value " + i.ToString(), value = number.ToString() };
                items.Add(item);
            }

            int max = rnd.Next(1, 40) + rnd.Next(1, 40);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < max; i++)
            {
                char randomChar = Convert.ToChar(rnd.Next(33, 126));
                sb.Append(randomChar);
            }

            randomString = sb.ToString();
            items.Add(new Item() { name = "random string", value = randomString });
            dataGrid.ItemsSource = items;
            SaveButton.IsEnabled = true;
        }
    }
}
