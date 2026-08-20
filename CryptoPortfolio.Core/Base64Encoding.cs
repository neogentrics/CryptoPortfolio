using System;
using System.Text;

/// <summary>
/// Implements Base64 encoding.
///
/// History:
/// Standardised in the Privacy-Enhanced Mail RFCs of the late 1980s and now defined by RFC 4648.
/// It exists because early email and network protocols were built for 7-bit text and would
/// corrupt or strip the high bit of arbitrary binary data. Base64 launders binary into a
/// 64-character alphabet that survives any such channel intact.
///
/// Purpose:
/// IMPORTANT - Base64 is an ENCODING, not encryption. It has no key, and decoding requires
/// nothing but knowledge of the scheme. It is included here precisely because it is so
/// frequently mistaken for encryption: seeing Base64 in a config file, a token or a network
/// capture and concluding the data is protected is one of the most common beginner errors in
/// security work, and a routine finding in real audits.
///
/// If you encounter Base64 while assessing a system, treat it as PLAINTEXT. It is worth
/// recognising on sight: length is always a multiple of four, the alphabet is A-Z a-z 0-9 plus
/// and slash, and it is padded with one or two equals signs.
/// </summary>
public static class Base64Encoding
{
    public static string Encode(string text) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(text ?? ""));

    public static string Decode(string base64)
    {
        try
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String((base64 ?? "").Trim()));
        }
        catch (FormatException)
        {
            return "Error: Input is not valid Base64.";
        }
    }
}
