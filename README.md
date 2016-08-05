## Jelastic Git-Push-Deploy Add-on 

[![Deploy](https://github.com/jelastic-jps/git-push-deploy/raw/master/images/deploy-to-jelastic.png)](https://jelastic.com/install-application/?manifest=https%3A%2F%2Fgithub.com%2Fjelastic-jps%2Fgit-push-deploy%2Fraw%2Fmaster%2Fmanifest.jps) 

This repository provides an example of automated CI with git integration.

### What it can be used for?

With a help of this JPS add-on, Git-Push-Deploy is installed on app server available in the environment to provide possibility to connect and update the project in automatic mode from Git repository after changes pushed.

[![Git-Push-Deploy](https://docs.google.com/drawings/d/1WHDD_uj96olWKjI2ukcxcBKlHNsL4YUifAfQ_WIE5fk/pub?w=528&h=322)](../../../git-push-deploy)

### How To Use

**Fork and customize**: 
- Update the link to private repository 
- Insert your private ssh key id (keyId) - see below 
- Modify "post-merge" hook for custom actions
- Change aplication server or entire topology 

**How to get private ssh key id:**

- Go to Dev Dashboard
- Add your Private key following this instruction https://docs.jelastic.com/ssh-add-key
- Open Developer Tools -> Console 

<img src="https://raw.githubusercontent.com/jelastic-jps/git-push-deploy/master/images/how-get-private-keyid.png" width="500">

- Execute the command below in the javascript console

```
JApp.Ajax.get("Management.Account.GetSSHKeys", {appid:GPlatform.APP_CLUSTER_APPID, isPrivate:true}, function(oResp) {console.log(Ext.each(oResp.keys, function(key) {console.log(key.title, key.id);} ))})
```
**How to set private ssh key id:**
- Copy your keyId and insert it in [manifest.jps](manifest.jps)


---

### Deployment

In order to get this solution instantly deployed, click the "Get It Hosted Now" button, specify your email address within the widget, choose one of the [Jelastic Public Cloud providers](https://jelastic.cloud) and press Install.

<a href="https://jelastic.com/install-application/?manifest=https%3A%2F%2Fgithub.com%2Fjelastic-jps%2Fgit-push-deploy%2Fraw%2Fmaster%2Fmanifest.jps" target="_blank"><img src="https://github.com/jelastic-jps/git-push-deploy/raw/master/images/deploy-to-jelastic.png" alt="Deploy" /></a>

To deploy this package to Jelastic Private Cloud, import [this JPS manifest](../../raw/master/manifest.jps) within your dashboard ([detailed instruction](https://docs.jelastic.com/environment-export-import#import)).

For more information on what Jelastic add-on is and how to apply it, follow the [Jelastic Add-ons](https://github.com/jelastic-jps/jpswiki/wiki/Jelastic-Addons) reference.
