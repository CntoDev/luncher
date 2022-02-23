# Carpe Noctem Tactical Operations - Arma 3 Server Launcher
CNTO Luncher is a tool used to run Arma 3 Server in Carpe Noctem Tactical Operations community. It has following features:
- Configurable repository selection
- Desktop and Web version
- Runs with novel DLCs
- Unlimited repositories (Desktop)
## Repository configuration
Both desktop and web version contain repository definition in `appsettings.json` file in their respective folder.
### Web repository configuration
Do not change `Id` value of repositories as they are referenced in web application.
```json
{
    "ConnectionStrings": {
      "DefaultConnection": "DataSource=app.db;Cache=Shared" // Connection to local SQLite database
    },
    "AllowedHosts": "*", // allow all hosts to connect
    "Urls": "http://*:8080", // run Kestrel web server at port 8080 at all local IP interfaces
    "Luncher": {
        "GamePath": "C:\\a3server\\arma3server_x64.exe", // path to game server
        "ProfilePath": "C:\\a3server\\profiles", // path to arma 3 profile
        "ConfigDirectory": "C:\\a3server", // path to directory with configuration files
        "ServerPassword": "****",
        "Repositories": [
            {
                "Id": "Main", // name of the repository
                "Path": "C:\\a3server\\mods_cnto", // repository path
                "Priority": 1 // repository loading order, bigger number means later loading order
            },
            {
                "Id": "Campaign",
                "Path": "C:\\a3server\\mods_cnto_campaign",
                "Priority": 2
            },		
            {
                "Id": "Dev",
                "Path": "C:\\a3server\\mods_cnto_dev",
                "Priority": 3
            },
            {
                "Id": "Vietnam",
                "Path": "C:\\a3server\\mods_vietnam",
                "Priority": 4
            },		
            {
                "Id": "Server Only",
                "Path": "C:\\a3server\\mods_cnto_server_only",
                "Priority": 1,
                "ServerSide": true // set to true if repository is loaded only on server	
            }
        ]
    }	
}
```
### Desktop repository configuration
```json
{
    "GamePath": "C:\\a3server\\arma3server_x64.exe", // path to game server application
    "ProfilePath": "C:\\a3server\\profiles", // path to profile folder
    "ConfigDirectory": "C:\\a3server", // path to folder with config files
    "ServerPassword": "******", // server password
    "Repositories": [
        {
            "Id": "Main", // name of the repository
            "Path": "C:\\a3server\\mods_cnto", // repository path
            "Priority": 1 // repository loading order, bigger number means later loading order
        },
        {
            "Id": "Campaign",
            "Path": "C:\\a3server\\mods_cnto_campaign",
            "Priority": 2
        },		
        {
            "Id": "Dev",
            "Path": "C:\\a3server\\mods_cnto_dev",
            "Priority": 3
        },
        {
            "Id": "Vietnam",
            "Path": "C:\\a3server\\mods_vietnam",
            "Priority": 4
        },		
        {
            "Id": "Server Only",
            "Path": "C:\\a3server\\mods_cnto_server_only",
            "Priority": 1,
            "ServerSide": true // set to true if repository is loaded only on server	
        }
    ]
}
```
## Deployment notes (CNTO only)
These are deployment notes for CNTO server, for CNTO administrators only.
### Desktop version
Desktop version is deployed at `D:\CNTO.Launcher` folder. To start the Luncher simply start `UI.exe` application. Desktop version has an autorestart feature which was used as scheduled job at 19:00 every Tuesday and Friday.
### Web version
Web version is deployed at `D:\CNTO.Launcher.Web` folder. Web server is started by opening Powershell and running `D:\CNTO.Launcher.Web\CNTO.Launcher.Web.exe` application.

