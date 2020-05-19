using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DALOs
{

    /// <summary>
    /// The data access layer representation of a Board.
    /// </summary>
    internal class DalBoard : DalObject<DalBoard>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        public const string BoardTaskCountName = "TaskCounter";

        private int _taskCounter;
        public int TaskCounter { get => _taskCounter; set{ _taskCounter = value; _controller.Update(Email, BoardTaskCountName, value); } }
        public List<DalColumn> Columns { get; set; }

        /// <summary>
        /// A public constructor that initializes all necessary fields to be persisted.
        /// </summary>
        /// <param name="email">The email of the user that is to be associated with the new DalBoard.</param>
        /// <param name="taskCounter">The task counter of the board to be persisted.</param>
        public DalBoard(string email, int taskCounter) : base(new BoardDalController())
        {
            Email = email;
            _taskCounter = taskCounter;
        }
    }
}
