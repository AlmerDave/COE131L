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

using OfficeOpenXml;
using System.Windows;

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
            string query = "SELECT serialnumber as 'Serial Number', classification.description as 'Classification', type.name as 'Item Name', type.model as 'Model'," +
                    "account.username as 'Laboratory Assistant'" +
                    ",supplier as 'Supplier Name',datedelivered as 'Date Delivered',status.description as 'Status'," +
                    "datedecommissioned as 'Date of Decommission',condition.description as 'Condition' " +
                    "FROM itemTable INNER JOIN type ON type.typeid = itemTable.itemtype INNER JOIN account ON account.id = itemTable.addedby INNER JOIN " +
                    "status ON status.statusid = itemTable.statusid INNER JOIN condition ON condition.id = itemTable.conditionId INNER JOIN classification ON classification.id = itemTable.classtype"; 

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
     
        public static DataTable getBreakageRecord()
        {
            DataTable itemTable = new DataTable();
            string query = "SELECT breakageInformation.serialno as 'Serial Number', classification.description as 'Classification', type.name as 'Item Name', type.model 'Model', "
                + "breakageInformation.studentid as 'Broken By', condition.description as 'Condition', account.username as 'Laboratory Assistant', "
                + "breakageInformation.daterecorded as 'Date Recorded' FROM breakageInformation  "
                + "INNER JOIN itemTable on itemTable.serialnumber = breakageInformation.serialno "
                + "INNER JOIN type on itemTable.itemtype = type.typeid "
                + "INNER JOIN condition on itemTable.conditionId = condition.id "
                + "INNER JOIN account on account.id = breakageInformation.recordedby "
                + "INNER JOIN classification ON classification.id = breakageInformation.classtype";

            using (SQLiteConnection conn = new SQLiteConnection("Data Source= MUlab.db"))
            {
                conn.Open();

                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);


                adapter.Fill(itemTable);
                conn.Close();
            }
            return itemTable;
        }
 
        public static DataTable searchRecord(string word)
        {
            DataTable itemTable = new DataTable();
            string quote = word + "%";
            string query = "SELECT serialnumber as 'Serial Number',type.name as 'Item Name', type.model as 'Model' ,account.username as 'Laboratory Assistant'" +
                    ",supplier as 'Supplier Name',datedelivered as 'Date Delivered',status.description as 'Status'," +
                    "datedecommissioned as 'Date of Decommission',condition.description as 'Condition' " +
                    "FROM itemTable INNER JOIN type ON type.typeid = itemTable.itemtype INNER JOIN account ON account.id = itemTable.addedby INNER JOIN " +
                    "status ON status.statusid = itemTable.statusid INNER JOIN condition ON condition.id = itemTable.conditionId "
                    + "WHERE serialnumber like @word or type.name like @word or type.model like @word or account.username like  @word or supplier like  @word or status.description like  @word  or condition.description like  @word ";

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
        
        public static DataTable searchBreakageRecord(string word)
        {
            DataTable itemTable = new DataTable();
            string quote = word + "%";
            string query = "SELECT breakageInformation.serialno as 'Serial Number', type.name as 'Item Name', type.model 'Model', "
                + "breakageInformation.studentid as 'Broken By', condition.description as 'Condition', account.username as 'Laboratory Assistant', "
                + "breakageInformation.daterecorded as 'Date Recorded' FROM breakageInformation  "
                + "INNER JOIN itemTable on itemTable.serialnumber = breakageInformation.serialno "
                + "INNER JOIN type on itemTable.itemtype = type.typeid "
                + "INNER JOIN condition on itemTable.conditionId = condition.id "
                + "INNER JOIN account on account.id = breakageInformation.recordedby "
                + "WHERE breakageInformation.serialno like @word or type.name like @word or type.model like @word or breakageInformation.studentid like @word or account.username like  @word or condition.description like @word AND itemTable.conditionId = 3";

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

        public static List<item> storeRecord()
        {
            List<item> storetable = new List<item>();

            string query = "SELECT * FROM type";

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();

                SQLiteCommand command = new SQLiteCommand(query, conn);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        item p = new item();
                        p.itemType = reader.GetInt32(1);
                        p.model = reader.GetString(2);
                        
                        storetable.Add(p);


                        // Process people...      
                    }

                }

                conn.Close();
            }
            return storetable;
        }

        public static List<item> storeClassification()
        {
            List<item> storetable = new List<item>();

            string query = "SELECT * FROM classification";

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();

                SQLiteCommand command = new SQLiteCommand(query, conn);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        item p = new item();
                        p.classType = reader.GetInt32(1);
                        p.classification = reader.GetString(2);

                        storetable.Add(p);


                        // Process people...      
                    }

                }

                conn.Close();
            }
            return storetable;
        }

        //FUNCTION FOR GETTINGserial in notification
        public static List<item> getSerial(string date)
        {
            List<item> notiftable = new List<item>();
      
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
                        item p = new item();
                        p.serialNumber = reader.GetString(0);
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
                    conn.Close();
                }

                

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
                string query = "INSERT INTO account(firstName,lastName,username,password, nickname) VALUES(@fname,@lname,@usname,@pass, @nickname)";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@fname", newUser.firstName);
                command.Parameters.AddWithValue("@lname", newUser.lastName);
                command.Parameters.AddWithValue("@usname", newUser.userName);
                command.Parameters.AddWithValue("@pass", newUser.password);
                command.Parameters.AddWithValue("@nickname", newUser.nickname);


                command.ExecuteNonQuery();

                conn.Close();
            }
        }

        //FUNCTION TO ADD NEW ITEM TO THE RECORD 
        //IF IT RETURNS FALSE THEN THE ITEM IS ADDED TO THE RECORD 
        public static bool addItem(item newItem)
        {
            bool itemExist = false;
            //SEARCH FOR ITEM
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                string query = "SELECT itemtype from itemTable  WHERE serialnumber = @serial";
                //PLACE USERTYPELATER ON IN THE SCRIPT
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@serial", newItem.serialNumber);

                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    itemExist = true;
                }
                conn.Close();
            }

            // ITEMEXIST = FALSE MEANS ITEM IS NOT EXISTING YET AND CAN BE ADDED TO THE RECORD
            if(itemExist==false)
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
                {
                    conn.Open();
                    string query = "INSERT INTO itemTable (serialnumber,itemtype,addedby,supplier,datedelivered,statusid,datedecommissioned,conditionid,itemtype,classtype)" +
                                                                "VALUES(@sernum,@itemtype, @userid, @supp, @datedel, @statid, @datedecom,@conid,@model,@class)";
                    SQLiteCommand command = new SQLiteCommand(query, conn);
                    command.Parameters.AddWithValue("@sernum", newItem.serialNumber);
                    command.Parameters.AddWithValue("@itemtype", newItem.itemType);
                    command.Parameters.AddWithValue("@userid", newItem.addedby);
                    command.Parameters.AddWithValue("@supp", newItem.supplier);
                    command.Parameters.AddWithValue("@datedel", newItem.datedelivered);
                    command.Parameters.AddWithValue("@statid", newItem.statusId);
                    command.Parameters.AddWithValue("@datedecom", newItem.datedecomm);
                    command.Parameters.AddWithValue("@conid", newItem.conditionId);
                    command.Parameters.AddWithValue("@model", newItem.itemType);
                    command.Parameters.AddWithValue("@class", newItem.classType);

                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return itemExist;
            
        }

        //FUNCTION TO REMOVE AN ITEM TO THE RECORD
        //IF TRUE IF ITEM IS FOUND AND REMOVED
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

        //FUNCTION TO GET ITEM TYPES
        public static List<string> getItemtypes()
        {
            List<string> itemType = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                
               
                conn.Open();
                string query = "SELECT name FROM type group by name";

                SQLiteCommand command = new SQLiteCommand(query, conn);


                using (SQLiteDataReader reader = command.ExecuteReader())
                    while (reader.Read())
                {
                    itemType.Add(reader.GetString(0));
                }
                conn.Close();
            }

            return itemType;

        }

        //function to get models based on selected item type
        public static List<item> getModel(string itemType)
        {
            List<item> modelList = new List<item>();

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {


                conn.Open();
                string query = "SELECT model from type where name = @itemtype group by model";

                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@itemtype", itemType);

                
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        item p = new item();
                        p.model = reader.GetString(0);
                        modelList.Add(p);


                        // Process people...      
                    }

                }
                conn.Close();
            }

            return modelList;

        }

        // FUNCTION FOR ADDING BROKEN ITEMS
        //RETURNS TRUE IF ITEM IS ALREADY SAVED IN THE LIST
        public static bool breakageAdd(string serialnum,int userId,int studentNum,string daterec)
        {
            bool breakageExist = false;
            DataTable breakagetable = new DataTable();

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                
                conn.Open();
                string query = "SELECT studentid FROM breakageInformation WHERE serialno=@sernum";

                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@sernum", serialnum);
                
                
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        breakageExist = true; //MEANS THAT IT IS ALREADY IN THE RECORD
                    }
                    else
                    {
                        string querynew = "INSERT INTO breakageInformation(serialno,studentid,recordedby,daterecorded) VALUES(@sernum,@stdnum,@recby,@datrec)";

                        SQLiteCommand cmd = new SQLiteCommand(querynew, conn);

                        cmd.Parameters.AddWithValue("@sernum", serialnum);
                        cmd.Parameters.AddWithValue("@stdnum", studentNum);
                        cmd.Parameters.AddWithValue("@recby", userId);
                        cmd.Parameters.AddWithValue("@datrec", daterec);

                        cmd.ExecuteNonQuery();
                    }
                    
                }


                conn.Close();
                /*
                    conn.Open();
                    string querynew = "INSERT INTO breakageInformation(serialno,studentid,recordedby,daterecorded) VALUES(@sernum,@stdnum,@recby,@datrec)";

                    SQLiteCommand cmd = new SQLiteCommand(querynew, conn);

                    cmd.Parameters.AddWithValue("@sernum", serialnum);
                    cmd.Parameters.AddWithValue("@stdnum", studentNum);
                    cmd.Parameters.AddWithValue("@recby", userId);
                    cmd.Parameters.AddWithValue("@datrec", daterec);

                    cmd.ExecuteNonQuery();
                    conn.Close();
               */

            }

                return breakageExist;
        }

        public static void UpdateEdit(item ediItem)
        {

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                string querynew = "UPDATE itemTable SET itemtype = @i,addedby = @a,supplier= @s, statusid = @stat, conditionId = @cond, datedelivered = @datedel, datedecommissioned = @datedecom WHERE serialnumber = @sernum";

                SQLiteCommand cmd = new SQLiteCommand(querynew, conn);
                cmd.Parameters.AddWithValue("@i", ediItem.itemType);
                cmd.Parameters.AddWithValue("@a", ediItem.addedby);
                cmd.Parameters.AddWithValue("@s", ediItem.supplier);
               
                cmd.Parameters.AddWithValue("@stat", ediItem.statusId);
                cmd.Parameters.AddWithValue("@datedel", ediItem.datedelivered);
                cmd.Parameters.AddWithValue("@datedecom", ediItem.datedecomm);
                cmd.Parameters.AddWithValue("@cond", ediItem.conditionId);
                cmd.Parameters.AddWithValue("@sernum", ediItem.serialNumber);


                cmd.ExecuteNonQuery();


                conn.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        public static void UpdateBreakage()
        {

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                string querynew = "UPDATE itemTable SET conditionId = 3 WHERE itemTable.serialnumber in (SELECT breakageInformation.serialno from breakageInformation)";

                SQLiteCommand cmd = new SQLiteCommand(querynew, conn);

                cmd.ExecuteNonQuery();


                conn.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        public static void UpdateBreakage(string serial)
        {

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                string querynew = "UPDATE itemTable SET conditionId = 1 WHERE itemTable.serialnumber = @word";
                SQLiteCommand cmd = new SQLiteCommand(querynew, conn);

                cmd.Parameters.AddWithValue("@word", serial);

                cmd.ExecuteNonQuery();


                conn.Close();
            }
        }





        //FUNCTION FOR REMOVING BROKEN ITEMS IN THE DATABASE
        //RETURNS TRUE IF BROKEN ITEM IS REMOVED
        public static bool breakageRemove(string serialnum)
        {
            bool breakageExist = false;

            string query = "SELECT studentid FROM breakageInformation WHERE serialno = @sernum";
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@sernum", serialnum);

                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    breakageExist = true; //MEANS THAT IT IS ALREADY IN THE RECORD
                }

                if (breakageExist == true)
                {
                    query = "DELETE FROM breakageInformation WHERE serialno = @sernum";
                    command = new SQLiteCommand(query, conn);
                    command.Parameters.AddWithValue("@sernum", serialnum);

                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
            return breakageExist;

        }

        //FUNCTION FOR GETTING ITEM INFORMATION USED FOR THE EDIT FUNCTION
        //RETURNS TRUE IF THE ITEM IS EXISTING AND EDITED
        public static bool searchitem(string serialNum,ref item foundItem, ref List<string> arr)
        {
            bool itemExist = false;

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                //PLACE USERTYPELATER ON IN THE SCRIPT
                string query = "SELECT itemtype,addedby,supplier,datedelivered,itemTable.statusid,datedecommissioned,conditionId, " +
                    "type.model,type.name,status.description,condition.description,account.username FROM itemTable INNER JOIN type ON type.typeid= itemTable.itemtype " +
                    "INNER JOIN status ON status.statusid = itemTable.statusid INNER JOIN condition ON condition.id = conditionId INNER JOIN account ON account.id = addedby WHERE serialnumber = @sernum";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@sernum",serialNum);

                SQLiteDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    itemExist = true;
                    while(reader.Read())
                    {
                        foundItem.itemType = reader.GetInt32(0);
                        foundItem.addedby = reader.GetInt32(1);
                        foundItem.supplier = reader.GetString(2);
                        foundItem.datedelivered = reader.GetString(3);
                        foundItem.statusId = reader.GetInt32(4);
                        foundItem.datedecomm = reader.GetString(5);
                        foundItem.conditionId = reader.GetInt32(6);
                        foundItem.model = reader.GetString(7);

                        arr.Add(reader.GetString(8));//0 - NAME
                        arr.Add(reader.GetString(9));//1 - stat desc
                        arr.Add(reader.GetString(10));//2 - cond desc
                        arr.Add(reader.GetString(11));//3 -  acc user
                    }
                }

                conn.Close();
            }

            return itemExist;
        }

        public static string getUsername(string serNum)
        {
            string username = "null";

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MUlab.db"))
            {
                conn.Open();
                //FUNCTION TO GET THE USERNAME
                string query = "SELECT username FROM account WHERE id =@sernum ";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@sernum", serNum);

                SQLiteDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    username = reader.GetString(0);
                }
                conn.Close();
            }
            return username;
        }

        public static void ExcelConvert(string saveFile)
        {
            FileInfo excelFile = new FileInfo("MULabInventory.xlsx");
            FileInfo sfd = new FileInfo(saveFile);
            using (ExcelPackage excel = new ExcelPackage(excelFile))
            {
                var worksheets = excel.Workbook.Worksheets["Summary Report"];
                worksheets.Cells["B3"].Value = 14;
                
                
                excel.SaveAs(sfd);
                bool isExcelInstalled = Type.GetTypeFromProgID("Excel.Application") != null ? true : false;
                if (isExcelInstalled)
                {
                    System.Diagnostics.Process.Start(sfd.ToString());
                }
                else
                {
                    MessageBox.Show("Microsoft Excel Not Installed", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }



        
    }
}
