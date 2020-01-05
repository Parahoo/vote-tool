using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
using System.Xml.Serialization;


namespace 投票统计器
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadUnits();
            CheckUndoState();
        }

        private void LoadUnits()
        {
            if (File.Exists("votusr.xml"))
            {
                try
                {
                    LoadFromXml();
                }
                catch
                {
                    LoadDefaultVotUser();
                }

            }
            else
                LoadDefaultVotUser();
        }

        private void LoadDefaultVotUser()
        {
            for (int i = 0; i < 25; i++)
            {
                unitContainer.Children.Add(new CountUnit());
            }
        }

        private void LoadFromXml()
        {
            List<VotUser> usrs = new List<VotUser>();
            XmlSerializer xs = new XmlSerializer(typeof(List<VotUser>));
            using (XmlReader s = XmlReader.Create("votusr.xml"))
                usrs = (List<VotUser>)xs.Deserialize(s);
            foreach(VotUser usr in usrs)
            {
                CountUnit curUnit = new CountUnit
                {
                    VotUser = usr
                };
                unitContainer.Children.Add(curUnit);
            }
        }

        private void SaveVotUserToXml()
        {
            List<VotUser> usrs = new List<VotUser>();
            foreach (var unit in unitContainer.Children)
            {
                CountUnit curUnit = unit as CountUnit;
                usrs.Add(curUnit.VotUser);
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";
            settings.NewLineChars = "\r\n";
            settings.Encoding = Encoding.UTF8;
            //settings.OmitXmlDeclaration = true;  // 不生成声明头  

            XmlSerializer xs = new XmlSerializer(typeof(List<VotUser>));
            using (XmlWriter s = XmlWriter.Create("votusr.xml", settings))
            {
                xs.Serialize(s, usrs);
            }
        }

        private void SaveVotUserToExl()
        {
            List<VotUser> usrs = new List<VotUser>();
            foreach (var unit in unitContainer.Children)
            {
                CountUnit curUnit = unit as CountUnit;
                usrs.Add(curUnit.VotUser);
            }
            ExlLoader.SaveToFile(System.Windows.Forms.Application.StartupPath+"\\votusr.xlsx", usrs);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach(var votusr in unitContainer.Children)
            {
                CountUnit a = votusr as CountUnit;
                a.ResetCount();
            }
        }

        private void CheckUndoState()
        {
            //undoBtn.IsEnabled = CountUnit.CheckCanUndo();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CountUnit.UndoCount();
            CheckUndoState();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveVotUserToXml();
            SaveVotUserToExl();
        }
    }
}
