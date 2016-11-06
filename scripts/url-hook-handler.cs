//@req(token)

if (token == "${TOKEN}") {
  resp = jelastic.env.vcs.Update("${ENV_NAME}", signature, "${PROJECT}");
} else {
  return {"result": 8, "error": "wrong token"}
}
