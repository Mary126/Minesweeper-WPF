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

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button[,] buttons;
        int[,] arr; //сылка на двухмерный массив
        int x, y, bombs, open = 0, flag = 0, Flag;
        Random b = new Random();
        bool[,] click;
        //ImageBrush flag_image = new ImageBrush();
        ImageBrush bomb = new ImageBrush();
        bool start_check = false;

        public void GenerateField()
        {
            buttons = new Button[y, x];
            arr = new int[y, x];
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            click = new bool[y, x];
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    click[i, j] = true;
                    arr[i, j] = 0;
                }
            }

            for (int i = 0; i < y; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < x; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    Button b = new Button();
                    b.Click += new RoutedEventHandler(Open_Field);
                    b.MouseRightButtonDown += new MouseButtonEventHandler(Put_a_flag);
                    b.Content = " ";
                    grid.Children.Add(b);
                    Grid.SetColumn(b, j);
                    Grid.SetRow(b, i);
                    buttons[i, j] = b;
                }
            }
        }

        public void CheckBomb(int a, int b)
        {
            if (arr[a, b] > -1)
            {
                arr[a, b] += 1;
                // MessageBox.Show("erw");
            }
        }

        public void GenerateBombs()
        {
            for (int i = 0; i < bombs; i++)
            {
                bool check = true;
                int x_pos = 0, y_pos = 0;
                while (check)
                {
                    x_pos = b.Next(0, x);
                    y_pos = b.Next(0, y);
                    if (arr[y_pos, x_pos] != -1) 
                    {
                        arr[y_pos, x_pos] = -1;
                        check = false;
                    }
                }
                if (y_pos != y - 1 && x_pos != x - 1) CheckBomb(y_pos + 1, x_pos + 1);
                if (y_pos != 0 && x_pos != 0) CheckBomb(y_pos - 1, x_pos - 1);
                if (y_pos != y - 1 && x_pos != 0) CheckBomb(y_pos + 1, x_pos - 1);
                if (y_pos != 0 && x_pos != x - 1) CheckBomb(y_pos - 1, x_pos + 1);
                if (x_pos != x - 1) CheckBomb(y_pos, x_pos + 1);
                if (y_pos != y - 1) CheckBomb(y_pos + 1, x_pos);
                if (x_pos != 0) CheckBomb(y_pos, x_pos - 1);
                if (y_pos != 0) CheckBomb(y_pos - 1, x_pos);
            }
        }

        public void Fill()
        {
            //GenerateBombs();
        }

        private void Open_all_none(int y_pos, int x_pos)
        {
            if (y_pos == y || x_pos == x || x_pos == -1 || y_pos == -1) return;
            if (buttons[y_pos, x_pos].Background == null) return;
            if (arr[y_pos, x_pos] != 0)
            {
                buttons[y_pos, x_pos].Background = null;
                buttons[y_pos, x_pos].IsEnabled = false;
                buttons[y_pos, x_pos].Content = arr[y_pos, x_pos];
                open++;
                return;
            }
            buttons[y_pos, x_pos].Background = null;
            buttons[y_pos, x_pos].IsEnabled = false;
            open++;
            Open_all_none(y_pos + 1, x_pos + 1);
            Open_all_none(y_pos - 1, x_pos - 1);
            Open_all_none(y_pos + 1, x_pos - 1);
            Open_all_none(y_pos - 1, x_pos + 1);
            Open_all_none(y_pos, x_pos + 1);
            Open_all_none(y_pos + 1, x_pos);
            Open_all_none(y_pos, x_pos - 1);
            Open_all_none(y_pos - 1, x_pos);
        }

        private void Disable_Field()
        {
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (arr[i, j] == -1) { buttons[i, j].Background = bomb; buttons[i, j].Content = " "; }
                    else
                    {
                        if (arr[i, j] == 0) buttons[i, j].Content = " ";
                        else buttons[i, j].Content = arr[i, j];
                        buttons[i, j].IsEnabled = false;
                    }
                }
            }
        }
        private void Open_Field(object sender, RoutedEventArgs e)
        {
            Button obj = (sender as Button);
            int y_pos = Grid.GetRow(obj);
            int x_pos = Grid.GetColumn(obj);
            if (!start_check)
            {
                bool bomb_ck = false;
                while (!bomb_ck)
                {
                    GenerateBombs();
                    if (arr[y_pos, x_pos] != -1)
                    {
                        bomb_ck = true;
                    }
                }
                start_check = true;
            }
            if (obj.Content.ToString() != "Flag")
            {
                if (arr[y_pos, x_pos] == -1)
                {
                    obj.Background = bomb;
                    //obj.IsEnabled = false;
                    Disable_Field();
                    MessageBox.Show("You loose");
                    //this.Close();
                }
                else if (arr[y_pos, x_pos] == 0)
                {
                    Open_all_none(y_pos, x_pos);
                }
                else
                {
                    obj.IsEnabled = false;
                    obj.Background = null;
                    obj.Content = arr[y_pos, x_pos];
                    open++;
                    if (open == (x * y)-bombs)
                    {
                        MessageBox.Show("You win");

                    }
                }
            }
        }

        private void Put_a_flag(object sender, MouseButtonEventArgs e)
        {
            Button obj = (sender as Button);
            int y_pos = Grid.GetRow(obj);
            int x_pos = Grid.GetColumn(obj);
            if (click[y_pos, x_pos])
            {
                if (arr[y_pos, x_pos] == -1) flag++;
                else flag--;
                Flag--;
                Status.Text = Flag.ToString();
                obj.Content = "Flag";
                if (flag == bombs)
                {
                    MessageBox.Show("You Win");
                    //this.Close();
                }
                click[y_pos, x_pos] = false;
            }
            else
            {
                obj.Content = " ";
                if (arr[y_pos, x_pos] == -1) flag--;
                click[y_pos, x_pos] = true;
                Flag++;
                Status.Text = Flag.ToString();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            bomb.ImageSource = new BitmapImage(new Uri("../../pic/Bomb.png", UriKind.Relative));
            bomb.Stretch = Stretch.Uniform;
            Status.Text = Flag.ToString();
            Window1 n = new Window1();
            n.ShowDialog();
            if (n.DialogResult == true)
            {
                x = n.x;
                y = n.y;
                bombs = n.Mines;
                grid.Children.Clear();
                GenerateField();
                start_check = false;
                Flag = bombs;
                Status.Text = Flag.ToString();
                Fill();
            }
        }

        private void Nastroyki_Click(object sender, RoutedEventArgs e)
        {
            Window1 n = new Window1();
            n.ShowDialog();
            if (n.DialogResult == true)
            {
                x = n.x;
                y = n.y;
                bombs = n.Mines;
                grid.Children.Clear();
                GenerateField();
                start_check = false;
                Flag = bombs;
                Status.Text = Flag.ToString();
                Fill();
            }
           
        }
        private void New_Game(object sender, RoutedEventArgs e)
        {
            grid.Children.Clear();
            GenerateField();
            start_check = false;
            Flag = bombs;
            Status.Text = Flag.ToString();
            Fill();
        }
    }
}
