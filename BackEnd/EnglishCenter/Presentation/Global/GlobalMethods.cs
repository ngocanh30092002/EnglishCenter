using System.Text;

namespace EnglishCenter.Presentation.Global
{
    public static class GlobalMethods
    {
        public static string GeneratePassword(int length)
        {
            Random random = new Random();
            string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            string specials = "!@#$%^&*()-_=+[]{}|;:,.<>?";
            string number = "0123456789";
            string allChars = upperCase + lowerCase + specials + number;

            if (length < 4)
                throw new ArgumentException("Password length should be at least 3 to include upper case, lower case, and special character.");

            var password = new StringBuilder();
            password.Append(upperCase[random.Next(upperCase.Length)]);
            password.Append(lowerCase[random.Next(lowerCase.Length)]);
            password.Append(specials[random.Next(specials.Length)]);
            password.Append(number[random.Next(number.Length)]);

            for (int i = 4; i < length; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            return new string(password.ToString().OrderBy(c => random.Next()).ToArray());
        }

    }
}
