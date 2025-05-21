TO start the app you must have connection string setted up in your config file(appsettings.Development.json or appsettings.json) like that
  "ConnectionStrings": {
    "DefaultConnection": "Your_connection_string"
  }

I decided to not split project because scaffolding created Data folder in CampanyApi project and i think it can be a problem with references if I will create separate projects for DTO and Controllers 
