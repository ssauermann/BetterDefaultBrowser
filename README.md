#Better Default Browser - BDB#
Do you hate it when all **links** are opened in the same browser?
Do you want some links to open in a **specific browser** or just use the **currently running one**?

Get your control back with simple filter lists with **Better Default Browser**!
#Installation#
##Requirements##
- **Windows 7, 8, 8.1 or 10** *(Vista is untested but might work)*
- **.NET Framework 4.5.2** or later


 
##Getting the Program##
Download the latest release **[here](http://github.com/ssauermann/betterdefaultbrowser "Release Version")**.

Alternatively you can download the latest source [here]() and build it yourself.



##First time setup##
1. Move the program to a **permanent** location!
2. Run `BetterDefaultBrowser.exe`
3. Install the BDB to the available system browsers via the menu. (*This requires admin privileges*)
4. Set BDB as the system default browser so all clicked links will be redirected to BDB.
5. Set another browser (**not BDB**) as the BDB default browser for unmatched urls.

Now the configuration is completed and all links outside of other browsers should be openend with your BDB default browser.

#Usage#
##Configuring filters##
- To add a filter select a filter type and press the **+** button.
- Select a filter in the list and press the ➔ button to edit it.
- Select a filter in the list and press the **–** button to delete it.
- Order the priority of your filters via the ↑ and ↓ arrows. The filter list will be applied top down and the first matching filter will be used.

## Filter Types ##
#### Settings for all filters ####
- **Name** - Set a name for display in the list

#### Managed Filter ####
This is a standard filter mapping an url to a single browser.
- **Browser** - Select a browser to open when this filter does match an url. (**Do not select BDB**)
- **Protocols** - Select the protocols this filter should match
- **Ignore** - Ignore parts of the url when matching
- **Website** - Enter an valid url that will be matched considering all other options in this filter (This has to be a valid url)

#### Open Filter ####
This filter maps an url to a currently opened browser.
- **Browsers** - Select a priority list of browsers to use when this filter does match an url. The first currently running browser of this list will be used. (**Do not select BDB**)
- **Only running** - Do not start a new browser when no selected browser is running. The filter will be skipped then. If this is not set, the browser of the inner type will be started if no selected browser is currently running. 
- **Underlying filter** - Select the type of the inner filter. The inner filter will be used for the matching. Press `Next` to configure it.


#### Plain Filter (Expert)####
This is a standard filter mapping an url to a single browser.
- **Browser** - Select a browser to open when this filter does match an url. (**Do not select BDB**)
- **Regex** - Enter a valid regular expression for the url matching.



# Help #
This is an alpha version so it may crash. If the program crashes you can find a log file in **`%appdata%\Better Default Browser`**. Please report crashes via the **[Issue Tracker](https://github.com/ssauermann/BetterDefaultBrowser/issues "Issue tracker")** and provide this log file in addition to a short explanation what you tried to do.

If BDB doesn't start up any more you can delete all settings (and the log file) via the menu (`File -> Delete Settings`) and beginn from scratch.


#FAQ#
###I've moved the programs folder and BDB doesn't work anymore!###
Don't panic, just uninstall and reinstall BDB via the menu.


#Uninstallation#
1. Set your system default browser to another browser.
2. Uninstall BDB via the menu. (If you can't open the main program and want to remove BDB, run `BetterDefaultBrowser-Helper.exe -uninstall` from an eleveated command promt)
3. Optional: Delete your settings and all log files via the menu.
4. Now you can safely delete the program files.