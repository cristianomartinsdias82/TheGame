# TheGame

(Português do Brasil)<br />
Instruções de execução e implantação

Os passos a seguir presumem que vocÊ está utilizando o sistema operacional Windows.

Para maiores informações sobre como executar a aplicação em outros sistemas operacionais, por consulte a url a seguir:<br/>
https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/environments?view=aspnetcore-5.0

Pré-requisitos<br />
Instale o gerenciador de banco de dados Sql Server<br />
Instale o kit de desenvolvimento de software .NET Core versão 3.1<br />
Ferramentas como Postman ou Advanced REST Client também poderão ser úteis para realizar solicitações aos pontos de acesso da solução<br />
Vale mencionar que a ferramenta Visual Studio 2017 ou posterior é desejável mas não é necessária<br />

Iniciando<br/>
Primeiramente, baixe esta solução para a sua máquina.

Como rodar esta solução<br />
1. Abra uma janela de comando ("Command prompt") e navegue até o diretório do projeto Web "The Game".<br />
2. Em seguida, digite as seguintes instruções:<br />
   set ASPNETCORE_ENVIRONMENT=Development [PRESSIONE A TECLA ENTER]<br/>
   dotnet run [PRESSIONE A TECLA ENTER]<br/>
   
(Estes comandos irão criar a base de dados, as tabelas, relacionamentos, a visão de placar dos jogadores e, por último executar uma carga essencial de dados)<br/>

Aqui você já pode testar a aplicação abrindo um navegador de internet e acessando o(s) seguinte(s) endereços:<br />
http://localhost:5000/swagger<br/>
http://localhost:5000/api/v1/leaderboards<br/>

3. Para gerar uma versão da aplicação candidata para ambiente de produção:
Abra uma janela de comando ("Command prompt") e navegue até o diretório da solução "The Game".<br />
4. Em seguida, digite o seguinte comando:<br/>
 dotnet build -c release [PRESSIONE A TECLA ENTER]<br/>
 set ASPNETCORE_ENVIRONMENT=Production [PRESSIONE A TECLA ENTER]<br/>
 dotnet run --no-launch-profile [PRESSIONE A TECLA ENTER]<br/>

Com esta configuração. a aplicação não disponibiliza a documentação eletrônica Open Api Swagger.
Ela aceita somente solicitações dos pontos de acessos relevantesda aplicação.
Também, nem migrações nem cargas de banco de dados são realizadas.

--------------------------------------------------------------------------------------------------------------------------------------------------

(English)
Execution and deployment instructions

The steps described below assume you are in a Windows operating system.

For more information about how to run in other operating systems, pelase refer to the following url:<br/>
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-5.0

Prerequisites<br/>
Please make sure the Sql Server is up and running on your machine.<br/>
Also, .Net Core 3.1 SDK must be installed.<br/>
Tools like Postman or Advanced REST Client may also come in handy to make requests to the solution endpoints<br/>
It's worth to mention that Visual Studio 2017 or later is desirable but not required<br/>

Getting started<br/>
First things first, download this solution to your machine.

How to run this solution<br/>
1. Open command prompt and go to "TheGame" web project directory.<br/>
2. Next, type the following:<br/>
   set ASPNETCORE_ENVIRONMENT=Development [HIT ENTER]<br/>
   dotnet run [HIT ENTER]<br/>

(This will create the database along with all tables, relationships, the leaderboards view and will ultimately perform initial data seeds)<br/>

You can test the application at this point by opening a web browser and then reaching out to the following url's:<br/>
http://localhost:5000/swagger<br/>
http://localhost:5000/api/v1/leaderboards<br/>

3. Now, to switch to a production environment:<br/>
Open command prompt and go to "TheGame" solution directory.<br/>
4. Next, type the following:<br/>
 dotnet build -c release [HIT ENTER]<br/>
 set ASPNETCORE_ENVIRONMENT=Production [HIT ENTER]<br/>
 dotnet run --no-launch-profile [HIT ENTER]<br/>

With this setup, the application no longer includes the Open API Swagger documentation.
It only accepts requests from the relevant application endpoints.
Also, neither migrations nor database loads are performed.

Application endpoints
1. SAVE GAME DATA ENDPOINT<br/>
(POST) http://localhost:5000/api/v1/match -> This is the entry point for receiving game match data<br/>
Request body example:<br/>
{
    "gameId" : 1,
    "playerId" : 1,
    "win" : 68000000,
    "timestamp" : "2021-04-17T14:28:34Z"
}

2. FETCH GAME DATA ENDPOINT (EXTRA endpoint)<br/>
(GET)  http://localhost:5000/api/v1/cache-data/game-matches -> This endpoint displays all pending match data that has been posted for later persistence.

3. FETCH A LIST WITH AVAILABLE GAMES IDS (EXTRA endpoint)<br/>
(GET)  http://localhost:5000/api/v1/cache-data/games -> This endpoint displays all games ids registered in the system<br/>
Useful for creating requests when using the endpoint explained in item 1

4. FETCH A LIST WITH AVAILABLE PLAYERS IDS (EXTRA endpoint)<br/>
(GET)  http://localhost:5000/api/v1/cache-data/players -> This endpoint displays all players ids registered in the system<br/>
Useful for creating requests when using the endpoint explained in item 1

5. LEADERBOARDS<br/>
(GET)  http://localhost:5000/api/v1/leaderboards -> This endpoint displays the leaderboards after the database flushing background service
has been executed at least once.

6. API DOCUMENTATION (EXTRA endpoint)<br/>
(GET)  http://localhost:5000/swagger -> This endpoint makes available the API endpoints live documentation via Swagger OpenAPI.
IMPORTANT NOTE: This endpoint is available only when ASPNETCORE_ENVIRONMENT environment variable is set to Development (please refer to the beginning of topic "How to run this solution")

How to play with this solution<br/>
1. Invoke a couple of times the endpoint 1. You can use a tool like Postman or Advanced REST Client to send some requests to it.<br/>
2. In this moment, invoke the endpoint 2 to check that match data is in the cache. These are data waiting to be flushed to the database.<br/>
From this moment on, you can keep inserting data and, after approximately 40 seconds, the database flushing service starts its job to persist all match data
it can get from cache in that moment, without losing new posted match data.<br/>
Everytime the background service runs, the leaderboards is automatically refreshed and its data is stored in cache for performance.
As soon as the game match data is flushed to the database, all processed data are removed from the cache preserving the ones that weren't processed yet,
waiting for the next service execution.<br/>
3. Invoke the endpoint 5 for leaderboards

Running the solution automated tests (both unit and integration ones)
1. Open command prompt and go to "TheGame" solution directory<br/>
2. Next, type the following:<br/>
   dotnet test [HIT ENTER]
