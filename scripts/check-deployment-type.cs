//@req(next)

nodes = jelastic.env.control.GetEnvInfo('${env.envName}', session).nodes
addon = 'unknown'
for (i = 0; i < nodes.length; i++){
  if (nodes[i].nodeGroup == 'cp') {
    engine = nodes[i].activeEngine
    addon = engine ? (engine.type == 'java' ? 'maven' : 'vcs') : 'mount'
    resp = {
      result: 0, 
      onAfterReturn: {}
    }
    resp.onAfterReturn[next] = {addon: addon}
    return resp
  }
}
return {result: 99, error: 'nodeGroup [cp] is not present in the topology', type: 'warning'}

