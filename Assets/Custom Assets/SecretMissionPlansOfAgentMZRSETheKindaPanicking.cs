//Don't act like i'm doing anything productive in here unity like you'll even be relevant after all the BS you pulled.

//ANYWAYS I know that these requirements need fufillin, and so that's what imma do. let's figure out how we can get us a game loop

//REQ 1 - Enemy NPCs. They gotta have
//          B03)Spatial awareness
//          B03A)Limited Range
//          B03b)and should face the player. Looks like that one tutorial I found should do the trick...
//          D02) NPCs gotta shoot at ya AND THE MAN'S HAND IS REVEALED. HOW'D I GUESS

//REQ 2 - B04) High score should have meaning. Well then... with how i usually make gaem, that's gonna be tricky. 
//          you see, i don't usually do score attack games... usually things like speed or complete objective.
//          so what mechanic can I incentivise with score???
//          D04B) Ok, so win/lose condition dosen't need to be score based... but still. How TF am I gonna do this

//REQ 3 - C02) Player health system. Ewwwwwwww, u want me to make this just a generic shooter, lim? plz no... 
//          C03) gotta have a damage volume to test it too. I mean, easy peasy pudding and pie, but why tho

//REQ 4 - C04) HUD. No duh.

//REQ 5 - D04C) New player abilities. Examples are more weapons, AOE damage, weapon, uh, recoil... right click aim...
//          b-b-b-b-BOOOORING. Not making a generic shooter, no siree, not with the fps microgame RIGHT THERE
//          Well let's think in terms of paint game we came up with. That's more weapons
//          Spray can to graffiti walls
//          Paintball gun for enemies
//          and... what was it for splatoon ink spray? Water gun? ¯\_(0_0)_/¯

//REQ 6 - D04D) Pickups. Easy, refill ammo. 

//REQ 7 - D04E) "Obstacles". More complex enemy AI is an example. Welp
//          ok let's not take it for granted. Moving platforms? Turrets? Security cameras?

//REQ 8 - D04F) New game mechanics that affect gameplay and game state. 
//          uh. i mean, idk what my objective is yet so idk what extra shiz i could have

//REQ 9 - Feedback systems for everything. UI, visuals, and sound included. 


//So let's lay this out FR fr. 
// Gameplay will be stealth based graffiti-ing. Bc bomb rush cyberfunk ROCKS. 
// Player has 3 weapons - 
//      Spray can for doing the actual graffiti pieces
//      Paintball gun for KO-ing enemies
//      And I guess water gun to paint environment.

// Enemies will be patrolling guards with water guns. They can clean up ur paint, and also shoot u dead.
// That online tutorial i found will be good to help set up the whole patrol/search/attack state machine.
// The cameras idea isn't too bad... if you get in it's vision cone, it'll ping the nearest guard. All guards? whatever's easier.
//      Ooh! maybe you can paint the camera to disable it


//Objective is to graffiti one massive piece at the end of the level. 
// You get extra points for 1. doing extra smaller pieces
// ||) Leaving paint on the environment w/o having it be cleaned up
// 3- Time bonus. Faster the better

//Tricky shit
// {Graffiti Minigame} JSR/BRC had that shiz, so i gotta figure out how to do it on my end.
// {Splatoon decal system} there's some tutorials i saw, but i doubt their helpfulness...
// {decals affect player} run faster on painted floor? Wall jump off painted walls? That's more coding
// {Scoring system} All these elements have all gotta be hooked up. Balls. Tiddies even
// {UI} will be neccesary, and while mr 'i guess i like ai now' will be helpful i still gotta code to make it work
// {Player and enemy health} I get i'll probably find a walkthru, but i'm just nervie

//Win State - Graffiti the piece and make it back to the entrance.
//Lose State - Lose all ur health. (...health pickup? or health from graffiti)

//OK, we got a game. Now... 

//          TO DO LIST
//  -Program Enemy Ai
//  