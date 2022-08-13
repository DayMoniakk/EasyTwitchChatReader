# EasyTwitchChatReader
A simple script to read Twitch chat without any dependencies.

_In order to keep the connection active you have to setup a timer by yourself in order to send a ping each 5 minutes using these two lines_
```cs
writer.WriteLine("PING :irc.chat.twitch.tv");
writer.Flush();
```
