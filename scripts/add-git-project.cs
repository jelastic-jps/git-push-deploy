//@auth
//@req(jpsRepo, projectRepo, branch, token)

import com.hivext.api.core.utils.Transport;

var baseUrl = jpsRepo;
var repo = projectRepo;

if (repo.indexOf(".git") == repo.length - 4) repo += ".git";

var params = {
   appId: appid,
   envName: "${env.envName}",
   session: session,
   type: "git",
   project: "ROOT",
   url: repo,
   branch: branch,
   keyId: null,
   login: null,
   password: null,
   autoupdate: true,
   interval: 1,
   autoResolveConflict: true,
   zdt: false
}
var arr = repo.split("/");
var repo = arr.pop().split(".").shift(); 
var gitUser = arr.pop();

//Remove previous version 
resp = jelastic.env.vcs.DeleteProject(params.envName, params.session, params.project);
if (resp.result != 0) return resp;

//create and update the project 
resp = jelastic.env.vcs.CreateProject(params.envName, params.session, params.type, params.project, params.url, params.branch, params.keyId, params.login, params.password, params.autoupdate, params.interval, params.autoResolveConflict, params.zdt);
if (resp.result != 0) return resp;

//pull update
resp = jelastic.env.vcs.Update(params.envName, params.session, params.project);
if (resp.result != 0) return resp;

//add web hook to GitHub via API
url = baseUrl + "/scripts/create-hook-and-handler.cs";
scriptBody = new Transport().get(url);

return jelastic.dev.scripting.EvalCode(scriptBody, "js", null, {
  envName: "${env.envName}",
  token: token, 
  baseUrl: baseUrl, 
  project: params.project, 
  gitUser: gitUser, 
  repo: repo, 
  token: token
});
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
