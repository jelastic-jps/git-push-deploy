//@req(json)

import javax.crypto.spec.SecretKeySpec;
import javax.crypto.Mac;

var hubSignature = window.location.headers["X-Hub-Signature"];
if (hubSignature == "sha1=" + hmacSha1(json, token)) {
    return jelastic.env.vcs.Update("env-0448488", signature, "ROOT");
} else {
    return {
        "result": 8,
        "error": "verification X-Hub-Signature error"
    }
}

function hmacSha1(value, key) {

    // Get an hmac_sha1 key from the raw key bytes
    var keyBytes = new java.lang.String(key).getBytes();
    var signingKey = new SecretKeySpec(keyBytes, "HmacSHA1");

    // Get an hmac_sha1 Mac instance and initialize with the signing key
    var mac = Mac.getInstance("HmacSHA1");
    mac.init(signingKey);

    // Compute the hmac on input data bytes
    var rawHmac = mac.doFinal(new java.lang.String(value).getBytes());

    // Convert raw bytes to Hex
    var hexBytes = new org.apache.commons.codec.binary.Hex().encode(rawHmac);

    //  Covert array of Hex bytes to a String
    return new java.lang.String(hexBytes, "UTF-8");

}
