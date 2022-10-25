using CodeHollow.FeedReader;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NewsBot
{
    class User
    {
        public User(long id, bool active)
        {
            Id = id;
            Active = active;
        }
        public long Id { get; set; }
        public bool Active { get; set; }
    }

    internal class Program
    {
        public static List<User> users = new List<User>();
        public static FeedItem? currentFeedItem;
        
        async static void GetFeeds()
        {
            try
            {
                users = FileIO.GetUserIds();
                Console.ForegroundColor = ConsoleColor.Green;
                var client = new TelegramBotClient("5771517045:AAEgI_JShDBaNrlWGt8dGiIQWOojfS4bOXQ");

                DB.DelDB();
                DB.CreateDb();
                var feeds = FileIO.GetFeedsFromFile();
                foreach (var feedLink in feeds)
                {
                    try
                    {
                        var feed = FeedReader.Read(feedLink);
                        foreach (var item in feed.Items)
                        {
                            item.Link = Regex.Replace(item.Link, "\t", "");
                            item.Link = Regex.Replace(item.Link, "\n", "");
                            Console.WriteLine(item.Title + " - " + item.Link);
                            DB.Add(item.Link);
                        }
                    }
                    catch
                    {

                    }
                    
                }

                Console.WriteLine();
                Console.WriteLine("Новые сообщения:");
                while (true)
                {
                    foreach (var feedLink in feeds)
                    {
                        try
                        {
                            var feed = FeedReader.Read(feedLink);
                            foreach (var item in feed.Items)
                            {
                                item.Link = Regex.Replace(item.Link, "\t", "");
                                item.Link = Regex.Replace(item.Link, "\n", "");
                                if (!DB.Contains(item.Link))
                                {
                                    currentFeedItem = item;
                                    client.StartReceiving(Update, Error);
                                    DB.Add(item.Link);
                                    string text = currentFeedItem.Title + " " + currentFeedItem.Link;
                                    Console.WriteLine(text);
                                    foreach (var user in users)
                                    {
                                        if (user.Active)
                                        {
                                            client.SendTextMessageAsync(user.Id, text);
                                            //client.SendTextMessageAsync(user.Id, "старт - получать сообщения");
                                            //client.SendTextMessageAsync(user.Id, "стоп - прекратить получение сообщений");
                                        }
                                    }
                                    //var menuCommands = new MenuButtonCommands();
                                    //MenuButton buttonMenu = new MenuButton(userId, menuCommands);
                                }
                            }
                        }
                        catch
                        {

                        }
                        
                    }
                    
                }
            }
            catch
            {
                Console.WriteLine("Error");
            }
            finally
            {
            }
        }

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken arg3)
        {
            var msg = update.Message;
            long _id = update.Message.Chat.Id;
            bool _active = false;
            if (msg.Text.ToLower().Contains("старт"))
            {
                _active = true;
            }
            bool _isThere = false;
            foreach(var user in users)
            {
                if (user.Id == _id)
                {
                    user.Active = _active;
                    _isThere = true;
                }
            }
            if (!_isThere)
            {
                users.Add(new User(_id, _active));
            }
            FileIO.UpdateUsers(users);
            //string text = currentFeedItem.Title + " " + currentFeedItem.Link;
            //botClient.SendTextMessageAsync(update.Message.Chat.Id, text);
        }

        async static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            //throw new NotImplementedException();
        }

        async static void Display()
        {
            DB.Display();
        }
        static void Main(string[] args)
        {
            /*var client = new TelegramBotClient("5771517045:AAEgI_JShDBaNrlWGt8dGiIQWOojfS4bOXQ");
            client.StartReceiving(Update, Error);*/

            GetFeeds();
            
            //ConsoleStream();
        }

        //async static Task Update(ITelegramBotClient arg1, Update arg2, )
    }
}