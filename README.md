###DTank documentation

The project contains a client and a master game server along with an integrated DMarket widget (BasicWidget) for DTanks game.
Project’s Basic Structure

####The structure of the game is broken down into the following scenes:

1. *_start* is the base scene with the main controller
2. *SelectAppType*. Contains the application selection logic: client, server, or offline
3. *Lobby*. Contains logic for server connection
4. *Authorization*. Contains the logic for authorization / registration within DTanks game
5. *Game*. Contains game’s battle logic; if mode is “multiplayer”, every player can get virtual in-game items
6. *Server*. Contains server logic
7. *Shop*. Contains the game content interaction logic and the logic of interaction with the Basic Widget

Basic Structure of Basic Widget Integration Part

####On a client side, you can find the following:

1. *_start* scene
The BasicWidget prefab itself is located.
2. *Shop* scene
ShopSceneController has special stacks to work with BasicWidget in the
Shop.DMarketIntegration.BasicWidgetStates folder, which contains the logic for
authorizing and part of the interaction with the server in the context of BasicWidget.
AppShopSceneState contains an intermediate logic for requests and responses between
Shop and the game server
3. *Server* scene
The ServerDMarketIntegrationCommand command contains the logic for working with
ServerApi
4. *DMarketInfoConverter.cs* +
It is used for mutual conversion of in-game and DMarket item formats within in-game
operatio
