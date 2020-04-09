﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    public class Column : PersistedObject<DataAccessLayer.Column>
    {
        private string _name;
        private int _limit;
        private List<Task> Tasks;

        public Column(string name)
        {

        }

        public void LimitColumnTasks(int limit)
        {
            throw new NotImplementedException();
        }

        public void InsertTask(Task t)
        {
            throw new NotImplementedException();
        }
        public Task RemoveTask(int id)
        {
            throw new NotImplementedException();
        }

        public Task GetTask(int taskId)
        {
            if (GetTasks.Exists(x => x.Id == taskId))
                return GetTasks.Find(x => x.Id == taskId);
            else
                return null;
        }

        public List<Task> GetTasks { get; }

        public string Name { get;}



    }
}
