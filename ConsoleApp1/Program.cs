using ConsoleApp1.View;

namespace ConsoleApp1
{
    enum Cmds
    {
        stop,
        show,
        update,
        delete,
        insert,
        update_or_insert
    };
    class Program
    {
        private static Manager manager = new();
        static void Main(string[] args)
        {
            string userCmd = string.Empty;
            manager.Connect();
            ShowAvailableCmds();
            do
            {
                Console.Write("Введите команду: ");
                userCmd = Console.ReadLine();
                switch (userCmd)
                {
                    case nameof(Cmds.show):
                        manager.ShowData();
                        break;

                    case nameof(Cmds.update):
                        manager.UpdateUserNameByLogin();
                        break;

                    case nameof(Cmds.delete):
                        manager.DeleteData();
                        break;

                    case nameof(Cmds.insert):
                        manager.AddUser();
                        break;

                    case nameof(Cmds.update_or_insert):
                        manager.UpdateUserProcByLogin();
                        break;

                    case nameof(Cmds.stop):
                        manager.Disconnect();
                        break;

                    default:
                        Console.WriteLine("Введена неверная команда!");
                        ShowAvailableCmds();
                        break;
                }
            } while (userCmd != Enum.GetName(typeof(Cmds), 0));

            Console.ReadLine();
        }

        static void ShowAvailableCmds()
        {
            Console.WriteLine("Список доступных команд:");
            foreach (var cmd in Enum.GetValues(typeof(Cmds)))
            {
                Console.WriteLine(cmd);
            }
        }
    }
}