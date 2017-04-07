//@req(user, repo, token, callbackUrl, scriptName)
import org.apache.commons.httpclient.HttpClient;
import org.apache.commons.httpclient.methods.GetMethod;
import org.apache.commons.httpclient.methods.DeleteMethod;
import org.apache.commons.httpclient.methods.PostMethod;
import org.apache.commons.httpclient.UsernamePasswordCredentials;
import org.apache.commons.httpclient.methods.StringRequestEntity;
import org.apache.commons.httpclient.auth.AuthScope;
import com.hivext.api.core.utils.JSONUtils;
import java.io.InputStreamReader;
import java.io.BufferedReader;

var client = new HttpClient();

//Parsing repo url
var origRepo = repo;
var domain = "github.com";
if (repo.indexOf(".git") > -1) repo = repo.split(".git")[0];
if (repo.indexOf("/") > -1) {
    var arr = repo.split("/");
    repo = arr.pop();
    if (repo == "") repo = arr.pop();
    user = arr.pop();
    domain = arr.pop() || domain;
}

var IS_GITHUB = domain.indexOf("github.com") != -1;

//Authentication for GitHub
if (IS_GITHUB) {
    var creds = new UsernamePasswordCredentials(user, token);
    client.getParams().setAuthenticationPreemptive(true);
    client.getState().setCredentials(AuthScope.ANY, creds);
}

//Get list of hooks
var gitApiUrl = IS_GITHUB ? "https://api." + domain + "/repos/" + user + "/" + repo + "/hooks" : "https://" + domain + "/api/v4/projects/" + user + "%2F" + repo + "/hooks";
var get = new GetMethod(gitApiUrl);
//Authentication for GitLab
if (!IS_GITHUB) get.addRequestHeader("PRIVATE-TOKEN", token);

var resp = exec(get);
if (resp.result != 0) return resp;
var hooks = eval("(" + resp.response + ")");

//Clear previous hooks
for (var i = 0; i < hooks.length; i++) {
    var url = IS_GITHUB ? hooks[i].config.url : hooks[i].url;
    if (url.indexOf(scriptName) != -1) {
        var del = new DeleteMethod(gitApiUrl + "/" + hooks[i].id);
        //Authentication for GitLab
        if (!IS_GITHUB) del.addRequestHeader("PRIVATE-TOKEN", token);

        resp = exec(del);
        if (resp.result != 0 && resp.result != 204) return resp;
    }
}


var action = getParam('act');
if (action == 'delete' || action == 'clean') {
    return {
        result: 0
    };
}

//Create a new hook
var post = new PostMethod(gitApiUrl);


//Hook request params
var params = IS_GITHUB ? {
    "name": "web",
    "active": true,
    "events": ["push", "pull_request"],
    "config": {
        "url": callbackUrl,
        "content_type": "json"
    }
} : {
    "push_events": true,
    "merge_requests_events": true,
    "url": callbackUrl
};

//Authentication for GitLab
if (!IS_GITHUB) post.addRequestHeader("PRIVATE-TOKEN", token);

resp = exec(post, params);
if (resp.result != 0) return resp;
var newHook = eval("(" + resp.response + ")");

return {
    result: 0,
    hook: newHook
};

function exec(method, params) {
    if (params) {
        var requestEntity = new StringRequestEntity(JSONUtils.jsonStringify(params), "application/json", "UTF-8");
        method.setRequestEntity(requestEntity);
    }
    var status = client.executeMethod(method),
        response = "",
        result = 0,
        type = null,
        error = null;
    if (status == 200 || status == 201) {
        var br = new BufferedReader(new InputStreamReader(method.getResponseBodyAsStream())),
            line;
        while ((line = br.readLine()) != null) {
            response = response + line;
        }
    } else {
        error = "ERROR: " + method.getStatusLine();
        if (status == 401) error = "Wrong username or/and token. Please, double check your entries.";
        result = status;
        type = "error";
        response = null;
    }
    method.releaseConnection();
    return {
        result: result,
        response: response,
        type: type,
        message: error
    };
}
