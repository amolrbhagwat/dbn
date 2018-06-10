# dbn
Database Navigator built in C#

## What is it?
A Windows program to connect to, and view data stored in databases.

## Why dbn?
For common tasks such as lookup and browsing, dbn eliminates the need to write SQL.

The key feature of dbn is the ability to view data in other tables which have the same field and value as the the record you are interested in. This makes it easy to cross-check data when investigating issues with queries which comprise of multiple table joins.

## What's currently supported?
- Connecting to MySql databases (more coming soon!)
- Fetching all (textual) data from a table
- Fetching data from a table which matches certain criteria
- Looking up related data in other tables

## Using dbn
Here's the general workflow for dbn:

1. Double-click to connect to a database from the list of database connections.

   ![Connecting to a database](/../screenshots/screenshots/selecting-database.png?raw=true "Connecting to a database")

2. Click on a table name to see the fields present in it.

   ![Selecting a table](/../screenshots/screenshots/selecting-table.png?raw=true "Selecting a table")

3. Enter values for the fields to fetch records matching those values. If no values are entered, the entire table will be fetched. Click the 'Fetch' button.

   ![Fetching records matching certain criteria](/../screenshots/screenshots/records-with-criteria.png?raw=true "Fetching records matching certain criteria")

4. Right-click on any cell in the results grid to see a list of tables containing the same field. Select a table name to fetch all records from that table which contain that same value for the field.

   ![Other tables with same field](/../screenshots/screenshots/other-tables-with-field.png?raw=true "Other tables with the same field")

5. The results will be updated automatically.

   ![Rows found in the other table](/../screenshots/screenshots/rows-in-other-table.png?raw=true "Rows found in the other table")

## Known issues
Please refer the issues tab at the top of the page.

## Credits
dbn uses the following packages:
1. MySQL connector for C# [(NuGet page)](https://www.nuget.org/packages/MySql.Data/)
2. INI Parser [(NuGet page)](https://www.nuget.org/packages/ini-parser/)

Classicmodels database schema and data, courtesy and copyright [mysqltutorial.org](http://www.mysqltutorial.org/mysql-sample-database.aspx).

Rfam database connection details from [here](http://rfam.readthedocs.io/en/latest/database.html).
