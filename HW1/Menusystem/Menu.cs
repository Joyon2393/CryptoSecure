using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuSystem
{
    public class Menu
    {
        public enum MenuLevel
        {
            Level0,
            Level1,
            Level2Plus
        }

        private Dictionary<string, MenuItem> MenuItems { get; set; } =new Dictionary<string, MenuItem>();
        private readonly MenuLevel _menuLevel;
        private readonly string[] reservedActions = new[] {"x", "m", "r"};

        public Menu(MenuLevel level)
        {
            _menuLevel = level;

        }

        public void AddMenuItem(MenuItem item)
        {
            if (item.UserChoice == "")
            {
                throw new Exception($"Userchoice can not be empty");
            }
            MenuItems.Add(item.UserChoice,item);
        }

        public string RunMenu()
        {
            var userChoice="";

            do
            {
                Console.Write("");
                foreach (var menuItem in MenuItems)
                {
                    Console.WriteLine(menuItem.Value);
                }

                switch (_menuLevel)
                {
                    case MenuLevel.Level0:
                        Console.WriteLine("x)exit");
                        break;
                    case MenuLevel.Level1:
                        Console.WriteLine("m)return to main");
                        Console.WriteLine("x)Exit");
                        break;
                    case MenuLevel.Level2Plus:
                        Console.WriteLine("r)return to previous");
                        Console.WriteLine("m)return to main");
                        Console.WriteLine("x)Exit");
                        break;
                    default:
                        throw new Exception("unknown menu depth");
                }
                Console.Write(">");
                userChoice = Console.ReadLine()?.ToLower().Trim() ?? "";
                if (!reservedActions.Contains(userChoice))
                {
                    if (MenuItems.TryGetValue(userChoice, out var userMenuItem))
                    {
                        userChoice = userMenuItem.MethodToExecute();
                    }
                    else
                    {
                        Console.WriteLine("I dont have this option");
                    }
                }

                if (userChoice == "x")
                {
                    if (_menuLevel == MenuLevel.Level0)
                    {
                        Console.WriteLine("Closing Down....");
                        break;
                    }

                    if (_menuLevel != MenuLevel.Level0 && userChoice == "m")
                    {
                        break;
                    }

                    if (_menuLevel == MenuLevel.Level2Plus && userChoice == "r")
                    {
                        break;
                    }
                }
                
            } while (true);

            return userChoice;
        }

    }
}