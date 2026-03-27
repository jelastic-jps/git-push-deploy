//@req(token, action)
import com.hivext.api.server.system.service.utils.EnvironmentStatus;
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
        build = ${BUILD}, 
        context = "${CONTEXT}";    


    var resp = jelastic.env.control.GetEnvInfo(envName, session);
    if (resp.result != 0) return resp;
    var status = resp.env.status;
    var isEnvRunning = status == EnvironmentStatus['ENV_STATUS_TYPE_RUNNING'].getValue() ? true : false;

    if (!isEnvRunning) {
        if (jelastic.marketplace && jelastic.marketplace.console) {
            jelastic.marketplace.console.WriteLog(appid, session, "GIT-PUSH-DEPLOY: environment is not running");
        }
        return { result: 0 };
    }

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
                    //resp = jelastic.env.build.BuildProject(buildEnv, signature, nodeId, projectId);
                    var params = {
                        envName: buildEnv, 
                        session: signature, 
                        nodeid: nodeId, 
                        projectid: projectId, 
                        delay: delay
                    }
                    resp = jelastic.env.build.BuildDeployProject(params);
                } else {
                    var params = {
                        envName: targetEnv,
                        session: signature,
                        context: context,
                        delay: delay
                    }
                    resp = jelastic.env.vcs.Update(params);                    
                }
            } else {
                if (build) {
                    resp = jelastic.env.build.BuildProject(buildEnv, signature, nodeId, projectId);
                } else {
                    return {result: 99, error: 'deploy to non-certified containers is not implemented yet', type: 'warning'}
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
