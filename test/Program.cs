using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Text.Json;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = new User("a@gmail.com", "abA1", "sh");
            Console.WriteLine(JsonSerializer.Serialize(user));
            Console.WriteLine(user.ToJson());
            string str = JsonSerializer.Serialize(user);
            Console.WriteLine(str);
            Console.ReadKey();
        }
    }
}
