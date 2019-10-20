using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace COE131L
{
    class Database
    {
        //public SQLiteConnection myConnection;

        public Database()
        {



        }
        //FUNCTION TO DISPLAY THE TABLE
        public static DataTable getRecord()
        {
            DataTable itemTable = new DataTable();
            string query = "SELECT serialnumber as 'Serial Number',type.name as 'Item Name',account.firstname as 'Added By'" +
                    ",supplier as 'Supplier Name',datedelivered as 'Date Delivered',status.description as 'Status'," +
                    "datedecommissioned as 'Date of Decommission',condition.description as 'Condition' " +
                    "FROM itemTable INNER JOIN type ON type.typeid = itemTable.itemtype INNER JOIN account ON account.id = itemTable.addedby INNER JOIN " +
                    "status ON status.statusid = itemTable.statusid INNER JOIN condition ON condition.id = itemTable.conditionId";

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();

                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);


                adapter.Fill(itemTable);
                conn.Close();
            }
            return itemTable;
        }
        //FUNCTION USED FOR SEARCHING
        public static DataTable searchRecord(string word)
        {
            DataTable itemTable = new DataTable();
            string quote = word + "%";
            string query = "SELECT serialnumber as 'Serial Number',type.name as 'Item Name',account.firstname as 'Added By'" +
                    ",supplier as 'Supplier Name',datedelivered as 'Date Delivered',status.description as 'Status'," +
                    "datedecommissioned as 'Date of Decommission',condition.description as 'Condition' " +
                    "FROM itemTable INNER JOIN type ON type.typeid = itemTable.itemtype INNER JOIN account ON account.id = itemTable.addedby INNER JOIN " +
                    "status ON status.statusid = itemTable.statusid INNER JOIN condition ON condition.id = itemTable.conditionId "
                    + "WHERE serialnumber like @word or type.name like @word or account.firstname like  @word or supplier like  @word or status.description like  @word  or condition.description like  @word ";

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();

                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@word", quote);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);


                adapter.Fill(itemTable);
                conn.Close();
            }
            return itemTable;
        }

        //FUNCTION USED FOR LOGGING IN RETURNS ACCOUNT DETAILS 
        public static bool accessUser(string username, string password, ref User loggedUser)
        {
            bool userExist = false;
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                string query = "SELECT id,firstname,lastname,username,password from account WHERE username = @uname and password = @pword";
                //PLACE USERTYPELATER ON IN THE SCRIPT
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@uname", username);
                command.Parameters.AddWithValue("@pword", password);

                //command.Parameters.AddWithValue("@uType", newUser.userType);

                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    // inrelevant code
                    while (reader.Read())
                    {
                        loggedUser.id = reader.GetInt32(0);
                        loggedUser.userName = reader.GetString(3);
                        loggedUser.password = reader.GetString(4);
                        loggedUser.firstName = reader.GetString(1);
                        loggedUser.lastName = reader.GetString(2);
                    }

                    userExist = true;
                }

                conn.Close();

                return userExist;
            }
        }


        //FUNCTION TO ADD NEW ACCOUNT TO THE RECORD
        public static void insertAccount(User newUser)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                //PLACE USERTYPELATER ON IN THE SCRIPT
                string query = "INSERT INTO account(firstName,lastName,username,password) VALUES(@fname,@lname,@usname,@pass)";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@fname", newUser.firstName);
                command.Parameters.AddWithValue("@lname", newUser.lastName);
                command.Parameters.AddWithValue("@usname", newUser.userName);
                command.Parameters.AddWithValue("@pass", newUser.password);
                //command.Parameters.AddWithValue("@uType", newUser.userType);

                command.ExecuteNonQuery();

                conn.Close();
            }
        }

        //FUNCTION TO ADD NEW ITEM TO THE RECORD
        public static void addItem(item newItem)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                string query = "INSERT INTO itemTable (itemtype,addedby,supplier,datedelivered,statusid,datedecommissioned,conditionid)" +
                                                            "VALUES(@itemtype, @userid, @supp, @datedel, @statid, @datedecom,@conid)";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@itemtype", newItem.itemType);
                command.Parameters.AddWithValue("@userid", newItem.addedby);
                command.Parameters.AddWithValue("@supp", newItem.supplier);
                command.Parameters.AddWithValue("@datedel", newItem.datedelivered);
                command.Parameters.AddWithValue("@statid", newItem.statusId);
                command.Parameters.AddWithValue("@datedecom", newItem.datedecomm);
                command.Parameters.AddWithValue("@conid", newItem.conditionId);

                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        //FUNCTION TO REMOVE AN ITEM TO THE RECORD
        public static bool removeItem(int serNum)
        {
            bool itemFound = false;


            //search item
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();

                SQLiteCommand command = new SQLiteCommand("SELECT itemtype,addedby,supplier,datedelivered,statusid,datedecommissioned,conditionid FROM itemTable WHERE serialnumber = @sernum", conn);
                command.Parameters.AddWithValue("@sernum", serNum);

                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    itemFound = true;
                }
                //remove item if found

                if (itemFound == true)
                {
                    command = new SQLiteCommand("DELETE FROM itemTable where serialnumber = @sernum", conn);
                    command.Parameters.AddWithValue("@sernum", serNum);

                    command.ExecuteNonQuery();


                }

                conn.Close();
            }

            return itemFound;
        }


        //FUNCTION TO FIND AN EXISTING ITEM
        public static bool searchItem(int serialNum, ref item foundItem)
        {
            bool itemFound = false;


            //search item
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();

                SQLiteCommand command = new SQLiteCommand("SELECT itemtype,addedby,supplier,datedelivered,statusid,datedecommissioned,conditionid FROM itemTable WHERE serialnumber = @sernum", conn);
                command.Parameters.AddWithValue("@sernum", serialNum);

                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    itemFound = true;

                    foundItem.serialNumber = serialNum;
                    foundItem.itemType = reader.GetInt32(0);
                    foundItem.addedby = reader.GetInt32(1);
                    foundItem.supplier = reader.GetString(2);
                    foundItem.datedelivered = DateTime.Parse(reader.GetString(3));
                    foundItem.statusId = reader.GetInt32(4);
                    foundItem.datedecomm = DateTime.Parse(reader.GetString(5));
                    foundItem.conditionId = reader.GetInt32(6);

                }


            }

            return itemFound;
        }
        //ASSUMED THAT THE ITEM IS EXISTING TO BE EDITED IN THIS FUNCTION 
        public static void editItem(item editItem)
        {

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                string query = "UPDATE itemTable SET itemtype = @itemType,addedby = @added,supplier = @supp,datedelivered = @datedel, statusid = @stat ,datedecommissioned = @datedecom, conditionid = @cond WHERE serialnumber = @sernum";

                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@sernum", editItem.serialNumber);
                command.Parameters.AddWithValue("@itemType", editItem.itemType);
                command.Parameters.AddWithValue("@added", editItem.addedby);
                command.Parameters.AddWithValue("@supp", editItem.supplier);
                command.Parameters.AddWithValue("@datedel", editItem.datedelivered);
                command.Parameters.AddWithValue("@stat", editItem.statusId);
                command.Parameters.AddWithValue("@datedecom", editItem.datedecomm);
                command.Parameters.AddWithValue("@cond", editItem.conditionId);

                command.ExecuteNonQuery();
            }
        }

        //FUNCTION FOR ADDING A NEW ITEM TYPE
        public static bool addItemType(string typeName,string modelName)
        {
            bool nameExists = true;
            string tName = typeName + "%";
            string mName = modelName + "%";
               
            string query = "SELECT name, model,typeid FROM type where name = @tname";

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();

                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@tname", tName);
                

                SQLiteDataReader reader = command.ExecuteReader();
                if(!reader.HasRows)
                {
                    nameExists = false;
                }
                conn.Close();
            
            }

            if (nameExists == false)
            {
                query = "INSERT INTO type (name,model) VALUES (@Name,@Model)";

                using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(query, conn);
                    command.Parameters.AddWithValue("@Name", typeName);
                    command.Parameters.AddWithValue("@Model", modelName);
                    command.ExecuteNonQuery();

                    conn.Close();
                }
            }
            return nameExists;
        }


    }
}

