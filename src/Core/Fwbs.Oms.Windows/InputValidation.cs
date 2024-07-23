namespace FWBS.OMS.UI
{
    public static class InputValidation
    {
        public static bool ValidateOpenFileInput(string input)
        {
            return input.StartsWith("OPEN ");
        }

        public static bool ValidatePrintFileInput(string input)
        {
            return input.StartsWith("PRINT ");
        }

        public static bool ValidateEmptyInput(string input)
        {
            return string.IsNullOrEmpty(input);
        }
    }
}