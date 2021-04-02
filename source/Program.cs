//------------------------------------------------------------------------- \\
//      A simple script to read Twitch chat without any dependencies.       \\
//                                                                          \\
//         Written by DayMoniakk (https://github.com/DayMoniakk)            \\
//                                                                          \\
//         You are free to use the project without crediting me.            \\
//------------------------------------------------------------------------- \\

using System;
using System.Net.Sockets;
using System.IO;

namespace EasyTwitchChatReader
{
    // If you want to use it in your one project : simply copy paste the class below in your project
    class Program
    {
        private const string USERNAME = "your username"; // The bot account name (or your own twitch account)
        private const string PASSWORD = "your token"; // Generate the password for your account at https://twitchapps.com/tmi
        private const string CHANNEL = "your channel"; // Which chat the bot will connect to and read

        private static TcpClient twitchClient;
        private static StreamReader reader;
        private static StreamWriter writer;

        static void Main(string[] args)
        {
            // Change the console title (optional)
            Console.Title = "Easy Twitch Chat Reader";
            
            // Tell the bot to connect to the chat
            Connect(USERNAME, PASSWORD, CHANNEL);

            // Read the chat when connected
            ReadChat();
        }

        private static void Connect(string username, string password, string channel)
        {
            // Initialize the connection
            twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
            reader = new StreamReader(twitchClient.GetStream());
            writer = new StreamWriter(twitchClient.GetStream());

            // Fill the requirements to connect
            writer.WriteLine("PASS " + password);
            writer.WriteLine("NICK " + username);
            writer.WriteLine("USER " + username + " 8 * :" + username);
            writer.WriteLine("JOIN #" + channel);
            writer.Flush();

            // Display the status in the console
            if (twitchClient.Connected)
            {
                // Check if the parameters are null or empty
                if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password) || !string.IsNullOrEmpty(channel))
                {
                    if (username == "your username" || password == "your token" || channel == "your channel")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[WARNING] Please assign the bot account informations in order to connect !");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Connected to " + channel + " chat !");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[WARNING] Please assign the bot account informations in order to connect !");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        private static void ReadChat()
        {
            while (twitchClient.Connected)
            {
                // Check if a message is sended in the chat
                if (twitchClient.Available > 0)
                {
                    // Get the message from the chat
                    var message = reader.ReadLine();

                    // Check if the message is coming from the chat and not a DM (confusing right ? I still don't know why it's like this :p)
                    if (message.Contains("PRIVMSG"))
                    {
                        //Get the users name by splitting it from the string
                        var splitPoint = message.IndexOf("!", 1); // Check for the "!" in case it's a command
                        var chatName = message.Substring(0, splitPoint);
                        chatName = chatName.Substring(1);

                        //Get the users message by splitting it from the string
                        splitPoint = message.IndexOf(":", 1);
                        message = message.Substring(splitPoint + 1);

                        // Output the full message to the console
                        Console.WriteLine(String.Format("{0}: {1}", chatName, message));
                    }
                }
            }
        }
    }
}
