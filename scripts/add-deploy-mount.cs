//@auth 
//@req(pathFrom, pathTo)

var mountFrom = "${nodes.build.first.id}";
var envName = "${settings.targetEnv}".split(".")[0];
var mountTo = "cp";
//var groups = jelastic.env.control.GetEnvInfo(envName, session).nodeGroups; 
/*for (var i = 0; i < groups.length; i++){
  if (groups[i].name == "storage") {
    mountTo = "storage";
    break;
  }
}*/

var resp = jelastic.env.file.RemoveMountPointByGroup(envName, session, mountTo, pathTo);
if (resp.result != 0) return resp;

//resp = jelastic.env.control.AddDockerVolumeByGroup('${env.envName}', session, mountTo, volume); 
resp = jelastic.env.file.AddMountPointByGroup(envName, session, mountTo, pathTo, 'nfs', null, pathFrom, mountFrom, 'auto-deploy-folder', false); 
return resp;
