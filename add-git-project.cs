 //@req(session, appid, url, login, password)

var params = {
   appId: appid,
   envName: "${env.appid}",
   session: session,
   type: "git",
   project: "ROOT",
   url: url,
   branch: "master",
   keyId: "",
   login: login,
   password: password,
   autoupdate: true,
   interval: 1,
   autoResolveConflict: true,
   zdt: false
}

var resp;
SshKeyResponse = jelastic.users.account.GetSSHKeys(appid, session, true);
if (SshKeyResponse.result != 0) return SshKeyResponse;
if (SshKeyResponse.keys != null && SshKeyResponse.keys.length > 0)
{
    for (i = 0; i < SshKeyResponse.keys.length; i++) {
        params.keyId = SshKeyResponse.keys[i].id;
        resp = jelastic.env.vcs.CreateProject(params.envName, params.session, params.type, params.project, params.url, params.branch, params.keyId, params.login, params.password, params.autoupdate, params.interval, params.autoResolveConflict, params.zdt);
        if (resp.result == 0)
        {
            resp = jelastic.env.vcs.Update(params.envName, params.session, params.project);
            if (resp.result == 0) break;
            else
                jelastic.env.vcs.DeleteProject(params.envName, params.session, params.project);
        }
    }  
    return resp;
}
else
return {
    result: 81,
    keyId: "Private SSH keys was not found"
};

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
