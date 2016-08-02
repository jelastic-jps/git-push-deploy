# Git-Push-Deploy
Example of automated CI with git integration 

### How to get private ssh key id (temporary workaround)

```
JApp.Ajax.get("Management.Account.GetSSHKeys", {appid:GPlatform.APP_CLUSTER_APPID, isPrivate:true}, function(oResp) {console.log(Ext.each(oResp.keys, function(key) {console.log(key.title, key.id);} ))})

```


