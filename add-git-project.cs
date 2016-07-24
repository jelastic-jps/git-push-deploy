//@auth 
//@req(url, keyId)

var params = {
   envName: "${env.appid}",
   session: session,
   type: "git",
   context: "ROOT",
   url: url,
   branch: "master",
   keyId: keyId,
   login: "git",
   password: "",
   autoupdate: true,
   interval: 1,
   autoResolveConflict: true,
   zdt: false
}

var resp = jelastic.env.vcs.CreateProject(params.envName, params.session, params.type, params.context, params.url, params.branch, params.keyId, params.login, params.password, params.autoupdate, params.interval, params.autoResolveConflict, params.zdt);
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
