using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class DalObject<T> where T:DalObject<T>
    {
        public abstract string ToJson();

        public abstract T FromJson(string json);

        public abstract void Save();
    }
}
