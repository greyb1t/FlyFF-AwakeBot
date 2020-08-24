# Flyff Awakebot (now with CHAOTIC code!)
Flyff Awakebot is a software designed to do the awakening for you!

![alt text](https://i.epvpimg.com/1zZQg.png "Logo Title Text 1")

[Usage Video](https://www.youtube.com/watch?v=15anXFvMVNs "flyff bot video")

# Features!

  - Customizability - Support almost any server
  - User-friendly
  - Completely automated by simulated clicks
  - Open source
  
# How it works
  - Takes an image of the awake
  - Does some simple image processing on the image of the awake
  - Uses Tesseract OCR to convert the awake text of the image to stringified text
  - Parses the text and determines what awakes and values based on a config for the specified server

# Download

Visit the [elitepvpers thread](https://www.elitepvpers.com/forum/flyff-hacks-bots-cheats-exploits-macros/4139263-flyff-awakebot-customizable-support-your-own-server.html) for a binary download of the bot.

# Usage Instructions
1. Choose the correct config. If you server does not exist, make a config yourself by the guide down below. Then share it.

2. It's recommended to add a small awake delay of 0.5 seconds. Not needed if it ain't a laggy server.

3. Set all the item and scroll position by clicking the red buttons and dragging and releasing onto the specific item / scroll.

4. Click the last red button and a green transparent window will appear, drag and form a rectangle above the item awake text. It's needed in order for the bot to read the current awake. It's important that you also make the rectangle a little bigger incase a bigger awake appears.

5. Don't move the game window after you've set the positions and rectangle.

6. Add your preferred awake and value.

7. Click start and let it do it's job.

# Support your preferred server
1. Go into the config folder of the bot.

2. Make a new .xml file or make a copy of another already existing config and change it's name to the server you want to support.

3. Add or change the server's custom awakes in the new .xml file. Many servers have quite equal awakes, but some needs to be changed.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**Attribute Explanation:**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**name** = The name you'd like the awake type to be displayed inside of the bot.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**gametext** = The exact text of the specified awake inside of the game.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Awake Type Example:  
```xml
<Type name="ADOCH" gametext="Additional Damage of Critical Hits"></Type>
```
4. Change the value of "AwakeTextPixelColorRgb" to the color of the awake text inside of the game.
Format: R, G, B
```xml
<Setting name="AwakeTextPixelColorRgb">0, 255, 0</Setting>
```

![alt text](https://i.epvpimg.com/OOhSb.png "Awake color image")

5. Change the value of "ScrollDelayMs" to the time taken in milliseconds for your specific server to awake an item. (The water effect when clicking an item with awake scroll).

![alt text](https://i.epvpimg.com/yujEc.png "Awake water effect image")
```xml
<Setting name="ScrollDelayMs">200</Setting>
```
6. Change the value of "Language" to the awake text in-game language. If you have a different language other than english, download the langauge pack of choice at the end of the thread and put the files into tessdata.
```xml
<Setting name="Language">eng</Setting>
```
# Config Example
```xml
<!-- Awake Type Explanation:
	name -> The name to display inside of the bot when choosing the awake (Example: ADOCH)
	gametext -> Case-sensitive awake line text inside of the game (Example: Additional Damage of Critical Hits) -->
	
<Settings>
  <AwakeTypes>  
	<Type name="INT" gametext="INT"></Type>
	<Type name="DEX" gametext="DEX"></Type>
	<Type name="STR" gametext="STR"></Type>
	<Type name="STA" gametext="STA"></Type>
	<Type name="Critical Bonus" gametext="Critcal Bonus"></Type>
	<Type name="Increased Attack" gametext="Increased Attack"></Type>
	<Type name="Increased HP" gametext="Increased HP"></Type>
	<Type name="Increased MP" gametext="Increased MP"></Type>
	<Type name="Attack" gametext="Attack"></Type>
	<Type name="PvE Damage" gametext="PvE Damage Increase"></Type>
	<Type name="Increased DEF" gametext="Increased DEF"></Type>
	<Type name="DEF" gametext="DEF"></Type>
	<Type name="Critical Chance" gametext="Critical Chance"></Type>
	<Type name="Attack Speed" gametext="Attack Speed"></Type>
	<Type name="DCT" gametext="Decreased Casting Time"></Type>
	<Type name="EXP" gametext="EXP"></Type>
	<Type name="Speed" gametext="Speed"></Type>
	<Type name="Max HP" gametext="Max. HP"></Type>
	<Type name="Max MP" gametext="Max. MP"></Type>
	<Type name="Max FP" gametext="Max. FP"></Type>
  </AwakeTypes>

  <!-- The color of a pixel in the in-game awake line text. 
	   Format: RGB (R, G, B) -->
	   
  <Setting name="AwakeTextPixelColorRgb">0, 255, 0</Setting>
  
    <!-- The amount of time in milliseconds it takes before an awake scroll is done
  showing that "Watering Effect" on the item before awakening it -->
  
  <Setting name="ScrollDelayMs">200</Setting>
  <Setting name="Language">eng</Setting>
</Settings>
```

# FAQ
Q: My server is not showing up in the "Process Selector".  
A: Make sure the name of the game's client is "Neuz.exe". If not, change the name of the process in Settings.xml  
```xml
<Setting name="ProcessName">Neuz</Setting>
```

Q: The server is detecting it!  
A: Change the name of the folder containing the bot, the exe itself and the value of BotWindowName in Settings.xml  
```xml
<Setting name="BotWindowName">greyb1t's Flyff Awakebot</Setting>
```

Q: The server I'm playing on doesn't have scrolls. Only /awake command.  
A: Doesn't matter, just make the "Awake Pos" to the /awake command on the bottom bar in-game and "Reversion Pos" to an empty slot in the inventory.  

# Known problems and solutions
Error:
```
System.Reflection.TargetInvocationException: Exception has been thrown by the target of an invocation. --->
System.DllNotFoundException: Failed to find library "liblept172.dll" for platform x86 ...
```
**Solution:** Uninstall (probably not required) VC 2015 Redist x64 and install x86 instead.

# Requirements
.NET Framework 4.6.1  
VC 2015 Redist x86 (x64 will not work)  

# Changelog
**Version 1.01**
Fixed a bug with "Item Awake Read Rect" being unreachable.

**Version 1.02**
On e.g Magma Flyff, the time taken for a awake scroll to finish awakening the item is about 1 second. But on other servers, it's 200 ms.
Added a new option inside of the config to add a delay unique to that server of the time taken before an awake is done.

**Version 1.06**
Added possibilty to support non-english language servers.
Improved the accuracy of the bot and it's alot smarter.
The performance has been increased.
The bot is now basing it's interpretation on the server config and possible awakes.
A new setting has been added in the server config.

**Version 1.07**
Fixed memory leak.

**Version 1.08**
Hopefully fixed the OutOfMemoryException, turns out the error was not memory. It was caused by creating a bitmap out of bounds.

**Version 1.09**
Added an option to only click the "Awake Scroll Pos" once.
This adds the support for Augmentation Scroll and /awake command without wasting scrolls / money.

**Version 1.1**
Some small changes including adding a debug side bar to be able to identify possible issues with the bot not recognizing the awakes.
