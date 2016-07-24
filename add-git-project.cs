
//@auth


var params = {
   envName: "${env.appid}",
   session: session,
   type: "git",
   context: "ROOT",
   url: "https://github.com/jelastic/HelloWorld.git",
   branch: "master",
   keyId: 2117,
   login: "git",
   password: "",
   autoupdate: true,
   interval: 1,
   autoResolveConflict: true,
   zdt: false
}


var resp = jelastic.env.vcs.CreateProject(
   params.envName,
   params.session,
   params.type,
   params.context,
   params.url,
   params.branch,
   params.keyId,
   params.login,
   params.password,
   params.autoupdate,
   params.interval,
   params.autoResolveConflict,
   params.zdt
);


var all = "resp="+resp + "   \n";
for (var m in resp) {
   all += m + "=" + resp[m] + "   \n"; 
}


return {
    result : 0,
    respone: "aa",
    onAfterReturn : {
        call : [
            {
                procedure: 'log',
                params: {
                    message: all
                }
            },
            {
                procedure: 'log',
                params: {
                    message: "e"
                }
            }
        ]
    }
};
