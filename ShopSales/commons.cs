using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
// command commands used across different tables
namespace ShopSales.commons
{
    class SQLtools
    {

        public static string getConnectionStringSQLserver() {
            string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + System.Environment.CurrentDirectory + @"\db\companyData.mdf;Integrated Security=True";
            return ConnectionString;
        }
        public static string getConnectionString()
        {
            //string ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + System.Environment.CurrentDirectory + @"\db\companyData.accdb;Persist Security Info=True";
            string ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + System.Environment.CurrentDirectory + @"\db\companyData.accdb;Persist Security Info=True";
            return ConnectionString;
        }


        private static OleDbCommand parseSQLCommand(string mode, string textBoxInput, OleDbConnection connection)
        {
            OleDbCommand sqlCmd = connection.CreateCommand();
            sqlCmd.CommandTimeout = 3;
            if (mode == "search")
            {
                /*
                sqlCmd.CommandText = "SELECT * FROM goods";
                string[] parameters = textBoxInput.Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray();
                if (parameters.Length > 0)
                {
                    sqlCmd.CommandText = "SELECT * FROM goods WHERE Name LIKE @Names0";
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i].Trim(' ');
                    }
                    sqlCmd.Parameters.AddWithValue("@Names0", '%' + parameters[0] + '%');
                    for (int i = 1; i < parameters.Length; i++)
                    {
                        sqlCmd.CommandText = sqlCmd.CommandText + " OR Name LIKE @Names" + i.ToString();
                        sqlCmd.Parameters.AddWithValue("@Name" + i.ToString(), '%' + parameters[i] + '%');
                    }
                }
                */
                sqlCmd.CommandText = "SELECT * FROM goods WHERE Name LIKE @parem0";
                sqlCmd.Parameters.AddWithValue("@Parem0", '%'+textBoxInput+'%');
            }
            else if (mode == "expenditure_DESC")
            {
                sqlCmd.CommandText = "SELECT *, (goods.Sales + goods.Inventory) * goods.Unit_Cost AS 'expenditure' " +
                    "FROM goods " +
                    "WHERE goods.Name LIKE @Names " +
                    "ORDER BY 'expenditure' DESC";
                sqlCmd.Parameters.AddWithValue("@Names", "%" + textBoxInput + "%");
            }
            else if (mode == "P&L")
            {
                sqlCmd.CommandText = @"SELECT *, (goods.Price - goods.Unit_Cost) AS 'Unit P&L',
                    (goods.Inventory) * (goods.Price - goods.Unit_Cost) AS 'unrealized P&L',
                    (goods.Sales) * (goods.Price - goods.Unit_Cost) AS 'realized P&L', 
                    (goods.Inventory + goods.Sales) * (goods.Price - goods.Unit_Cost) AS 'P&L'  
                    FROM goods 
                    ORDER BY 'P&L' DESC";
                sqlCmd.Parameters.AddWithValue("@Names", "%" + textBoxInput + "%");
            }
            else if (mode == "ADD/UPDATE")
            {

                sqlCmd.CommandText = @"IF EXISTS (SELECT * FROM goods WHERE Name = @Parem0)
                    BEGIN
                    UPDATE goods
                    SET Name = @Parem0, Unit_Cost = @Parem1, Price = @Parem2, Sales = @Parem3, Inventory = @Parem4
                    WHERE goods.Name = @Parem0
                    END
                    ELSE
                    BEGIN
                    INSERT INTO goods (Name, Unit_Cost, Price, Sales, Inventory)
                    Values (@Parem0, @Parem1, @Parem2, @Parem3, @Parem4)
                    END
                    SELECT * FROM goods";
                string[] parems = textBoxInput.Split(',').ToArray();
                for (int i = 0; i < parems.Length; i++)
                {
                    parems[i] = parems[i].Trim(' '); // trim off spaces
                    sqlCmd.Parameters.AddWithValue("@Parem" + i.ToString(), parems[i]);
                }

            }
            else if (mode == "ADD")
            {
                sqlCmd.CommandText = @"INSERT INTO goods (Name, Unit_Cost, Price, Sales, Inventory)
                    Values (@Parem0, @Parem1, @Parem2, @Parem3, @Parem4)";
                string[] parems = textBoxInput.Split(',').ToArray();
                for (int i = 0; i < parems.Length; i++)
                {
                    parems[i] = parems[i].Trim(' '); // trim off spaces
                    sqlCmd.Parameters.AddWithValue("@Parem" + i.ToString(), parems[i]);
                }
            }
            else if (mode == "UPDATE")
            {
                sqlCmd.CommandText = @"UPDATE goods
                    SET Name = @Parem0, Unit_Cost = @Parem1, Price = @Parem2, Sales = @Parem3, Inventory = @Parem4
                    WHERE goods.Name = @Parem0";
                string[] parems = textBoxInput.Split(',').ToArray();
                for (int i = 0; i < parems.Length; i++)
                {
                    parems[i] = parems[i].Trim(' '); // trim off spaces
                    sqlCmd.Parameters.AddWithValue("@Parem" + i.ToString(), parems[i]);
                }
            }
            else if (mode == "DELETE")
            {
                sqlCmd.CommandText = @"DELETE FROM goods WHERE Name = @Parem0";
                sqlCmd.Parameters.AddWithValue("@Parem0", textBoxInput);
            }
            return sqlCmd;
            

            
        }

        private static OleDbConnection openConnection(string connectionString)
        {
            // connects to the db and return the sqlConnection object
            OleDbConnection connection = new OleDbConnection(connectionString);
            return connection;
        }

        public static DataTable executeQuery_DispalyOnTable(string mode, string queryString, string connectionString)
        {
            
            var table = new DataTable();
            using (OleDbConnection connection = openConnection(connectionString))
            {
                //connection.Open();
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(parseSQLCommand(mode, queryString, connection)))
                {
                    //System.Console.WriteLine(adapter.SelectCommand);
                     adapter.Fill(table);
                    connection.Close();
                    return table;
                }
            }
        }

        public static bool IsExistInDB(string textBoxInput, string connectionString)
        {
            using (OleDbConnection connection = openConnection(connectionString))
            {
                DataTable temp = new DataTable();
                OleDbCommand sqlCmd = connection.CreateCommand();
                sqlCmd.CommandTimeout = 3;
                sqlCmd.CommandText = "SELECT Name FROM goods WHERE Name = @Input";
                sqlCmd.Parameters.AddWithValue("@Input", textBoxInput);

                connection.Open();
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(sqlCmd))
                {
                    adapter.Fill(temp);
                }
                connection.Close();
                if (temp.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

    }
}
