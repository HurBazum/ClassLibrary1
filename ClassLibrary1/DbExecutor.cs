using Microsoft.Data.SqlClient;
using System.Data;

namespace ClassLibrary1
{
    public class DbExecutor
    {
        private MainConnector connector;
        public DbExecutor(MainConnector connector)
        {
            this.connector = connector;
        }

        public DataTable SelectAll(string table)
        {
            var selectcammandtext = "select * from " + table;

            var adapter = new SqlDataAdapter
                (
                selectcammandtext,
                connector.GetConnection()
                );

            DataSet ds = new();

            adapter.Fill(ds);

            return ds.Tables[0];
        }

        public SqlDataReader SelectAllCommandReader(string table)
        {
            SqlCommand cmd = new()
            {
                CommandType = CommandType.Text,
                CommandText = "select * from " + table,
                Connection = connector.GetConnection()
            };

            SqlDataReader dr = cmd.ExecuteReader();

            if(dr.HasRows)
            {
                return dr;
            }

            return null;
        }

        public int DeleteByColumn(string table, string columnName, string columnValue)
        {
            SqlCommand cmd = new()
            {
                CommandType = CommandType.Text,
                CommandText = $"delete from {table} where {columnName} = '{columnValue}'",
                Connection = connector.GetConnection()
            };

            return cmd.ExecuteNonQuery();
        }

        public int AddRow(string table, string name, string login)
        {
            SqlCommand cmd = new()
            {
                CommandType = CommandType.Text,
                CommandText = $"insert into {table}(Name,Login) values('{name}','{login}')",
                Connection = connector.GetConnection()
            };

            return cmd.ExecuteNonQuery();         
        }

        public int ExecProcedureAdding(string name, string login)
        {
            SqlCommand cmd = new()
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "AddingUserProc",
                Connection = connector.GetConnection()
            };

            cmd.Parameters.Add(new SqlParameter("@Name", name));
            cmd.Parameters.Add(new SqlParameter("@Login", login));

            return cmd.ExecuteNonQuery();
        }

        public int UpdateByColumn(string table, string columnName, string newValue, string columnCheck, string checkValue)
        {
            SqlCommand cmd = new()
            {
                CommandType = CommandType.Text,
                CommandText = $"update {table} set {columnName} = '{newValue}' where {columnCheck} = '{checkValue}'",
                Connection = connector.GetConnection()
            };

            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// если логин существует - обновляет данные в строке
        /// иначе добавляет новую строку в БД
        /// </summary>
        /// <param name="newValue">новое имя</param>
        /// <param name="checkValue">логин</param>
        /// <returns></returns>
        public int ExecProcedureUpdating(string newValue, string checkValue)
        {
            SqlCommand cmd = new()
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "UpdatingUserByLoginProc",
                Connection = connector.GetConnection()
            };

            cmd.Parameters.Add(new SqlParameter("@Name", newValue));
            cmd.Parameters.Add(new SqlParameter("@Login", checkValue));

            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// считает строки, нужно для вывода верного сообщения в Manager.UpdateUserProcByLogin()
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public int CountRows(string table)
        {
            SqlCommand cmd = new()
            {
                CommandType = CommandType.Text,
                CommandText = $"select count(*) from {table}",
                Connection = connector.GetConnection()
            };

            return (int)cmd.ExecuteScalar();
        }
    }
}