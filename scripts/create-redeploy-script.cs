//@auth
//@req(url, next, targetEnv, nodeGroup)

import com.hivext.api.core.utils.Transport;
import com.hivext.api.utils.Random;

var buildEnv = "${env.envName}";

//reading script from URL
var scriptBody = new Transport().get(url)

//inject token
var token = Random.getPswd(64);
scriptBody = scriptBody.replace("${TOKEN}", token);

targetEnv = targetEnv.toString().split(".")[0];
scriptBody = scriptBody.replace("${TARGET_ENV}", targetEnv);
scriptBody = scriptBody.replace("${NODE_GROUP}", nodeGroup.toString());
scriptBody = scriptBody.replace("${BUILD_ENV}", buildEnv);
scriptBody = scriptBody.replace("${BUILD_ENV_APPID}", "${env.appid}");
scriptBody = scriptBody.replace("${UID}", user.uid.toString());

//getting master node     
var certified = true;
var resp = jelastic.env.control.GetEnvInfo(targetEnv, session);
if (resp.result != 0) return resp;
var nodes = resp.nodes;
var build = false;
for (var i = 0; i < nodes.length; i++) {
   if (nodes[i].nodeGroup == nodeGroup) {
       type = nodes[i].engineType || (nodes[i].activeEngine || {}).type
       certified = type ? true : false;
       if (type == "java") build = true;
       break;
   }
}

scriptBody = scriptBody.replace("${CERTIFIED}", certified.toString());
scriptBody = scriptBody.replace("${BUILD}", build.toString());

if (build) {
   
   //getting build node id and project id
   var nodeId = parseInt("${nodes.build.first.id}", 10);
   var projectId = parseInt("${nodes.build.first.customitem.projects[0].id}", 10);
   var projectName = "${nodes.build.first.customitem.projects[0].name}";

   if (isNaN(nodeId)) {
      var resp = jelastic.env.control.GetEnvInfo(buildEnv, session);
      if (resp.result != 0) return resp;
      var nodes = resp.nodes;   
      for (var i = 0; i < nodes.length; i++) {
          if (nodes[i].nodeGroup == "build") {
              nodeId = nodes[i].id;
              var projects = nodes[i].customitem.projects;
              if (projects) {
                 projectId = projects[0].id;
                 projectName = projects[0].name;
              }
              break;
          }
      }   
   }   
   
   scriptBody = scriptBody.replace("${BUILD_NODE_ID}", nodeId.toString());
   scriptBody = scriptBody.replace("${PROJECT_ID}", projectId.toString());
   scriptBody = scriptBody.replace("${PROJECT_NAME}", projectName.toString());
}

var scriptName = "${env.envName}-${globals.scriptName}"; 
//delete the script if it exists already
jelastic.dev.scripting.DeleteScript(scriptName);

//create a new script 
var resp = jelastic.dev.scripting.CreateScript(scriptName, 'js', scriptBody);
if (resp.result != 0) return resp;

//get app host
var host = 'https://' + window.location.host.replace('app.', 'cs.');

return {
    result: 0,
    onAfterReturn : {
        call : {
            procedure : next,
            params : {
                appid : appid,
                host : host,
                token : token
            }
        }
    }
}
