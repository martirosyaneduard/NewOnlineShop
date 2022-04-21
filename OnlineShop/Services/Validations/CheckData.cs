namespace OnlineShop.Services.Validations
{
    public static class CheckData
    {
        public static void CheckPositiveNumber(double number, string name)
        {
            if (number <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}
