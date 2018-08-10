//@auth
//@req(name, url, branch, targetEnv, login, password)

//git repo url normalization
url = url.replace(/^\s+|\s+$/gm, '');
var ind = url.lastIndexOf(".git");
if (ind == -1 || ind != url.length - 4) {
   ind = url.lastIndexOf("/");
   if (ind > -1 && ind == url.length - 1) url = url.substring(0, ind);
   url += ".git";
}

var nodeId = parseInt("${nodes.build.first.id}", 10);
var projectId = parseInt("${nodes.build.first.customitem.projects[0].id}", 10);
var envName = "${env.envName}"; 

if (isNaN(nodeId)) {
   var resp = jelastic.env.control.GetEnvInfo(envName, session);
   if (resp.result != 0) return resp;
   var nodes = resp.nodes;   
   for (var i = 0; i < nodes.length; i++) {
       if (nodes[i].nodeGroup == "build") {
           nodeId = nodes[i].id;
           var projects = nodes[i].customitem.projects;
           if (projects) projectId = projects[0].id;
           break;
       }
   }   
}

targetEnv = targetEnv.toString().split(".")[0];
var params = {
   name: name,
   envName: envName,
   env: targetEnv,
   nodeId: nodeId,
   session: session,
   type: "git",
   context: "ROOT",
   url: url,
   branch: branch,
   keyId: null,
   login: login,
   password: password,
   autoupdate: false,
   interval: 1,
   autoResolveConflict: true
}

//remove the old project
if (!isNaN(projectId)) {
   var resp = jelastic.env.build.RemoveProject(params.envName, params.session, params.nodeId, projectId);
   if (resp.result != 0) return resp;
}

//add and build new project 
var resp = jelastic.env.build.AddProject(params.envName, params.session, params.nodeId, params.name, params.type, params.url, params.keyId, params.login, params.password, params.env, params.context, params.branch, params.autoupdate, params.interval, params.autoResolveConflict);
if (resp.result != 0) return resp;

//github triggers first build automatically after we add a webhook 
//calling build action manually for non-github repos
if (url.indexOf("github.com") == -1) {
   projectId = resp.id;
   var delay = getParam("delay") || 30;
   resp = jelastic.env.build.BuildDeployProject({
      envName: params.envName,
      session: params.session,
      nodeid: params.nodeId,
      projectid: projectId, 
      delay: delay
   });
}

return resp;
