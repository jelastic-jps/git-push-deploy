//@auth 
//@req(pathFrom)

var mountFrom = "${nodes.build.first.id}";
var envName = "${settings.targetEnv}".split(".")[0];
var mountTo = "cp";

var resp = jelastic.env.file.RemoveMountPointByGroup(envName, session, mountTo, pathTo);
if (resp.result != 0) return resp;

resp = jelastic.env.control.ExecCmdById(envName, session, "${nodes.build.cp.id}", toJSON([{
       "command": "echo ${WEBROOT:-$Webroot_Path}"
   }]) + "", true, "root");
if (resp.result != 0) return resp;

webroot = resp.responses[0].out;
if (!webroot) return {result: 99, type: "error", message: "$WEBROOT is not found"}

resp = jelastic.env.file.AddMountPointByGroup(envName, session, mountTo, webroot, 'nfs', null, pathFrom, mountFrom, 'auto-deploy-folder', false); 
return resp;
