# Project rundown

Daybreak is a custom launcher for the popular online RPG [Guild Wars](https://www.guildwars.com/en/).

## Getting Started

Follow this link to the [Getting Started page](Getting-Started)

## Features

- [Guild Wars REST API](Daybreak-API)
    Can inject a REST API into Guild Wars that exposes some Guild Wars information such as builds and character stats over http. Useful for automating some of the features of Daybreak (eg. Character Switching or displaying Faction stats)

- [Build template management](Build-Management)

    Can manage the local build templates, create/edit/delete build templates, search and import build templates from the internet. Daybreak support both single build templates as well as team build templates

- [Build template synchronization](Build-Management#build-synchronization)
    Can backup/restore build templates from a linked OneDrive account, show differences between local and remote builds and allows individual uploads/downloads

- [Integration with GWToolbox builds](Build-Management#integration-with-gwtoolbox-builds)

    Daybreak integrates with GWToolbox builds. Daybreak can load, edit and save builds from GWToolbox config as well as export vanilla builds to GWToolbox config

- [Manage user credentials](Credential-Management)

    Can manage multiple user credentials, allows the user to switch between them with the click of a button, stores the credentials encrypted locally and protects them using Windows APIs

- [Download Guild Wars](Download-Guild-Wars)

    Can download the Guild Wars installer and launch it

- [Executable Management](Executable-Management)

    Can manage multiple Guild Wars executables, allows the user to switch between them with the click of a button. Maintains the executable versions and checks for updates. Daybreak will also verify executables and prompt the user to fix them if it detects any issues

- [Multiple Launch Configurations](Launch-Configurations)

    Can manage multiple launch configurations with combinations of credentials and executables. Can attach to configurations once they're launched. Launch configurations support passing arguments to Guild Wars when launching

- [Multi-Box/Multi-Launch Support](Multi-Launch)

    Can manage and launch multiple instances of Guild Wars concurrently

- [Guild Wars Live Integration](Focus-View)

    Can collect information from the current running executable and shows it in a centralized view with links and shortcuts to useful information. Links current quest to the quest wiki page, current map to the map wiki page, build pages, loads builds from the embedded browser and more features

- [uMod/gMod Integration](GMod-Management)

    Can install and manage uMod. Can automatically launch uMod dll when launching Guild Wars. Can download and manage tpf files for uMod

- [GwToolboxpp Integration](Mods#manage-a-mod)

    Can install and manage GwToolboxpp. Can automatically launch GwToolboxpp when launching Guild Wars

- [DirectSong Integration](Mods#manage-a-mod)

    Can install and manage DirectSong. Can automatically launch DirectSong when launching Guild Wars

- [ReShade Integration](ReShade-Management)

    Can install and manage ReShade. Can automatically launch ReShade when launching Guild Wars. Can download and manage ReShade shader packs and deploys them on startup

- [Screen Auto-Placement](Screen-Affinity)

    Can auto-place the Guild Wars window on the desired screen and location at launch

- [Auto-Update](Version-Management)

    Can monitor for new Daybreak versions and automatically download and install new versions when they are available. Can roll-back to previous versions from the version management view

- [Gw Market](Gwmarket)

    Integrates with [Gw Market](https://gwmarket.net/) to provide automatic inventory scanning and player certification

- [Trade Chats](Trade-Chat)

    Integrates with [Kamadan](https://kamadan.gwtoolbox.com/)/[Ascalon](https://ascalon.gwtoolbox.com/) Trade Chat to provide access to live trade requests, query for trade requests. Thanks to the folks at [Kamadan-Trade-Chat](https://github.com/3vcloud/kamadan-trade-chat) for providing the functionality and allowing me to integrate it into Daybreak

- [Party Search](Party-Search)

    Integrates with [Party Search](https://party.gwtoolbox.com/) to provide access to live party listings

- [Trader Quotes](Trader-Quotes)

    Integrates with [Kamadan](https://kamadan.gwtoolbox.com/) to provide material prices and price trends. Provides a view for visualizing the history of prices

- [Notifications](Notifications)

    Implements a notification service that can display notifications and perform actions based on them

- [Trade Alerts](Trade-Alerts)

    Leverages the [Kamadan](https://kamadan.gwtoolbox.com/)/[Ascalon](https://ascalon.gwtoolbox.com/) Trade Chat integration to set up trade alerts based on rules. Will trigger [notifications](Notifications) when a trade message is found based on the configured rules. Daybreak supports alerts for [Trader Quotes](Trader-Quotes), with alerts when the price is higher or lower than an upper or a lower target

- [Create Guild Wars Copies](Copy-Guild-Wars-Executable)

    Can create new Guild Wars executables from existing ones, to be used with features such as [Multi-Launch](Multi-Launch). These copies will be automatically onboarded into Daybreak to be managed as all the other executables

- [Seasonal Events Support](Seasonal-Events)

    Daybreak detects when seasonal events are supposed to be active and notifies the user. Daybreak present the user with a Calendar view that shows each event and the days it takes place across days, months and years

- [Plugin Support](Plugins)

    Daybreak supports plugins written for the .net runtime. Daybreak can also enable/disable plugins. Please check the [wiki](Plugins) for details about plugin functionality and for a quickstart guide on writing your own plugins

- [Setting Synchronization](Settings-Synchronization)

    Daybreak can back up and restore the launcher settings to a OneDrive account. These settings can be shared across devices by logging into OneDrive through the Daybreak launcher

- [Command Line Arguments](Daybreak-Command-Line-Arguments)

    Daybreak supports command line arguments when launching. Please check [Command Line Arguments Page](Daybreak-Command-Line-Arguments) for supported arguments
