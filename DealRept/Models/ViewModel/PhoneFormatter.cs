using System;
using System.Text.RegularExpressions;

 public static class PhoneFormatter
{
    public static string FormatPhNumber(string phoneNum, string phoneFormat)
    {
        if (phoneFormat == "")
        {
            phoneFormat = "## ### ###-##-##";
        }
        Regex regex = new Regex(@"[^0-9]");
        phoneNum = regex.Replace(phoneNum, "");
        if (phoneNum.Length > 0)
        {
            phoneNum ="+"+ Convert.ToInt64(phoneNum).ToString(phoneFormat);
        }
        return phoneNum;
    }
   
}
