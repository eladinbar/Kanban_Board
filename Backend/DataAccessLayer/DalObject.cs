﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class DalObject<T> where T:DalObject<T>
    {
        public string ToJson() {
            return JsonSerializer.Serialize(this); //Returns the T instance in Json (string) form
        }

        public T FromJson(string json) {
            return JsonSerializer.Deserialize<T>(json);
        }

        public abstract void Save(string path);
    }
}
