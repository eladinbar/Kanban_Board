using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class Column : DalObject<Column>
    {
        private string _name;
        private int _limit;
        private List<Task> _tasks;

        public Column (string name,int limit,List<Task> tasks)
        {
            _name = name;
            _limit = limit;
            _tasks = tasks;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public Column FromJson()
        {
            throw new NotImplementedException();
        }
    }
}
