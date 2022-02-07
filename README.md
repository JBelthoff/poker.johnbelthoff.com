# Poker Hand Evaluator
A working version of this code is located here: https://poker.johnbelthoff.com/

## About John Belthoff's Texas Holdem & Poker Hand Evaluator
This is a web project I started one day to see if I could re-create software to control a game of Texas Holdem Poker.  

The project is written using Microsoft Asp.Net in c# and relies on a port from c++ to c# of Cactus Kev's Poker Hand Evaluator.  

I don't know Cactus Kev and his original webpage is no longer on the web AFAIK so I have included a <a href="/cactus-kev2.html">copy of Cactus Kev's original article</a> on this site for convenience or so you can follow along using his logic and mathematics.  

I have about 20-30 hours of real work into the program and at this point the game plays with 9 players, changes dealer, calculates the winning hand and displays the results.  

Certainly there is much more programming to be done to achieve a fully functional Texas Holdem Poker program, however I feel I have tackled the basics, or the main meat, of the challenge.  

The rest should be rather simple and straightforward.  

If you have any questions please feel free to contact me anytime.  

Otherwise, Enjoy!

## Instructions for use
1. Create an MS SqlServer Database named: PokerApp
2. Create a Login and User for the Database
3. Run the Script named "CreateDB.sql" on the DBase PokerApp - File is located in x_dBase directory
4. Create an IIS website on a Windows Server
5. Configure the IIS website (AppPools, HostHeader, etc...)
6. Update Web.config file with your DBase Credentials
7. Install, or publish, Web App to the Webserver
8. View the URL in a browser and Play Poker!
