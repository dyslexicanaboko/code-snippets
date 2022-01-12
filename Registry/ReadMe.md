# General comment
Just be aware that restarting your machine or application can potentially undo these fixes.

# Outlook auto complete
- Outlook auto complete.bat
- Outlook auto complete.reg

Use when Outlook's autocomplete for looking up contacts on the To line is disabled by your organization. Just be aware that you may have to run this every time you reopen Outlook or restart your computer depending on your organization's settings.

# Registry Excel TFS plugin.reg
Sometimes when Windows, MS Office and/or VisualStudio has a major update the TFS plugin (AzureDevOps) can get knocked out. This registry fix will bring it back so that you can re-enable it in Excel.

# Registry Google Chrome.reg
When Google Chrome sync is disabled by your organization you can just override the organization's settings directly in the registry.

# Registry MMC restricted run set to zero.reg
If you are trying to use "Active Directory Users and Computers" some organizations disable this by default and you get an error when running the Snap-In. This fix will allow you to run the Snap-In again.
