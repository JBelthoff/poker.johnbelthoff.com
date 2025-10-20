# Poker Hand Evaluator

> ⚠️ **Legacy Notice**
>
> This repository contains an earlier version of the Poker Hand Evaluator written for **ASP.NET WebForms**.  
> The latest, modernized implementation built with **ASP.NET Core (Razor Pages)** is available here:  
> 👉 **[JBelthoff/poker.net](https://github.com/JBelthoff/poker.net)**  
> Live modern app: **https://poker-calculator.johnbelthoff.com/**

---

## About John Belthoff's Texas Holdem & Poker Hand Evaluator

This is a web project I started one day to see if I could re-create software to control a game of Texas Holdem Poker.  

The project is written using Microsoft ASP.NET in C# and relies on a port from C++ to C# of Cactus Kev's Poker Hand Evaluator.  

I don't know Cactus Kev and his original webpage is no longer on the web AFAIK, so I have included a [copy of Cactus Kev's original article](/cactus-kev2.html) for reference and convenience.  

I have about 20–30 hours of real work into the program, and at this point the game plays with 9 players, changes dealer, calculates the winning hand, and displays the results.  

Certainly there is much more programming to be done to achieve a fully functional Texas Holdem Poker program, however I feel I have tackled the basics — the main meat of the challenge.  

The rest should be rather simple and straightforward.  

If you have any questions, please feel free to contact me anytime.  

Otherwise, enjoy!

## Instructions for use

1. Create an MS SQL Server database named `PokerApp`
2. Create a Login and User for the database
3. Run the script named **CreateDB.sql** on the `PokerApp` database — file is located in the `x_dBase` directory
4. Create an IIS website on a Windows Server
5. Configure the IIS website (AppPools, HostHeader, etc.)
6. Update `Web.config` with your database credentials
7. Publish the Web App to the web server
8. View the URL in a browser and play Poker!
