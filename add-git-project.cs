
//@auth

var params = {
   envName: "${env.domain}",
   session: session,
   type: "git",
   context: "test",
   url: "https://github.com/jelastic/HelloWorld.git",
   branch: "master",
   keyId: 2117,
   login: "git",
   autoupdate: true,
   interval: 1,
   autoResolveConflict: true,
   zdt: false
}

var resp = jelastic.env.vcs.CreateProject(params);

return {
    result : 0,
    respone: resp,
    onAfterReturn : {
        call : {
            procedure: 'log',
            params: {
                message: resp
            }
        }
    }
};
