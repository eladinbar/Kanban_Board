using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.KanbanTests
{
    class UserForTestCreator
    {
        public List<ServiceLayer.User> _users { get; }

        public UserForTestCreator(int numOfUsers)
        {
            _users = new List<ServiceLayer.User>();
            Random rand = new Random();
            for (int i = 0; i < numOfUsers; i++)
            {
                int nickname = rand.Next(numOfUsers) + 1;
                ServiceLayer.User tempUser = new ServiceLayer.User(nickname + "@mashu.com", nickname + "");
                _users.Add(tempUser);

            }
        }

        public void PrintUsers()
        {
            Console.WriteLine("Current list of users: ");
            int i = 1;
            foreach (ServiceLayer.User tempUser in _users)
            {
                Console.WriteLine(i + ") " + tempUser.Email);
                i++;
            }
        }
    }
}