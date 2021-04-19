# TheGame

Contexto
Você está trabalhando para uma empresa de jogos online que opera vários servidores de jogos. Cada jogo resulta em ganho ou perda de pontos
para o jogador.
Os dados são mantidos em memória por cada servidor e periodicamente esses dados são persistidos. Sua tarefa é implementar um serviço que
exponha 2 endpoints:

Endpoint 1:
Permite que os servidores persistam os dados do resultado de um jogo:
 GameResult: 
   PlayerId (long) -> ID do jogador
   GameId (long) -> ID do jogo 
   Win (long) -> o número de pontos ganhos (positivos ou negativos)
   Timestamp (date time) -> data de quando o jogo foi realizado (UTC)

Como resultado da chamada a esse endpoint o balanço dos pontos do jogador devem ser primeiramente armazenado em memória no servidor
e após um determinado tempo deverá persistir todas as informações que estão em memória para o banco de dados de uma só vez.
Esse tempo deverá ser facilmente configurável pelo usuário que vai implantar a solução, pois ainda não se sabe ainda qual será a 
volumetria de informações e as especificações do servidor. Se um jogador não tem um registro do balanço dos seus pontos no banco de dados,
ele deverá ser criado. NOTA: Um grupo de dados pode conter diversos registros de um único jogador (i.e. o jogador participou em vários
jogos).
Existem múltiplos servidores de jogos, realizando partidas simultâneas de jogos diferentes, então o serviço irá receber várias requisições
concorrentes, que podem conter resultados do mesmo jogador. Inicialmente este serviço irá rodar como um piloto em um único servidor.
Portanto, dados perdidos devido ao mal funcionamento do servidor ou do serviço não é considerado crítico, mas não deveria ocorrer dentro
de circunstâncias normais. 

Endpoint 2: 
Esse endpoint permite que os web sites onde o jogador inicia os jogos mostre um placar da classificação dos 100 melhores jogadores. Os
100 melhores jogadores são ordenados pelo balanço de pontos que eles possuem em ordem descendente.
Ele retornará os seguintes dados:
Leaderboard:
   PlayerId (long) -> ID do jogador
   Balance (long) -> balanço de pontos do jogador
   LastUpdateDate (date time) -> data em que o balanço de pontos do jogador foi atualizado pela última vez (usando o fuso horário do
   servidor de aplicação) 

NOTA:
Os jogadores são muito competitivos, e há vários jogadores ativos ao mesmo tempo, então o endpoint do placar de classificação será chamado
várias vezes (milhares de requisições por minuto). Ainda não se sabe o quão valiosa essa funcionalidade será para o negócio da empresa,
então inicialmente isso irá rodar como um piloto em um servidor dedicado que precisa lidar com toda a carga.

Expectativas da entrega:
Para essa tarefa você é livre para escolher qualquer tecnologia .Net que for mais favorável para cada aspecto da implementação.
Se você decidir usar um framework que talvez não seja o melhor candidato mas você o escolheu porque estás mais familiarizado com ele,
não tem problema, desde que você indique o porquê da escolha (i.e. Eu escolhi o framework X, mas o framework Y seria o ideal para
acesso ao banco de dados porque a. b. c. ...). Não é esperado que você conheça novos frameworks para essa tarefa desde que você consiga
explicar o porquê das suas escolhas claramente.
É esperado também que a aplicação esteja publicada no github e seja entregue funcional, então se houver qualquer condição especial para
que ela seja configurada, como endereço de banco de dados, utilização de migration, entre outros... por favor providencie as dentro do
arquivo README.MD dentro do repositório git criado e documentando detalhadamente o processo a ser realizado.

É esperado que o código seja de qualidade, seguindo boas práticas de desenvolvimento, patterns, clean code, estrutura de projetos do tipo
DDD / TDD... então não o escreva como 'é só uma demo' mas como você o faria para um produto real. Você deve considerar a tarefa completa
quando você considera o seu código 'pronto para produção/implantação'.
Se você tiver qualquer dúvida, por favor entre em contato conosco.

-----------------------------------------------------------------------------------------------------

Execution and deployment instructions

The steps described below assume you are in a Windows operating system.

For more information about how to run in other operating systems, pelase refer to the following url:
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

You can test the application at this point by opening a web browser and then reaching out to the following url's:
http://localhost:5000/swagger
http://localhost:5000/api/v1/leaderboards

3. Now, to switch to a production environment:<br/>
Open command prompt and go to "TheGame" solution directory the type the following:<br/>
 dotnet build -c release [HIT ENTER]<br/>
 set ASPNETCORE_ENVIRONMENT=Production [HIT ENTER]<br/>
 dotnet run --no-launch-profile [HIT ENTER]<br/>

With this setup, the application does not include the Open API Swagger documentation.
It only accepts requests from the relevant application endpoints.
Also, neither migrations nor database load are performed.

Application endpoints
1. SAVE GAME DATA ENDPOINT<br/>
(POST) http://localhost:5000/api/v1/match -> This is the entry point for receiving game match data
Request body example:
{
    "gameId" : 1,
    "playerId" : 1,
    "win" : 68000000,
    "timestamp" : "2021-04-17T14:28:34Z"
}

2. FETCH GAME DATA ENDPOINT (EXTRA endpoint)<br/>
(GET)  http://localhost:5000/api/v1/cache-data/game-matches -> This endpoint displays all pending match data that has been posted for later persistence.

3. FETCH A LIST WITH AVAILABLE GAMES IDS (EXTRA endpoint)<br/>
(GET)  http://localhost:5000/api/v1/cache-data/games -> This endpoint displays all games ids registered in the system
Useful for creating requests when using the endpoint explained in item 1

4. FETCH A LIST WITH AVAILABLE PLAYERS IDS  (EXTRA endpoint)<br/>
(GET)  http://localhost:5000/api/v1/cache-data/players -> This endpoint displays all players ids registered ine th system
Useful for creating requests when using the endpoint explained in item 1

5. LEADERBOARDS<br/>
(GET)  http://localhost:5000/api/v1/leaderboards -> This endpoint displays the leaderboards after the database flushing background service
has been executed at least once.

How to play with this solution<br/>
1.Invoke a couple of times the endpoint 1. You can use a tool like Postman or Advanced REST Client to send some requests as per explained in item 1.<br/>
2.In this moment, invoke the endpoint 2 to check that match data is in the cache. These are data waiting to be flushed to the database.
From this moment on, you can keep inserting data and, after approximately 40 seconds, the database flushing service starts its job to persist all match data
it can get from cache in that moment, without losing new posted match data.
Everytime the background service runs, the leaderboards is automatically refreshed and its data is stored in cache for performance.
As soon as the game match data is flushed to the database, all processed data are removed from the cache preserving the ones that weren't processed yet,
waiting for the next service execution.<br/>
3. Invoke the endpoint 5 for leaderboards

Running the solution automated tests (both unit and integration ones)
1. Open command prompt and go to "TheGame" solution directory.
2. Next, type the following:
   dotnet test [HIT ENTER]
