using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;


namespace COE131L
{
    class Database 
    {
        //public SQLiteConnection myConnection;

        public Database()
        {
            //myConnection = new SQLiteConnection("Data Source = MULabInventory.db");


        }



        public static string connectionStr()
        {
            return ConfigurationManager.ConnectionStrings["constring"].ConnectionString;
        }

        public static void insertAccount(User newUser)
        {
            using(SQLiteConnection conn = new SQLiteConnection(connectionStr()))
            {
                conn.Open();
                //PLACE USERTYPELATER ON IN THE SCRIPT
                SQLiteCommand command = new SQLiteCommand("INSERT INTO account(firstName,lastName,username,password) VALUES(@fname,@lname,@usname,@pass)",conn);
                command.Parameters.AddWithValue("@fname",newUser.firstName);
                command.Parameters.AddWithValue("@lname", newUser.lastName);
                command.Parameters.AddWithValue("@usname", newUser.userName);
                command.Parameters.AddWithValue("@pass", newUser.password);
                //command.Parameters.AddWithValue("@uType", newUser.userType);

                command.ExecuteNonQuery();

                conn.Close();
            }
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
