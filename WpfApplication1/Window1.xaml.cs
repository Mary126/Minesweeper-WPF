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
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        int height, width, mines;
        bool custom = false;
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (custom)
            {
                if (visota.Text == " " || shirina.Text == " " || mini.Text == " ")
                {
                    MessageBox.Show("Вы не ввели все параметры");
                }
                else
                {
                    height = int.Parse(visota.Text);
                    width = int.Parse(shirina.Text);
                    mines = int.Parse(mini.Text);
                    this.DialogResult = true;
                }
            }
            else
            {
                DialogResult = true;
            }
        }
        public int x
        {
            get {return height; }
        }
        public int y
        {
            get { return width; }
        }
        public int Mines
        {
            get { return mines; }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).Content.ToString() == "Просто")
            {
                height = 10;
                width = 10;
                mines = 10;
            }
            else if ((sender as RadioButton).Content.ToString() == "Средне")
            {
                height = 16;
                width = 16;
                mines = 40;
            }
            else if ((sender as RadioButton).Content.ToString() == "Профессионал")
            {
                height = 16;
                width = 30;
                mines = 99;
            }
            else if((sender as RadioButton).Content.ToString() == "Клиентура")
            {
                custom = true;
            }
        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            custom = false;
        }

    }
}
