//@req(user, token, url, branch, type)

//git repo url normalization
url = url.replace(/^\s+|\s+$/gm, '');
var ind = url.lastIndexOf(".git");
if (ind == -1 || ind != url.length - 4) {
   ind = url.lastIndexOf("/");
   if (ind > -1 && ind == url.length - 1) url = url.substring(0, ind);
   url += ".git";
}

contexts = type == "ruby" ? ["development", "test", "production"] : ["ROOT"];
    
p = {
    session: session,
    envName: "${env.envName}",
    type: "git",
    url: url,
    branch: branch,
    login: user,
    password: token,
    keyId: null,
    context: contexts[0],
    autoupdate: true,
    interval: 1,
    autoResolveConflict: true,
    zdt: true
}

resp = jelastic.env.control.GetEnvInfo("${env.envName}", session);
if (resp && resp.result != 0) return resp;

if (resp.env && resp.env.contexts) {
   contextsExists = resp.env.contexts.length;
}
   
if (contextsExists) {
   //removing already deployed app/project in context
   //specifically for ruby -> removing all contexts (development, test and production), 
   //because ruby supports only one context at a time  
   for (i = 0; i < contexts.length; i++) {
       resp = jelastic.env.control.RemoveApp(p.envName, p.session, contexts[i]);
       if (resp.result != 0 && resp.result != 2313) return resp;

       resp = jelastic.env.vcs.DeleteProject(p.envName, p.session, contexts[i]);
       if (resp.result != 0 && resp.result != 2500) return resp;
   }
}

//create and update the project 
resp = jelastic.env.vcs.CreateProject(p.envName, p.session, p.type, p.context, p.url, p.branch, p.keyId, p.login, p.password, p.autoupdate, p.interval, p.autoResolveConflict, p.zdt);
if (resp.result != 0) {
   resp.params = p;
   return resp;
}

resp = jelastic.env.vcs.Update(p.envName, p.session, p.context);
return resp;
