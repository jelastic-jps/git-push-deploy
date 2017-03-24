//@req(user, token, url, branch, type)

var params = {
   session: session,
   envName: "${env.envName}",
   type: "git",
   url: url,
   branch: branch,
   login: user,
   password: token,
   keyId: null,
   context: type == "ruby" ? "production" : "ROOT",
   autoupdate: true,
   interval: 1,
   autoResolveConflict: true,
   zdt: true
}

resp = jelastic.env.vcs.DeleteProject(params.envName, params.session, params.context);
if (resp.result != 0 && resp.result != 2500) return resp;

resp = jelastic.env.control.RemoveApp(params.envName, params.session, params.context);
if (resp.result != 0 && resp.result != 2313) return resp;

//create and update the project 
resp = jelastic.env.vcs.CreateProject(params.envName, params.session, params.type, params.context, params.url, params.branch, params.keyId, params.login, params.password, params.autoupdate, params.interval, params.autoResolveConflict, params.zdt);
if (resp.result != 0) return resp;

resp = jelastic.env.vcs.Update(params.envName, params.session, params.context);
return resp;
