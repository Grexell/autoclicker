using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;

namespace Coursuch
{
    public class Executor
    {

        private ObservableCollection<ActionList> actions;

        public Dictionary<ActionList, Thread> threadDictionary { get; set; }
        public Dictionary<Thread, ActionList> actionDictionary { get; set; }
        public Dictionary<Thread, ManualResetEvent> resetsDictionary { get; set; }

        public Executor()
        {
            threadDictionary = new Dictionary<ActionList, Thread>();
            actionDictionary = new Dictionary<Thread, ActionList>();
            resetsDictionary = new Dictionary<Thread, ManualResetEvent>();
        }

        public ObservableCollection<ActionList> Actions { get => actions;
            set {
                actions = value;

                actions.CollectionChanged += addItem;
            }
        }

        public void addItem(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (NotifyCollectionChangedAction.Add == e.Action)
            {
                foreach (var it in e.NewItems) {

                    Thread itthread = new Thread(new ThreadStart(this.run));
                    ManualResetEvent manualReset = new ManualResetEvent(false);

                    threadDictionary.Add((ActionList)it, itthread);
                    actionDictionary.Add(itthread, (ActionList)it);
                    resetsDictionary.Add(itthread, manualReset);
                    ((ActionList)it).listeners.Add(new ActionList.executeListeners(this.pause));

                    itthread.Start();
                }
            }
            else if (NotifyCollectionChangedAction.Remove == e.Action)
            {
                foreach (var it in e.OldItems)
                {
                    stop((ActionList)it);
                }
            }
        }

        public void stop(ActionList list) {
            Thread bindingThread = threadDictionary[list];

            bindingThread.Abort();

            threadDictionary.Remove(list);
            actionDictionary.Remove(bindingThread);
            resetsDictionary.Remove(bindingThread);
        }

        public void pause(ActionList list) {
            try
            {
                Thread bindingThread = threadDictionary[list];

                if (list.IsExecute)
                {
                    resetsDictionary[bindingThread].Set();
                }
                else
                {
                    resetsDictionary[bindingThread].Reset();
                }
            }
            catch {
            }
        }

        public void run()
        {
            try
            {
                Thread cur = Thread.CurrentThread;
                ManualResetEvent manualReset = resetsDictionary[cur];
                ActionList list = actionDictionary[cur];

                bool working = true;

                while (working)
                {

                    manualReset.WaitOne();
                    foreach (var a in list.Actions) {
                        manualReset.WaitOne();
                        Thread.Sleep(a.StartTime > 0 ? a.StartTime : 0);
                        a.Execute();
                        Thread.Sleep(a.ExecutionTime > 0 ? a.ExecutionTime : 0);
                    }

                    if (list.IsRepeat)
                    {
                        list.Repeat--;

                        working = list.Repeat > 0;
                    }
                    else {
                        working = false;
                    }
                }
            }
            catch
            {
            }
        }
    }
}