# Daybreak
Custom client for Guildwars.
Requires webview2 runtime https://go.microsoft.com/fwlink/p/?LinkId=2124703.

![Showcase 1](https://media1.giphy.com/media/Z32o0OZ5pZHDOIodzD/giphy.gif)
![Showcase 2](https://media0.giphy.com/media/aQ8Wl7lsuhT0AblCPI/giphy.gif)
![Showcase 3](https://media2.giphy.com/media/s06PtxgeAAZtoJhTx6/giphy.gif)

# Examples/Usage
To modify any settings, press the settings button on the titlebar
![Settings button](https://i.imgur.com/0QSTvNF.png)

When launching, if any of the required settings are not valid (missing username/password/character name), the launcher will open the settings page.
![Settings page](https://i.imgur.com/Pzs8N6S.png)

By default, the browser address bar is set to readonly. To allow any link to be typed in the address bar, change the "Address bar readonly" setting from settings page to "True".

When in the main view, the browsers open the default/prefferred page. To change the prefferred page, navigate to it using one of the browsers and press the star button. The current loaded page will become the default for the selected browser.
![Browser default selection](https://i.imgur.com/nDnyIIL.png)

To display other images than the ones retrieved from "http://bloogum.net/guildwars", place images in the Screenshots folder, next to the Daybreak.exe executable. If the folder doesn't exist yet, either create it or run the launcher once so that it gets created automatically.

# Features
Automatically detect if guildwars is running or not. Includes the ability to launch guildwars from the client.

Manages username and password combination.

Ability to set a character name which gets autoloaded during launch.

Embedded browser set on useful pages or links.

Ability to set default page for each of the two browser windows.

Rotates screenshots from "Screenshots" folder. If no screenshots are present in the folder, downloads and rotates images from http://bloogum.net/guildwars (link to page is visible when showing images from the website).
