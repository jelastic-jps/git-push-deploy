//@req(token, action)
if (token == "${TOKEN}") {
    var targetEnv = "${TARGET_ENV}",
        nodeGroup = "${NODE_GROUP}", 
        buildNodeId = "${BUILD_NODE_ID}",
        envName = "${BUILD_ENV}",
        envAppid = "${BUILD_ENV_APPID}",
        projectName = "${PROJECT_NAME}",
        delay = getParam("delay") || 30,
        UID = ${UID},
        certified = ${CERTIFIED},
        build = ${BUILD};

    if (action == 'redeploy') {
        if (certified) {
            return {result:99, error: 'redeploy action triggers automatically for certified containers', type: 'warning'};
        } else {
            return jelastic.env.control.RestartContainersByGroup(targetEnv, signature, nodeGroup, delay);
        }
    } else if (action == 'rebuild') {
        var buildEnv = "${BUILD_ENV}",
            nodeId = "${BUILD_NODE_ID}",
            projectId = "${PROJECT_ID}", 
            resp;
            if (certified) {
                if (build){                    
                    var module = "/usr/lib/jelastic/modules/maven.module";
                    var host = window.location.host.replace(/cs|app/, "core");
                    var cmd = ['url="https://' + host + '/JElastic/environment/build/rest/builddeploy?envName=\\${ENVIRONMENT}&projectName=\\${PROJECT_NAME}"', 
                               'cmd="parseArguments \\"\\$@\\"; [ \\${SESSION:0:4} = \'lds:\' ] && { readProjectConfig; echo \\$(curl -fsSL \\"$url\\"); writeJSONResponseOut \\"result=>0\\" \\"message=>redirect->build+deploy\\"; return 0; }"', 
                               'sed -i "/SESSION:0:/d" ' + module, 'sed -i "/doBuild()/a  $cmd" ' + module
                              ];
                  
                    var resp = jelastic.env.control.ExecCmdById(buildEnv, signature, nodeId, toJSON([{
                        "command": cmd.join("\n")
                    }]) + "", true, "root");
                    if (resp.result != 0) return resp;
                    
                    resp = jelastic.env.build.BuildProject(buildEnv, signature, nodeId, projectId);
                    //resp = jelastic.env.build.BuildDeployProject(buildEnv, signature, nodeId, projectId, delay);
                } else {
                    resp = jelastic.env.vcs.Update(targetEnv, signature, 'ROOT', delay);
                }
            } else {
                if (build) {
                    resp = jelastic.env.build.BuildProject(buildEnv, signature, nodeId, projectId);
                } else {
                    return {result: 99, error: 'deploy to non-certified containers is implemented yet', type: 'warning'}
                }
            }
        return resp; 
    } else {
        return {
            "result": 3,
            "error": "unknown action [" + action + "]"
        }
    }
} else {
    return {
        "result": 8,
        "error": "wrong token"
    }
}
