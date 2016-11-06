//@auth
//@req(jpsRepo, projectRepo, branch, token)

var baseUrl = jpsRepo;
var repo = projectRepo;

return {resul: -1, response: "a = " + repo + " base = " +  baseUrl}
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
var user = arr.pop();

//create and update the project 
resp = jelastic.env.vcs.CreateProject(params.envName, params.session, params.type, params.project, params.url, params.branch, params.keyId, params.login, params.password, params.autoupdate, params.interval, params.autoResolveConflict, params.zdt);
if (resp.result != 0) return resp;
resp = jelastic.env.vcs.Update(params.envName, params.session, params.project);
if (resp.result != 0) return resp;

//add web hook to GitHub via API
url = baseUrl + "/scripts/create-hook-and-handler.cs";
scriptBody = new Transport().get(url);

return jelastic.dev.scripting.EvalCode(scriptBody, "js", null, {
  user: user, 
  repo: repo, 
  token: token, 
  url: hookurl,
  baseUrl: baseUrl, 
  project: params.project, 
  user: user, 
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
