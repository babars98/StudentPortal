namespace StudentPortal.BL
{
    public static class Utility
    {
        private static string IDPrefix = "C";
        private const int FirstId = 7736562;

        public static string GenerateId(int counter)
        {
            string id = $"{IDPrefix}{FirstId + counter}";
            return id;
        }
    }
}
