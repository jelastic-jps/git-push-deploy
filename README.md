[![Git-Push-Deploy](https://jelastic.com/assets/img/jelastic-logo265.png)](../../../git-push-deploy)
## Jelastic Git-Push-Deploy Add-on 

This repository provides [Git-Push-Deploy](https://github.com/jelastic-jps/git-push-deploy) add-on for Jelastic Platform.

**Git-Push-Deploy** is an example of automated CI with git integration.

**Type of nodes this add-on can be applied to**: 
- Application server (cp)
- Apache 2 with Python engine.

### What it can be used for?

With a help of our bookmarklet, Git-Push-Deploy is installed on app server available in the environment to provide possibility to create/update the project in automatic mode from Git repository after changes pushed.

**Process workflow deploy**

[![Git-Push-Deploy](https://docs.google.com/drawings/d/1WHDD_uj96olWKjI2ukcxcBKlHNsL4YUifAfQ_WIE5fk/pub?w=589&h=148)](../../../git-push-deploy)

#### How to start using:

**How to get private ssh key id:**

- Go to Dev Dashboard
- Add your Private key following this instruction https://docs.jelastic.com/ssh-add-key
- Open Developer Tools -> Console 

<img src="https://raw.githubusercontent.com/siruslan/git-push-deploy/master/images/how-get-private-keyid.png" width="500">

- Execute the command below in the javascript console

```
JApp.Ajax.get("Management.Account.GetSSHKeys", {appid:GPlatform.APP_CLUSTER_APPID, isPrivate:true}, function(oResp) {console.log(Ext.each(oResp.keys, function(key) {console.log(key.title, key.id);} ))})
```
**How to set Private ssh key id:**
- Copy your keyId and past it in [manifest.jps](manifest.jps)

### Deployment

In order to get this solution instantly deployed, click the "Get It Hosted Now" button, specify your email address within the widget, choose one of the [Jelastic Public Cloud providers](https://jelastic.cloud) and press Install.

[![GET IT HOSTED](https://raw.githubusercontent.com/jelastic-jps/jpswiki/master/images/getithosted.png)](https://jelastic.com/install-application/?manifest=https%3A%2F%2Fgithub.com%2Fjelastic-jps%2Fgit-push-deploy%2Fraw%2Fmaster%2Fmanifest.jps)

To deploy this package to Jelastic Private Cloud, import [this JPS manifest](../../raw/master/manifest.jps) within your dashboard ([detailed instruction](https://docs.jelastic.com/environment-export-import#import)).

For more information on what Jelastic add-on is and how to apply it, follow the [Jelastic Add-ons](https://github.com/jelastic-jps/jpswiki/wiki/Jelastic-Addons) reference.