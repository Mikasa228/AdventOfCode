using System.Text.RegularExpressions;

namespace Common;

public static class MatchExtensions
{
    public static int GetIntValue(this Match match, string groupName)
    {
        var stringValue = match.Groups[groupName].Value;
        return int.Parse(stringValue);
    }
}
