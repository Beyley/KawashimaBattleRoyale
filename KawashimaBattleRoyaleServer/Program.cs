using System;
using KawashimaBattleRoyaleServer;

Console.WriteLine("Starting Dr. Kawashima's Battle Royale Server!");
Server server = new("http://127.0.0.1:1231/kawashima/", typeof(Client));
Console.ReadLine();
