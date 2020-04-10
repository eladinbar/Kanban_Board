﻿using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    interface PersistedObject<T> where T : DalObject<T>
    {
        T ToDalObject();

        void Save();
    }
}
