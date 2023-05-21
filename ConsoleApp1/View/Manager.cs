using ClassLibrary1;
using ConsoleApp1.Helper;

namespace ConsoleApp1.View
{
    public class Manager
    {
        MainConnector connector = new();
        DbExecutor executor;
        Table userTable = new();
        public Manager()
        {
            userTable.Name = "NetworkUser";
            userTable.ImportantField = "Login";
            userTable.Fields.Add("Id");
            userTable.Fields.Add("Name");
            userTable.Fields.Add("Login");
        }
        public void Connect()
        {
            var result = connector.ConnectAsync();

            if (result.Result)
            {
                Console.WriteLine("Подключение успешно!");

                executor = new(connector);
            }
            else
            {
                Console.WriteLine("Ошибка подключения!");
            }
        }

        public void Disconnect()
        {
            Console.WriteLine("Отключаем БД!");

            connector.DisconnectAsync();
        }

        public void ShowData()
        {
            Console.WriteLine("Получаем данные из таблицы " + userTable.Name);

            var reader = executor.SelectAllCommandReader(userTable.Name);

            var columnList = new List<string>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                columnList.Add(reader.GetName(i));
            }

            foreach (string column in columnList)
            {
                Console.Write(column + "\t");
            }

            Console.WriteLine();

            while (reader.Read())
            {
                for (int i = 0; i < columnList.Count; i++)
                {
                    var value = reader[columnList[i]];
                    Console.Write($"{value}\t");
                }
                Console.WriteLine();
            }

            reader.Close();
        }

        public void DeleteData()
        {
            Console.Write("Введите логин для удаления: ");
            Console.WriteLine($"Была удалена {executor.DeleteByColumn(userTable.Name, userTable.ImportantField, Console.ReadLine())} строка!");
        }

        public void AddData()
        {
            Console.WriteLine("Добавляем строку в БД!");
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите логин: ");
            string login = Console.ReadLine();

            Console.WriteLine($"Была добавлена {executor.AddRow(userTable.Name, name, login)} строка!");
        }

        public void AddUser()
        {
            Console.WriteLine("Добавим пользователя!");
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите логин: ");
            string login = Console.ReadLine();

            Console.WriteLine($"Была добавлена {executor.ExecProcedureAdding(name, login)} строка!");
        }

        public void UpdateUserNameByLogin()
        {
            Console.WriteLine("Обновим имя пользователя по логину!");
            Console.Write("Введите логин: ");
            string login = Console.ReadLine();
            Console.Write("Введите новое имя: ");
            string name = Console.ReadLine();

            if (UpdateUserByLogin(login, name) == 1)
            {
                Console.WriteLine("Имя успешно обновлено!");
            }
            else
            {
                Console.WriteLine("Возникла ошибка!");
            }
        }

        public void UpdateUserProcByLogin()
        {
            int rows = executor.CountRows(userTable.Name);
            Console.WriteLine($"rows = {rows}");
            int tableRows = rows;
            Console.WriteLine("Обновим имя пользователя по логину!");
            Console.Write("Введите логин: ");
            string login = Console.ReadLine();
            Console.Write("Введите новое имя: ");
            string name = Console.ReadLine();
            executor.ExecProcedureUpdating(name, login);
            if (tableRows == executor.CountRows(userTable.Name))
            {
                Console.WriteLine(rows);
                Console.WriteLine("Была изменена строка!");
            }
            else
            {
                Console.WriteLine("Была добавлена новая строка!");
            }
        }

        private int UpdateUserByLogin(string value, string newvalue)
        {
            return executor.UpdateByColumn(userTable.Name, userTable.Fields[1], newvalue, userTable.ImportantField, value);
        }
    }
}