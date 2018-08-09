### DTank documentation

The project contains a client and a master game server along with an integrated DMarket widget for DTanks game.
Project’s Basic Structure

#### The structure of the game is broken down into the following scenes:

1. *_start* is the base scene with the main controller
2. *SelectAppType*. Contains the application selection logic: client, server, or offline
3. *Lobby*. Contains logic for server connection
4. *Authorization*. Contains the logic for authorization / registration within DTanks game
5. *Game*. Contains game’s battle logic; if mode is “multiplayer”, every player can get virtual in-game items
6. *Server*. Contains server logic
7. *Inventory*. Contains the game content interaction logic and the logic of interaction with the Basic Widget