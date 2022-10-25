using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsBot
{
    internal class FileIO
    {
        public static List<User> GetUserIds()
        {
            var result = new List<User>();
            try
            {
                var file = new StreamReader("userId.txt");
                while (!file.EndOfStream)
                {
                    try
                    {
                        var line = file.ReadLine().Split('#');
                        var _id = long.Parse(line[0]);
                        var _active = bool.Parse(line[1]);
                        result.Add(new User(_id, _active));
                    }
                    catch
                    {

                    }
                    
                }
                file.Close();
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static void UpdateUsers(List<User> users)
        {
            try
            {
                var file = new StreamWriter("userId.txt");
                foreach (User user in users)
                {
                    file.WriteLine(user.Id + "#" + user.Active);
                }
                file.Close();
            }
            catch
            {

            }
        }

        public static void ChangeUserId(long id)
        {
            var file = new StreamWriter("userId.txt");
            file.Write(id);
            file.Close();
        }
        public static List<string> GetFeedsFromFile()
        {
            var result = new List<string>();
            try
            {
                var file = new StreamReader("feeds.txt");
                while (!file.EndOfStream)
                {
                    result.Add(file.ReadLine().Trim());   
                }
                return result;
            }
            catch
            {
                return result;
            }

        }
    }
}
