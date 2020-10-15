using System;
using MenuSystem;

namespace Homework
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=======>TIC-Tac_toe Ahmed======>");
            var menuB=new Menu(Menu.MenuLevel.Level2Plus);
            menuB.AddMenuItem(new MenuItem("Sub menu 2","1",DefaultMenuAction));
            var menuA = new Menu(Menu.MenuLevel.Level1);
            menuA.AddMenuItem(new MenuItem("Go to submenu 2", "1", menuB.RunMenu));
            menuA.AddMenuItem(new MenuItem("testing", "2", DefaultMenuAction));

            var menu = new Menu(Menu.MenuLevel.Level0);
            menu.AddMenuItem(new MenuItem("Go to submenu 1", "s", menuA.RunMenu));
            menu.AddMenuItem(new MenuItem("New game human vs human. Pointless.", "1", DefaultMenuAction));
            menu.AddMenuItem(new MenuItem("New game puny human vs mighty AI", "2", DefaultMenuAction));
            menu.AddMenuItem(new MenuItem("New game mighty AI vs superior AI", "3", DefaultMenuAction));
            menu.RunMenu();

            
        }
        static string DefaultMenuAction()
        {
            Console.WriteLine("Not implemented yet!");

            return "";
        }

    }
}