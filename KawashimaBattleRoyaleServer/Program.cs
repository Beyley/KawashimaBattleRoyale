using System;
using KawashimaBattleRoyaleServer;
Console.WriteLine("Starting Dr. Kawashima's Battle Royale Server!");
Server server = new("ws://0.0.0.0:1231/kawashima/", typeof(Client));
Console.ReadLine();
