# ASP.NET Core Voice Legacy 

## Usage 

### Running 

#### CLI

 + `dotnet restore` 
 + `dotnet build` 
 + `dotnet run` 

App is running on port 5000(?).

Routes: `{host}:5000/service/voice` -> Main callback : routes request to inbound or outbound handlers based on `direction`. This is what should be in your callback. 

```bash
    {host}:5000/service/inbound -> Handles inbound requests 
    {host}:5000/service/outbound -> Handles outbound requests
    {host}:5000/service/dtmf -> Handles user input
```

#### Docker 
##### Docker CLI
+ Build `docker build . -t voiceapp` 
+ Run `docker run -d -p 7380:7380 voiceapp` 

`{host}:7380/service/*` <-> Routes as above

#### Docker-compose 

Just build and run  `docker-compose up -d` 

### Customizing 

+ Add DB service 
+ Profit
