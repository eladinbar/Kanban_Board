﻿using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DALOs
{
    internal class DalBoard : DalObject
    {
        public const string BoardTaskCountName = "TaskCounter";

        private int _taskCounter;
        public int TaskCounter { get => _taskCounter; set{ _taskCounter = value;} }

        public DalBoard(string email, int taskCounter) : base(new BoardDalController())
        {
            Email = email;
            _taskCounter = taskCounter;
        }
    }
}
