
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

var all = "";
for (var m in jelastic.env.vcs.CreateProject) {
   all += m + "=" + jelastic.env.vcs.CreateProject[m] + "   \n"; 
}
//var method = jelastic.env.vcs.CreateProject + "";

var resp = "aa";// jelastic.env.vcs.CreateProject(params);



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
                    message: "bb"
                }
            }
        ]
    }
};