/*
  <connectionStrings>
    <add name="usercon" connectionString="Data Source =.\accounts.db;Version=3;" providerName="System.Data.SqlClient" />
  </connectionStrings>
  <configSections>
    <!-- For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 -->
    <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
  </configSections>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
  </startup>
  <entityFramework>
    <defaultConnectionFactory type="System.Data.Entity.Infrastructure.LocalDbConnectionFactory, EntityFramework">
      <parameters>
        <parameter value="mssqllocaldb" />
      </parameters>
    </defaultConnectionFactory>
    <providers>
      <provider invariantName="System.Data.SqlClient" type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
      <provider invariantName="System.Data.SQLite.EF6" type="System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6" />
    </providers>
  </entityFramework>
  <system.data>
    <DbProviderFactories>
      <remove invariant="System.Data.SQLite.EF6" />
      <add name="SQLite Data Provider (Entity Framework 6)" invariant="System.Data.SQLite.EF6" description=".NET Framework Data Provider for SQLite (Entity Framework 6)" type="System.Data.SQLite.EF6.SQLiteProviderFactory, System.Data.SQLite.EF6" />
    <remove invariant="System.Data.SQLite" /><add name="SQLite Data Provider" invariant="System.Data.SQLite" description=".NET Framework Data Provider for SQLite" type="System.Data.SQLite.SQLiteFactory, System.Data.SQLite" /></DbProviderFactories>
  </system.data>
     
     
     */
