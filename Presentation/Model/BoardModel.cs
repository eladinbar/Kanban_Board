using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Presentation.Model
{
    public class BoardModel : NotifiableModelObject
    {

        public string CreatorEmail { get; set; }
        public ObservableCollection<ColumnModel> Columns { get; set; }

        public BoardModel(BackendController controller, string creatorEmail) : base(controller)
        {
            this.CreatorEmail = creatorEmail;
            this.Columns = this.CreateColumns(creatorEmail);
            //this.Columns.CollectionChanged += HandleChange; //do we realy need HandleChange?????????????????
        }

        private ObservableCollection<ColumnModel> CreateColumns(string creatorEmail)  //receives SL.Columns and its list of Tasks and transform them into PL.Columns - 
            //has to be here or in ViewModel / BackendControlller??????????
        {
            ObservableCollection<ColumnModel> tempColumns = new ObservableCollection<ColumnModel>();
            int i = 0;
            foreach (string cName in this.Controller.GetBoard(creatorEmail).ColumnsNames.ToList())
            {
                var c = this.Controller.GetColumn(creatorEmail, cName);
                ObservableCollection<TaskModel> tasks = new ObservableCollection<TaskModel>();
                foreach (var t in c.Tasks)
                {
                    tasks.Add(new TaskModel(Controller,t.Id, t.Title, t.Description, t.CreationTime, t.DueDate, t.CreationTime, t.emailAssignee, i));
                }
                tempColumns.Add(new ColumnModel(tasks, c.Limit, c.Name, i));
                i++;
            }
            return tempColumns;
        }

        //private void HandleChange(object sender, NotifyCollectionChangedEventArgs e) //?????? dont know if needed??????????????
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
                
        //        foreach (ColumnModel tempColumn in e.OldItems) //e.OldItems - the list of Objects that have been changed??????????
        //        {
        //            //Controller.RemoveMessage(user.Email, y.Id);
        //        }

        //    }
        //}


    }
}
