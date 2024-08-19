public class TextFormatter
{
    public static string FormatText(float text)
    {
        if (text >= 1000)
        {
            int unitIndex = -1;
            float displayText = text;

            while (displayText >= 1000 && unitIndex < 25)
            {
                displayText /= 1000f;
                unitIndex++;
            }

            char unitChar = (char)('A' + unitIndex);
            return displayText % 1 == 0 ? $"{(int)displayText}{unitChar}" : $"{displayText:F1}{unitChar}";
        }
        else
        {
            return text % 1 == 0 ? $"{(int)text}" : $"{text:F1}";
        }
    }

    public static string FormatText(double text)
    {
        if (text >= 1000)
        {
            int unitIndex = -1;
            double displayText = text;

            while (displayText >= 1000 && unitIndex < 25)
            {
                displayText /= 1000f;
                unitIndex++;
            }

            char unitChar = (char)('A' + unitIndex);
            return displayText % 1 == 0 ? $"{(int)displayText}{unitChar}" : $"{displayText:F1}{unitChar}";
        }
        else
        {
            return text % 1 == 0 ? $"{(int)text}" : $"{text:F1}";
        }
    }
}
