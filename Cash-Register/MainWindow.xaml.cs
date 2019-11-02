using System;
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
using System.Windows.Threading;
using System.ComponentModel;
using System.IO;
using System.Collections.ObjectModel;

namespace Cash_Register
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;

        private string directory;
        private string[] new_line = { "\r\n" };

        private List<string> product_name_list;
        private List<int> product_price_list;

        private string last_product;
        private int last_product_index;
        private int last_count;

        private List<Cart> items;

        public MainWindow()
        {
            InitializeComponent();
            setup_Timer();

            directory = Environment.CurrentDirectory;

            subtotal.Content = "0 円";
            load_products();
        }

        private void load_products()
        {
            try
            {
                using (StreamReader sr = new StreamReader("C:\\Users\\Mokuichi147\\source\\repos\\Cash-Register\\Cash-Register\\product_list.txt"))
                {
                    String line = sr.ReadToEnd();
                    string[] products = line.Split(new_line, StringSplitOptions.None);
                    product_name_list = new List<string>();
                    product_price_list = new List<int>();
                    for (int i = 0; i < products.Length; i++)
                    {
                        product_name_list.Add(products[i].Split(' ')[0]);
                        product_price_list.Add(int.Parse(products[i].Split(' ')[1]));
                    }
                    if (product_name_list.Count > 5)
                    {
                        product_up.IsEnabled = true;
                        product_down.IsEnabled = true;
                    }
                    else
                    {
                        product_up.IsEnabled = false;
                        product_down.IsEnabled = false;
                    }
                    count_up.IsEnabled = false;
                    switch (product_name_list.Count)
                    {
                        case 0:
                            break;
                        case 1:
                            product_0.Content = product_name_list[0];
                            product_1.IsEnabled = false;
                            product_2.IsEnabled = false;
                            product_3.IsEnabled = false;
                            product_4.IsEnabled = false;
                            break;
                        case 2:
                            product_0.Content = product_name_list[0];
                            product_1.Content = product_name_list[1];
                            product_2.IsEnabled = false;
                            product_3.IsEnabled = false;
                            product_4.IsEnabled = false;
                            break;
                        case 3:
                            product_0.Content = product_name_list[0];
                            product_1.Content = product_name_list[1];
                            product_2.Content = product_name_list[2];
                            product_3.IsEnabled = false;
                            product_4.IsEnabled = false;
                            break;
                        case 4:
                            product_0.Content = product_name_list[0];
                            product_1.Content = product_name_list[1];
                            product_2.Content = product_name_list[2];
                            product_3.Content = product_name_list[3];
                            product_4.IsEnabled = false;
                            break;
                        case 5:
                            product_0.Content = product_name_list[0];
                            product_1.Content = product_name_list[1];
                            product_2.Content = product_name_list[2];
                            product_3.Content = product_name_list[3];
                            product_4.Content = product_name_list[4];
                            break;
                    }
                    Console.WriteLine(line);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("ファイルが読み込めませんでした");
                Console.WriteLine(e.Message);
            }
        }

        private void setup_Timer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += new EventHandler(clock_Timer);

            _timer.Start();

            this.Closing += new CancelEventHandler(stop_Timer);
        }

        private void clock_Timer(object sender, EventArgs e)
        {
            Clock.Content = DateTime.Now.ToString("現在時刻[ HH:mm ]");
        }

        private void stop_Timer(object sender, EventArgs e)
        {
            _timer.Stop();
        }

        private void count_down_Click(object sender, RoutedEventArgs e)
        {
            int count0 = int.Parse(count_0.Content.ToString());
            int count1 = int.Parse(count_1.Content.ToString());
            int count2 = int.Parse(count_2.Content.ToString());
            int count3 = int.Parse(count_3.Content.ToString());

            count_0.Content = count0 + 4;
            count_1.Content = count1 + 4;
            count_2.Content = count2 + 4;
            count_3.Content = count3 + 4;

            if (!count_up.IsEnabled)
            {
                count_up.IsEnabled = true;
            }
        }

        private void count_up_Click(object sender, RoutedEventArgs e)
        {
            int count0 = int.Parse(count_0.Content.ToString());
            int count1 = int.Parse(count_1.Content.ToString());
            int count2 = int.Parse(count_2.Content.ToString());
            int count3 = int.Parse(count_3.Content.ToString());

            count_0.Content = count0 - 4;
            count_1.Content = count1 - 4;
            count_2.Content = count2 - 4;
            count_3.Content = count3 - 4;

            if (count0 -4 == 1)
            {
                count_up.IsEnabled = false;
            }
        }

        private void count_Click(object sender, RoutedEventArgs e)
        {
            int num = int.Parse(((Button)sender).Content.ToString());
            last_count = num;
            count_0.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 180 / (float)255, 180 / (float)255, 180 / (float)255));
            count_1.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 180 / (float)255, 180 / (float)255, 180 / (float)255));
            count_2.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 180 / (float)255, 180 / (float)255, 180 / (float)255));
            count_3.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 180 / (float)255, 180 / (float)255, 180 / (float)255));
            if (count_0.Content.ToString() == last_count.ToString())
                count_0.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 30 / (float)255, 180 / (float)255, 180 / (float)255));
            else if (count_1.Content.ToString() == last_count.ToString())
                count_1.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 30 / (float)255, 180 / (float)255, 180 / (float)255));
            else if (count_2.Content.ToString() == last_count.ToString())
                count_2.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 30 / (float)255, 180 / (float)255, 180 / (float)255));
            else if (count_3.Content.ToString() == last_count.ToString())
                count_3.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 30 / (float)255, 180 / (float)255, 180 / (float)255));
        }

        private void product_Click(object sender, RoutedEventArgs e)
        {
            last_product = ((Button)sender).Content.ToString();
            for (int i = 0; i < product_name_list.Count; i++)
            {
                if (product_name_list[i] == last_product)
                    last_product_index = i;
            }
            product_0.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 180 / (float)255, 180 / (float)255, 180 / (float)255));
            product_1.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 180 / (float)255, 180 / (float)255, 180 / (float)255));
            product_2.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 180 / (float)255, 180 / (float)255, 180 / (float)255));
            product_3.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 180 / (float)255, 180 / (float)255, 180 / (float)255));
            product_4.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 180 / (float)255, 180 / (float)255, 180 / (float)255));
            if (last_product_index == 0)
                product_0.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 30 / (float)255, 180 / (float)255, 180 / (float)255));
            else if (last_product_index == 1)
                product_1.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 30 / (float)255, 180 / (float)255, 180 / (float)255));
            else if (last_product_index == 2)
                product_2.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 30 / (float)255, 180 / (float)255, 180 / (float)255));
            else if (last_product_index == 3)
                product_3.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 30 / (float)255, 180 / (float)255, 180 / (float)255));
            else if (last_product_index == 4)
                product_4.Background = new SolidColorBrush(Color.FromScRgb(100 / 100, 30 / (float)255, 180 / (float)255, 180 / (float)255));
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void button14_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button15_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (last_product != null && last_count != 0)
            {
                if (shopping_cart.SelectedIndex == -1)
                {
                    items = new List<Cart>();
                    int total = product_price_list[last_product_index] * last_count;
                    //items.Add(new Cart() { name = last_product, count = last_count, subtotal = total });
                    shopping_cart.Items.Add(new Cart() { name = last_product, count = last_count, subtotal = total }); ;
                }
                else
                {
                    int total = product_price_list[last_product_index] * last_count;
                    shopping_cart.Items.Add(new Cart() { name = last_product, count = last_count, subtotal = total });
                }
                subtotal.Content = (last_count).ToString() + " 円";
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void shopping_cart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class Cart
    {
        public string name { get; set; }
        public int count { get; set; }
        public int subtotal { get; set; }
    }
}
