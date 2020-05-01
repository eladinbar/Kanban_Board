using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    internal class BoardDalController : DalController
    {
        private const string BoardTableName = "Boards";

        public BoardDalController() : base(BoardTableName) { }

        public List<DalBoard> SelectAllBoards()
        {
            List<DalBoard> boardList = Select().Cast<DalBoard>().ToList();
            return boardList;
        }

        internal override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
