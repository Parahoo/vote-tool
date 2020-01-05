using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace 投票统计器
{
    public class VotUser : INotifyPropertyChanged
    {
        private int countNum = 0;
        private string userName = "***";

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public int CountNum {
            get => countNum;
            set {
                countNum = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountNum"));
            }
        }
        public string UserName {
            get => userName;
            set {
                userName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserName"));
            }
        }

        public void VotPlus()
        {
            CountNum = CountNum + 1;
        }
        public void UndoVotPlus()
        {
            CountNum--;
        }
        public void ResetVot()
        {
            CountNum = 0;
        }
    }
    /// <summary>
    /// CountUnit.xaml 的交互逻辑
    /// </summary>
    public partial class CountUnit : UserControl
    {
        VotUser votUser = new VotUser();
        public CountUnit()
        {
            InitializeComponent();
            DataContext = VotUser;
        }

        public void SetVotUser(VotUser user)
        {
            VotUser = user;
            DataContext = VotUser;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VotUser.CountNum++;
            undoList.Push(this);
        }

        static Stack<CountUnit> undoList = new Stack<CountUnit>(10);

        public VotUser VotUser {
            get => votUser;
            set {
                votUser = value;
                DataContext = votUser;
            } }

        static public void UndoCount()
        {
            if(undoList.Count > 0)
            undoList.Pop().UndoVotPlus();
        }

        private void UndoVotPlus()
        {
            votUser.UndoVotPlus();
            Task.Run(() => {
                Dispatcher.Invoke(() => { label.Foreground = Brushes.Red;});                
                Thread.Sleep(1800);
                Dispatcher.Invoke(() => { label.Foreground = Brushes.Black;});
                
            });
        }

        static public bool CheckCanUndo()
        {
            return undoList.Count > 0;
        }

        public void ResetCount()
        {
            VotUser.ResetVot();
        }

        private void cfgBtn_Click(object sender, RoutedEventArgs e)
        {
            //if (userInput.Visibility == Visibility.Visible)
            //    userInput.Visibility = Visibility.Hidden;
            //else
            //    userInput.Visibility = Visibility.Visible;
        }

        private void userInput_MouseLeave(object sender, MouseEventArgs e)
        {
            userInput.Visibility = Visibility.Hidden;
            votBtn.Focus();
        }

        private void cfgBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            userInput.Visibility = Visibility.Visible;
            nameInput.Focus();
            nameInput.SelectAll();
        }
    }
}
