# WSJT-X DX Alerter

![Project Status: Inactive – The project has reached a stable, usable state but is no longer being actively developed; support/maintenance will be provided as time allows.](http://www.repostatus.org/badges/latest/inactive.svg)
[![lifecycle](https://img.shields.io/badge/lifecycle-stable-green.svg)](https://www.tidyverse.org/lifecycle/#stable)

## Description

I got my FCC Amateur Radio Operator (Ham Radio) license several years ago. I eventually started using the digital modes JT-65 and FT-8 using Joe Taylor's great app, WSJT-X, a couple years ago because theses modes can grab up communications from really noisy, dirty frequencies which is an issue these days. The problem with these modes is that if you are hunting new countries to work you have to camp on the interface program and eyeball filter the traffic in search of a new callsign/country. This can tie up hours and hours of your time.

So, I sat down and wrote a program to monitor the traffic decoded by WSJT-X and look for callsigns I have not worked and then see if they are from a DX (long distance/foreign) station located in a country or entity in the DXCC entities list I have not yet worked. If it is a new one it sounds an alert and optionally sends an email notice as well. This way I can work on my software or hardware projects in an adjacent room and not miss a chance at a new DX entity to add to the list.

The usual way to use it is to launch WSJT-X and get it going, then launch the alerter and hit the "Start" button to have it start reading the activity log of WSJT-X. Because the alerter has to time-share on the activity log there is a sync method to synchronize and stay out of the way of WSJT-X. Make sure you click the correct choice of digital mode as that determines the internal timer elapsed time setting. If the alerter determines it is colliding with WSJT-X it will resync all on its own.

**a note about "entities"** 

Entities are countries and also remote parts of countries, like the Canary Islands, Alaska, Puerto Rico, that are some distance away from the main landmass of the country. Also, Russia was divided into European and Asian Russia due to it's size. There are over 300 entities in the current DX entity list. I placed a copy of it in text file format into the project folder under source code.


## The Project

I originally wrote this program to read the \<your callsign\>.log file of XMLog contact logging application when it starts up. In this newer version (V2.0) I added the ability to also read a comma delimited data file (##.csv). If you use some other logging app I'm sure it can export your contacts. If you can, select the columns you just want - callsign, country, QSL(card)received, and LotW received. Otherwise export the data and use a spreadsheet program to drop the columns you don't need and then reexport it as a .csv file. Those last two columns are then used to mark the country as already contacted and verified, so the alerter will disregard any 'new' contacts from that country. You can choose to still get alerts from an entity if no one from there has validated the contact via a QSL card or Logbook of the World.

This app was compiled for .Net 4.7.2 and "any cpu". You can always download the source and recompile with whichever options you prefer. I have chopped off several of the folders below the source code files to reduce the download time. All you have to do is recompile and Visual Studio (2017) will recreate them as they are working files and the 'bin' file tree for the Debug and Release compiled versions.

I am uploading both the source files and the compiled application. That way you can test drive it and see if it looks like something you'd like to use and/or dive into the code. Either way, ENJOY!

**Email Logon Info Encryption**

This app uses encrypted email information in the Properties document - WSJTX DX Alerter.exe.config, because it is otherwise in plain text so someone could hack your email account and abuse it. If you just want to run the WSJT-X app you need to put your email information in there after encrypting it. The items are: user ID, user password, email server address, address of destination, address of sender. Find that document and open it with a plain text editor like Notepad. Use a TripleDES encrypter tool to create the encrypted values and edit them into the document and save it. If you are going to play with the source code, edit the encrypted values into the Properties::Settings in the source. The email port number is in clear text. If your server requires another port then edit that into the Properties document too.

I have uploaded my encrypter app to github in the project Chucks_Encrypter-Decrypter. It is lightweight and simple to use. 

## License

This software is licensed under the GNU General Public License Version 3 (and newer if published).
