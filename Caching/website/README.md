# Foundation.Caching

This module is designed to expand Sitecore's caching capability to add a timeout parameter. By inserting a value, the rendering will only expire its cache at the end of the specified time, not on a content publish.

## Installation
Add the module code to your solution (either importing the files or as a Helix module). Run the included Sitecore package to add the timeout field; you can then incorporate the field into your source control using your preferred tool (TDS, Unicorn, CLI).

### Notes
* This module is configured with the Sitecore 10 `Sitecore.Mvc` reference. You can change this as appropriate for your solution.
* The caching site definition is preset with the `preventHtmlCacheClear` attribute; if you use this module with a 9.2 or lower site, you do not have to do anything extra, as the site will not be included in the publish:end cache clearing.

## More Information

See this [blog post](https://www.google.com) for more information about the code in this module.
