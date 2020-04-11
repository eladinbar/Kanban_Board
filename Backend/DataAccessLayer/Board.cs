using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class Board : DalObject<Board>
    {
        private string _userEmail;
        private int _taskCounter;
        private List<Column> _columns;

        public Board (string email, int taskCounter, List<Column> columns)
        {
            _userEmail = email;
            _taskCounter = taskCounter;
            _columns = columns;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public Board FromJson()
        {
            throw new NotImplementedException();
        }
    }
}
