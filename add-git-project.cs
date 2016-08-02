 //@req(session, url, keyId, login)

var params = {
   envName: "${env.appid}",
   session: session,
   type: "git",
   project: "ROOT",
   url: url,
   branch: "master",
   keyId: keyId,
   login: login,
   password: null,
   autoupdate: true,
   interval: 1,
   autoResolveConflict: true,
   zdt: false
}

var resp = jelastic.env.vcs.CreateProject(params.envName, params.session, params.type, params.project, params.url, params.branch, params.keyId, params.login, params.password, params.autoupdate, params.interval, params.autoResolveConflict, params.zdt);
if (resp.result == 0) resp = jelastic.env.vcs.Update(params.envName, params.session, params.project); 
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
