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
        build = ${BUILD},
        context = "${CONTEXT}";

    function getRepoFromWebhook() {
        var r = getParam("repository.clone_url") || getParam("repository.git_url") ||
            getParam("repository.html_url") || getParam("project.git_http_url") ||
            getParam("repository.url");
        if (r) return r;
        var p = getParam("payload");
        if (p) {
            try {
                var j = typeof p == "string" ? JSON.parse(p) : p;
                var repo = j.repository || j.project;
                if (repo) return repo.clone_url || repo.git_url || repo.html_url || repo.git_http_url || repo.url;
            } catch (e) {}
        }
        return null;
    }
    function normalizeUrl(u) {
        return (u || "").replace(/\.git$/i, "").replace(/\/+$/, "").toLowerCase();
    }
    function findContextsByRepo(envName, session, nodeGroup, repoUrl) {
        var resp = jelastic.env.control.GetEnvInfo(envName, session);
        if (resp.result != 0 || !resp.nodeGroups) return [];
        var norm = normalizeUrl(repoUrl);
        var contexts = [];
        for (var g = 0; g < resp.nodeGroups.length; g++) {
            if (resp.nodeGroups[g].name != nodeGroup || !resp.nodeGroups[g].deployments) continue;
            var depl = resp.nodeGroups[g].deployments;
            for (var d = 0; d < depl.length; d++) {
                if (depl[d].type == "GIT" && depl[d].archivename &&
                    normalizeUrl(depl[d].archivename) == norm && depl[d].context) {
                    contexts.push(depl[d].context);
                }
            }
        }
        return contexts;
    }
    var repoUrl = getRepoFromWebhook();
    var contexts = repoUrl ? findContextsByRepo(envName, signature, nodeGroup, repoUrl) : [];
    if (!contexts.length) contexts = [context];

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
                    var lastResp;
                    for (var c = 0; c < contexts.length; c++) {
                        lastResp = jelastic.env.vcs.Update({
                            envName: targetEnv,
                            session: signature,
                            context: contexts[c],
                            delay: delay
                        });
                        if (lastResp.result != 0) return lastResp;
                    }
                    resp = lastResp || {result: 0};
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
