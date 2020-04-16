using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Text.Json;
using System.IO;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            //DirectoryInfo dir = new DirectoryInfo("C: \\Users\\Elad\\Source\\Repos\\BGU - SE\\milestones - 2 - tpee\\Backend\\data\\Users");
            //string file = File.ReadAllText(@"C:\Users\Elad\Source\Repos\BGU-SE\milestones-2-tpee\Backend\data\Users\1@mashu.com.json");
            //User savedUser = JsonSerializer.Deserialize<User>(file);
            User user = new User("a@gmail.com", "abA1", "sh");
            User newUser = new User("b@gmail.com", "bab1A", "sw");
            string s = JsonSerializer.Serialize(user);
            newUser = JsonSerializer.Deserialize<User>(s);
            string str = "bla";
            string newStr;
            newStr = JsonSerializer.Deserialize<string>(JsonSerializer.Serialize(str));
            Apple apple = new Apple("Red", "Sweet");
            Apple newApple = new Apple("Green", "Sour");
            string s1 = JsonSerializer.Serialize(apple);
            newApple = JsonSerializer.Deserialize<Apple>(s1);
            //Console.WriteLine(JsonSerializer.Serialize(user));
            //Console.WriteLine(user.ToJson());
            //string str = JsonSerializer.Serialize(user);
            //Console.WriteLine(str);
            //Console.ReadKey();
        }
    }
}
