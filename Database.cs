﻿using System;
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
        public static List<User> getSerial(string date)
        {
            List<User> notiftable = new List<User>();
      
            string query = "SELECT i.serialnumber FROM itemTable as i WHERE i.datedecommissioned = @date ";

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();

                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@date", date);
                
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User p = new User();
                        p.id = reader.GetInt32(0);
                        notiftable.Add(p);


                        // Process people...      
                    }
                    
                }

                conn.Close();
            }
            return notiftable;
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
                SQLiteCommand command = new SQLiteCommand( query, conn);
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


        //FUNCTION TO EDIT AN EXISTING ITEM
        public static bool editItem(item editItem)
        {
            bool itemFound = false;


            //search item
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                string query = "SELECT itemtype,addedby,supplier,datedelivered,statusid,datedecommissioned,conditionid FROM itemTable WHERE serialnumber = @sernum";

                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@sernum", editItem);

                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    itemFound = true;
                }

                //edit new details UPDATE command 
                command = new SQLiteCommand();

            }

            return itemFound;
        }


        //FUNCION FOR INSERTING A NEW ITEM TYPE 
        //IF RETURNS FALSE MEANING THE SUCCESSFUL INSERT
        public static bool addnewtype(string name, string model)
        {
            bool typeExist = false;

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                string query = "SELECT name,model FROM type where name = @name and model = @model";

                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@model", model);

                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    typeExist = true;
                }

                if(typeExist == false)
                {
                    query = "INSERT INTO type(name,model) VALUES(@name,@model)";
                    command = new SQLiteCommand(query, conn);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@model", model);

                    command.ExecuteNonQuery();
                }

                conn.Close();
            }

            return typeExist;
        }

        //FUNCTION FORREMOVING AITEM TYPE 
        //IF IT RETURNS TRUE THE REMOVAL IS SUCCESSFUL
        public static bool removetype(string name, string model)
        {
            bool typeExist = false;

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                string query = "SELECT name,model FROM type where name = @name and model = @model";

                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@model", model);

                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    //IF TYPE EXISTS
                    typeExist = true;
                }
                if (typeExist == true)
                {
                    query = "DELETE FROM type where name = @name and model = @model";
                    command = new SQLiteCommand(query, conn);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@model", model);

                    command.ExecuteNonQuery();
                }

                conn.Close();
            }

            return typeExist;
        }


    }
}
