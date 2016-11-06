//@auth
//@req(baseUrl, envName, project, gitUser, token, repoUser, repoName)

import com.hivext.api.core.utils.Transport;
import com.hivext.api.utils.Random;

//reading script from URL
var url = baseUrl + "/scripts/url-hook-handler.cs";
var scriptBody = new Transport().get(url);

scriptBody = scriptBody.replace("${ENV_NAME}", envName + "");
scriptBody = scriptBody.replace("${PROJECT}", project + "");

var scriptName = envName + "-url-hook-handler";

//delete the script if it exists already
var resp = jelastic.dev.scripting.DeleteScript(scriptName);
if (resp.result != 0 && resp.result != 1702) return resp;

//create a new script 
resp = jelastic.dev.scripting.CreateScript(scriptName, "js", scriptBody);
if (resp.result != 0) return resp;

//get app domain
var domain = jelastic.dev.apps.GetApp(appid).hosting.domain;

//add web hook to GitHub via API
url = baseUrl + "/scripts/github-api-add-hook.cs";
scriptBody = new Transport().get(url);

//get app hook domain
var domain = jelastic.dev.apps.GetApp(appid).hosting.domain;
var hookurl = "http://" + domain +"/"+scriptName;

resp = jelastic.dev.scripting.EvalCode(scriptBody, "js", null, {
  gitUser: gitUser, 
  repoUser: repoUser,
  repoName: repoName, 
  token: token, 
  url: hookurl,
  secret: Random.getPswd(64)
});
if (resp.result != 0) return resp;
return resp.response;
