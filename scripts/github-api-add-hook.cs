//@req(gitUser, repo, token, url)

import org.apache.commons.httpclient.HttpClient;
import org.apache.commons.httpclient.methods.PostMethod;
import org.apache.commons.httpclient.UsernamePasswordCredentials;
import org.apache.commons.httpclient.methods.StringRequestEntity;
import org.apache.commons.httpclient.auth.AuthScope;
import com.hivext.api.core.utils.JSONUtils;
import java.io.InputStreamReader;
import java.io.BufferedReader;

var client = new HttpClient();

//Authentication
var creds = new UsernamePasswordCredentials(gitUser, token);
client.getParams().setAuthenticationPreemptive(true);
client.getState().setCredentials(AuthScope.ANY, creds);

//api post request
var api = "https://api.github.com/repos/"+gitUser+"/"+repo+"/hooks";
var post = new PostMethod(api);

//hook configs
var params = {
    "name": "web",
    "active": true,
    "events": ["push", "pull_request"],
    "config": {
        "url": url,
        "content_type": "json"
    }
};

var requestEntity = new StringRequestEntity(JSONUtils.jsonStringify(params), "application/json", "UTF-8");
post.setRequestEntity(requestEntity);

var status = client.executeMethod(post), 
    br = new BufferedReader(new InputStreamReader(post.getResponseBodyAsStream())),
    response = "",
    line;

while ((line = br.readLine()) != null) {
    response = response + line;
}

post.releaseConnection();

resp = {result: 0,  response: {"type": typeof JSONUtils.toJSON(response), "json": JSONUtils.toJSON(response).message } };
resp.onAfterReturn = {
      call : {
         procedure: 'log',
            params: {
               message: "git api - gitUser = " + gitUser + " token = " + token
         }
      }
   }

return resp;

