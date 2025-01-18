// Services/TotpService.cs

using OtpNet;
using System;

namespace Auth2FA.Services {
  public class TotpService {
    public string GenerateTOTPSecret() {
      var key = KeyGeneration.GenerateRandomKey(20); // 160-bit secret
      return Base32Encoding.ToString(key);
    }

    public string GenerateQRCodeUri(string secret, string username) {
      // This method can be used if you decide to implement QR codes in the future
      return $"otpauth://totp/MyApp:{username}?secret={secret}&issuer=MyApp";
    }

    public bool VerifyTOTP(string secret, string code) {
      var totp = new Totp(Base32Encoding.ToBytes(secret));
      return totp.VerifyTotp(code, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
    }
  }
}
