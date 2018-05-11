using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Coursuch
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string DEFAULT_FILEPATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Automation\\temp";
        public static string DEFAULT_FILE = "saves.json";


        public MainWindow()
        {
            InitializeComponent();

            if (FileWorker.ckeckFile(DEFAULT_FILEPATH + "\\" + DEFAULT_FILE))
            {
                actionlists = new ObservableCollection<ActionList>();
                executor = new Executor();

                executor.Actions = actionlists;

                try
                {
                    ObservableCollection<ActionList> lists = FileWorker.read<ObservableCollection<ActionList>>(DEFAULT_FILEPATH + "\\" + DEFAULT_FILE);

                    foreach (var it in lists)
                    {
                        actionlists.Add(it);
                    }

                }
                catch
                {
                }
            }
            else
            {
                actionlists = new ObservableCollection<ActionList>();
            }

            actionLists.ItemsSource = actionlists;
        }

        private ObservableCollection<ActionList> actionlists;
        private Executor executor;

        public bool tryCreateMouseAction(out MouseAction mouseAction)
        {
            int startTime;
            int executionTime;
            int x;
            int y;

            if (int.TryParse(mouseStartTime.Text, out startTime) &&
            int.TryParse(mouseExecutionTime.Text, out executionTime) &&
            int.TryParse(mouseX.Text, out x) &&
            int.TryParse(mouseY.Text, out y) &&
            mouseFlag.SelectedItem != null &&
            startTime >= 0 &&
            executionTime >= 0 &&
            x >= 0 &&
            y >= 0)
            {
                mouseAction = new MouseAction
                {
                    StartTime = startTime,
                    ExecutionTime = executionTime,
                    X = x,
                    Y = y,
                    MouseFlag = (MouseFlags)mouseFlag.SelectedValue
                };
                return true;
            }

            mouseAction = null;
            return false;
        }

        public bool tryCreateKeyboardAction(out KeyBoardAction keyBoardAction)
        {
            int startTime;
            int executionTime;

            if (int.TryParse(keyboardStartTime.Text, out startTime) &&
                int.TryParse(keyboardExecutionTime.Text, out executionTime) &&
                keyboardFlag.SelectedItem != null &&
                keyboardKey.SelectedItem != null &&
                startTime >= 0 &&
                executionTime >= 0)
            {
                keyBoardAction = new KeyBoardAction
                {
                    StartTime = startTime,
                    ExecutionTime = executionTime,
                    Flag = (KeyFlag)keyboardFlag.SelectedValue,
                    Key = (Key)keyboardKey.SelectedValue
                };
                return true;
            }

            keyBoardAction = null;
            return false;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ActionList list = new ActionList();

            MouseAction mouseAction;
            KeyBoardAction keyBoardAction;

            if (tryCreateMouseAction(out mouseAction))
            {
                list.Actions.Add(mouseAction);
            }

            if (tryCreateKeyboardAction(out keyBoardAction))
            {
                list.Actions.Add(keyBoardAction);
            }

            actionlists.Add(list);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                FileWorker.creadeDirrectory(DEFAULT_FILEPATH);
                FileWorker.write(DEFAULT_FILEPATH + "\\" + DEFAULT_FILE, actionlists);
            }
            catch
            {
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    foreach (var i in new ObservableCollection<ActionList>(FileWorker.read<ObservableCollection<ActionList>>(openFileDialog.FileName)))
                    {
                        actionlists.Add(i);
                    }
                }
                catch
                {
                }
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    ObservableCollection<ActionList> actions = new ObservableCollection<ActionList>();
                    foreach (var i in actionLists.SelectedItems)
                    {
                        actions.Add((ActionList)i);
                    }
                    FileWorker.write(saveFileDialog.FileName, actions);
                }
                catch
                {
                }
            }
        }

        public static readonly RoutedUICommand DeleteCommand = new RoutedUICommand("Удалить", "DeleteCommand", typeof(MainWindow));

        private void DeleteCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            actionlists.Remove((ActionList)e.Parameter);
        }

        private void DeleteCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static readonly RoutedUICommand ExecuteCommand = new RoutedUICommand("Запустить", "ExecuteCommand", typeof(MainWindow));

        private void ExecuteCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ((ActionList)e.Parameter).IsExecute = !((ActionList)e.Parameter).IsExecute;

            if (((ActionList)e.Parameter).IsExecute)
            {
                ((Button)e.OriginalSource).Content = "Остановить";
            }
            else
            {
                ((Button)e.OriginalSource).Content = "Запустить";
            }
        }

        private void ExecuteCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static readonly RoutedUICommand DeleteActionCommand = new RoutedUICommand("Удалить действие", "DeleteActionCommand", typeof(MainWindow));

        private void DeleteActionCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var a = ((AbstractAction)e.Parameter);

            a.parentList.Actions.Remove(a);

        }

        private void DeleteActionCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            ((ComboBox)sender).SelectedValue = e.Key;
        }

        private void ComboBox_KeyDown_1(object sender, KeyEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            while (actionLists.SelectedItems.Count > 0)
            {
                actionlists.Remove((ActionList)actionLists.SelectedItems[0]);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var i in actionLists.SelectedItems)
            {
                MouseAction mouseAction;
                KeyBoardAction keyBoardAction;

                if (tryCreateMouseAction(out mouseAction))
                {
                    ((ActionList)i).Actions.Add(mouseAction);
                }

                if (tryCreateKeyboardAction(out keyBoardAction))
                {
                    ((ActionList)i).Actions.Add(keyBoardAction);
                }
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (Autostart.IsAutoStartEnabled)
            {
                Autostart.DisableSetAutoStart();
            }
            else
            {
                Autostart.EnableAutoStart();
            }
        }
    }
}