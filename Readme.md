Description
ADO.NET was chosen to interact with MS SQL Server database for the better performance in bulk import. The SqlBulkCopy() method is highly optimized to insert large amount of data efficiently

Executing program

1. Create the database and table
   create database PostDb;
   use PostDb;
   create table Posts (Id int primary key, UserId int not null, Title varchar(200) not null, Body varchar(1000) not null);

2. Add required packages
   dotnet add package System.Data.SqlClient via cmd
   Install-Package System.Security.Permissions -Version 7.0.0

3. Rename the Server name according the connected server

4. Run the app
