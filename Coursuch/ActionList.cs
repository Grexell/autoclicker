using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Coursuch
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ActionList
    {
        public const int DEFAULT_CAPACITY = 10;

        public delegate void executeListeners(ActionList list);

        [JsonIgnore]
        private bool isExecute;

        [JsonIgnore]
        public List<executeListeners> listeners;

        [JsonProperty]
        public bool IsRepeat { get; set; }
        [JsonProperty]
        public int Repeat { get; set; }
        [JsonProperty]
        public bool IsExecute { get=> isExecute;
            set {
                isExecute = value;

                foreach (var listener in listeners) {
                    listener.Invoke(this);
                }
            }
        }

        [JsonProperty]
        public ObservableCollection<AbstractAction> Actions { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        public ActionList() : this(new ObservableCollection<AbstractAction>())
        {}
        
        [OnDeserialized]
        internal void OnSerializedMethod(StreamingContext context)
        {
            foreach (var a in Actions) {
                if (a != null) {
                    a.parentList = this;
                }
            }
            
            Actions.CollectionChanged += addItem;

            listeners = new List<executeListeners>();
        }

        public ActionList(ObservableCollection<AbstractAction> actions)
        {
            listeners = new List<executeListeners>();
            Actions = actions;
            IsRepeat = false;
            Repeat = 0;
            IsExecute = false;
            Name = "";

            Actions.CollectionChanged += addItem;
        }

        public void addItem(object sender, NotifyCollectionChangedEventArgs e) {
            if (NotifyCollectionChangedAction.Add == e.Action) {
                foreach (var a in e.NewItems) {
                    ((AbstractAction)a).parentList = this;
                }
            } else if (NotifyCollectionChangedAction.Remove == e.Action)
            {
                foreach (var a in e.OldItems)
                {
                    ((AbstractAction)a).parentList = null;
                }
            }
        }
    }
}