 //@req(session, appid, url, login, password, branch)

var params = {
   appId: appid,
   envName: "${env.appid}",
   session: session,
   type: "git",
   project: "ROOT",
   url: url,
   branch: branch,
   keyId: "WILL BE AUTODETECTED BELLOW",
   login: login,
   password: password,
   autoupdate: true,
   interval: 1,
   autoResolveConflict: true,
   zdt: false
}

var resp = jelastic.users.account.GetSSHKeys(appid, session, true);
if (resp.result != 0 || resp.keys == null) return resp;

if (resp.keys.length == 0) {
 return {
  result: 81,
  keyId: "private ssh key was not found, please follow the intstruction to add a new private ssh key https://docs.jelastic.com/ssh-add-key"
 }
}

//getting the first private key   
params.keyId = resp.keys[0].id;
resp = jelastic.env.vcs.CreateProject(params.envName, params.session, params.type, params.project, params.url, params.branch, params.keyId, params.login, params.password, params.autoupdate, params.interval, params.autoResolveConflict, params.zdt);
if (resp.result != 0) return resp;
resp = jelastic.env.vcs.Update(params.envName, params.session, params.project);
return resp;

/*
return {
   result : resp.result,
   onAfterReturn : {
      call : {
         procedure: 'log',
            params: {
            message: "put your message"
         }
      }
   }
}
*/
