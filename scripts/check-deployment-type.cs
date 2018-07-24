//@req(next)

envName = '${env.envName}'
nodes = jelastic.env.control.GetEnvInfo(envName, session).nodes
addon = 'unknown'

for (i = 0; i < nodes.length; i++){
  if (nodes[i].nodeGroup == 'cp') {
    type = nodes[i].engineType || (nodes[i].activeEngine || {}).type;
    addon = type ? (type == 'java' ? 'maven' : 'vcs') : 'mount'
      
    if (addon == 'mount') return {result:99, error: 'deploy to custom containers is not implemented yet', type: 'warning'}
    if (addon == 'maven') envName += '-git-push-${fn.random(1000)}'
    
    resp = {result: 0, onAfterReturn: []}
    o = {}, o[next] = {addon: addon, type: type, envName: envName}
    resp.onAfterReturn.push(o)

    return resp
  }
}
return {result: 99, error: 'nodeGroup [cp] is not present in the topology', type: 'warning'}
