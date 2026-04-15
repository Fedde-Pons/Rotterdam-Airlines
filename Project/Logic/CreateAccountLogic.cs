
// this handles creating a new account
public class CreateAccountLogic
{
    // this is how we talk to the database / file thing
    private AccountsAccess _access = new();


    public bool CreateAccount(AccountModel accountModel)    
    {
        _access.Write(accountModel);
        return true;
    }

    public bool ValidateFirstName(string firstName)
    {
        // Check if firstName contains any spaces
        if (firstName.Contains(" "))
        {
            return false;
        }

        // Check if firstName contains any numbers
        foreach (char c in firstName)
        {
            if (char.IsDigit(c))
            {
                return false;
            }
        }

        return true;
    }

    public bool ValidateLastName(string lastName)
    {
        // Check if lastName contains any numbers
        foreach (char c in lastName)
        {
            if (char.IsDigit(c))
            {
                return false;
            }
        }

        return true;
    }

    public bool ValidateEmailAddress(string emailAddress)
    {
        // Check if email contains @ and .
        if (!emailAddress.Contains("@") || !emailAddress.Contains("."))
        {
            return false;
        }

        // Find position of @ and .
        int atIndex = emailAddress.IndexOf("@");
        int dotIndex = emailAddress.LastIndexOf(".");

        // Must have text before @, @ must come before . , and . must have text after it
        if (atIndex == 0 || dotIndex <= atIndex + 1 || dotIndex == emailAddress.Length - 1)
        {
            return false;
        }

        // Check that @ appears only once
        if (emailAddress.IndexOf("@") != emailAddress.LastIndexOf("@"))
        {
            return false;
        }

        return true;
    }

    public bool ValidatePassword(string password)
    {
        // Check if password length is between 8 and 20 characters
        if (password.Length < 8 || password.Length > 20)
        {
            return false;
        }

        // Check if password contains no spaces
        if (password.Contains(" "))
        {
            return false;
        }

        bool hasUppercase = false;
        bool hasLowercase = false;
        bool hasNumber = false;
        bool hasSpecialChar = false;
        string specialChars = "!@#$%^&*";

        // Check each character
        foreach (char c in password)
        {
            if (char.IsUpper(c))
                hasUppercase = true;
            else if (char.IsLower(c))
                hasLowercase = true;
            else if (char.IsDigit(c))
                hasNumber = true;
            else if (specialChars.Contains(c))
                hasSpecialChar = true;
        }

        // All requirements must be met
        if (!hasUppercase || !hasLowercase || !hasNumber || !hasSpecialChar)
        {
            return false;
        }

        return true;
    }

    public bool ValidatePhoneNumber(string countryCode, string phoneNumber)
    {
        // Valid country codes (simplified list of country codes)
        // Format is typically 1-3 digits
        List<string> validCountryCodes = new List<string>
        {
            "1", "7", "20", "27", "30", "31", "32", "33", "34", "36", "39", "40", "41", "43", "44", "45",
            "46", "47", "48", "49", "51", "52", "53", "54", "55", "56", "57", "58", "60", "61", "62", "63",
            "64", "65", "66", "81", "82", "84", "86", "90", "91", "92", "93", "94", "95", "98", "212", "213",
            "216", "218", "220", "221", "222", "223", "224", "225", "226", "227", "228", "229", "230", "231",
            "232", "233", "234", "235", "236", "237", "238", "239", "242", "243", "244", "245", "246", "248",
            "249", "250", "251", "252", "253", "254", "255", "256", "257", "258", "260", "261", "262", "263",
            "264", "265", "266", "267", "268", "269", "290", "291", "297", "298", "299", "350", "351", "352",
            "353", "354", "355", "356", "357", "358", "359", "370", "371", "372", "373", "374", "375", "376",
            "377", "378", "380", "381", "382", "383", "385", "386", "387", "389", "420", "421", "423", "500",
            "501", "502", "503", "504", "505", "506", "507", "508", "509", "590", "591", "592", "593", "594",
            "595", "596", "597", "598", "599", "670", "672", "673", "674", "675", "676", "677", "678", "679",
            "680", "681", "682", "683", "684", "685", "686", "687", "688", "689", "690", "691", "692", "850",
            "852", "853", "855", "856", "880", "886", "960", "961", "962", "963", "964", "965", "966", "967",
            "968", "970", "971", "972", "973", "974", "975", "976", "977", "992", "993", "994", "995", "996", "998"
        };

        if (countryCode == "X" || phoneNumber == "X") {return true;}

        // Check if country code is valid
        if (!validCountryCodes.Contains(countryCode))
        {
            return false;
        }

        // Check if phone number contains only digits
        foreach (char c in phoneNumber)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }

        // Check if phone number has a reasonable length (typically 6-15 digits)
        if (phoneNumber.Length < 6 || phoneNumber.Length > 15)
        {
            return false;
        }

        return true;
    }

    public bool ValidateDateOfBirth(string dateOfBirth)
    {
        // Check format dd/mm/yyyy - must have exactly 2 slashes
        int slashCount = 0;
        foreach (char c in dateOfBirth)
        {
            if (c == '/')
                slashCount++;
        }

        if (slashCount != 2)
        {
            return false;
        }

        // Split by /
        string[] parts = dateOfBirth.Split('/');

        if (parts.Length != 3)
        {
            return false;
        }

        // Check each part is only digits
        foreach (string part in parts)
        {
            if (part.Length == 0)
                return false;

            foreach (char c in part)
            {
                if (!char.IsDigit(c))
                    return false;
            }
        }

        // Check format is dd/mm/yyyy (2 digits, 2 digits, 4 digits)
        if (parts[0].Length != 2 || parts[1].Length != 2 || parts[2].Length != 4)
        {
            return false;
        }

        int day = int.Parse(parts[0]);
        int month = int.Parse(parts[1]);
        int year = int.Parse(parts[2]);

        // Check if year is at least 1909
        if (year < 1909)
        {
            return false;
        }

        // Check if date is valid
        try
        {
            DateTime dob = new DateTime(year, month, day);
        }
        catch
        {
            return false;
        }

        return true;
    }
}

