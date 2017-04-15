//@auth 
//@req(pathFrom, nodeId, nodeGroup)

var mountFrom = "${nodes.build.first.id}";
var envName = "${settings.targetEnv}".split(".")[0];
var mountTo = nodeGroup;

//getting app deployment directory 
resp = jelastic.env.control.ExecCmdById(envName, session, nodeId, toJSON([{
       "command": "echo ${WEBROOT:-$Webroot_Path}"
   }]) + "", true, "root");
if (resp.result != 0) return resp;

webroot = resp.responses[0].out;
if (!webroot) return {result: 99, type: "error", message: "$WEBROOT is not found"}

var resp = jelastic.env.file.RemoveMountPointByGroup(envName, session, mountTo, webroot);
if (resp.result != 0) return resp;

resp = jelastic.env.file.AddMountPointByGroup(envName, session, mountTo, webroot, 'nfs', null, pathFrom, mountFrom, 'auto-deploy-folder', false); 
return resp;
