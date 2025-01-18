namespace Auth2FA.Services {
  using OtpNet;
  using System;
  public class TotpService {
    public string GenerateTOTPSecret() {
      var key = KeyGeneration.GenerateRandomKey(20);
      return Base32Encoding.ToString(key);
    }
    public string GenerateQRCodeUri(string secret, string username) {
      return $"otpauth://totp/MyApp:{username}?secret={secret}&issuer=MyApp";
    }

    public bool VerifyTOTP(string secret, string code) {
      var totp = new Totp(Base32Encoding.ToBytes(secret));
      return totp.VerifyTotp(code, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
    }
  }

}
