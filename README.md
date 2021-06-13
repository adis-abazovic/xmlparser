# XMLParser

#### Prerequisites:
* .NET 5
* .NET CLI
* Visual Studio 2019 
  * XmlParser.API project (WebApi template) is created from Visual Studio so it can't be built using `dotnet run` command

### 1. Running the API/Server
1. Open the solution **XmlParser.sln** with Visual Studio 2019
2. Set startup projects to option **"Single Startup Project"** and choose **"XmlParser.API"**
3. Rebuild the solution
4. Run the project/Start Debugging (F5)

### 2. Running the Client (Console App)
1. Open Powershell terminal
2. Navigate to location of **XmlParser.Client** project
3. Execute:\
   `dotnet run -- {path_to_xml_file} {element_filters}`
   
   Example: `dotnet run -- C:\Users\Adis\Desktop\XMLPlay\congree.xml p li`
       
   > dotnet run -- C:\Users\Adis\Desktop\XMLPlay\congree.xml p li


### Notes
  - By default, API project is using **InMemoryDatabase**.  
  If you want to use SQL Server Database (MSSQLLocalDb) just un-comment the [following line](https://github.com/adis-abazovic/xmlparser/blob/30fa54dca5ee09d12d92044a5c80186cd6eaa866/XmlParser/XmlParser.API/Startup.cs#L34 ) in Startup.cs
  (and comment out the line that uses InMemoryDatabase)
  - SignalR is used for sending notification from server to the client about the progress of an operation
