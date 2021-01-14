using Poker_Engine.Service;
using PokerEngine.Model;
using System;

namespace PokerGame
{
    class Program
    {
        static void Main()
        {
            var addPlayer = true;

            var game = new GameService();

            do
            {
                var player = new Player();

                Console.Write("Enter player name: ");
                player.Name = Console.ReadLine();

                Console.Write("Enter player cards: [separated by comma] ");
                player.Cards = Console.ReadLine().Replace(" ", "").Split(',', StringSplitOptions.RemoveEmptyEntries);


                if (!game.AddPlayer(player, out var message))
                    Console.WriteLine(message);

                Console.Write("Add more player [Y/N]? ");
                var add = Console.ReadLine();

                if (add.ToLower() != "y")
                    addPlayer = false;
            }
            while (addPlayer);

            var winners = game.Winners;

            foreach (var p in game.Players)
                Console.WriteLine($"Name: {p.Name}, Hand: {p.Hand}");

            Console.WriteLine("Congratulations!");

            foreach(var w in winners)
                Console.WriteLine($"Name: {w.Name}, Hand: {w.Hand}");

        }


    }
}
