# WeatherData
     
$ export ASPNET_CORE_ENVIRONMENT=development    
$ npm install              
$ dotnet restore      
$ npm update           
$ webpack --config webpack.config.vendor.js
$ webpack --config webpack.config.js
      
$ sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=PaSSword12!' -p 1433:1433 -d microsoft/mssql-server-linux    
$ docker ps       
      
Ewentualnie w appsettings.json jest connection string o nazwie: "WindowsLocalSQLServerDatabase"
Mozna go podmienić w Startup.cs za "MacOsSQLServerDatabase" i aplikacja powinna działać z lokalną bazą danych w VisualStudio.    
      
$ dotnet run      
