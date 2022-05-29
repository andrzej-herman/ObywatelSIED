namespace Client.PeselLogic;

public class Citizen
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Sex { get; set; }
    public string PESEL { get; set; }
    public string Names => $"{FirstName} {SecondName}";

    public string DateOfBirthString
    {
        get { return DateOfBirth.ToString("dd.MM.yyyy"); }
    }


    public void GetPESEL(int currentIndex)
    {
        var pesel = $"{GetCodedDateOfBirth()}{GetPeselCodeNumber(currentIndex)}";
        PESEL = $"{GetCodedDateOfBirth()}{GetPeselCodeNumber(currentIndex)}{GetControlNumber(pesel)}";
    }

    private string GetCodedDateOfBirth()
    {
        var yearPart = DateOfBirth.Year.ToString().Substring(2, 2);
        var dayPart = DateOfBirth.Day.ToString("00");
        var monthPart = DateOfBirth.Year switch
        {
            >= 1900 and <= 1999 => DateOfBirth.Month.ToString("00"),
            >= 2000 and <= 2099 => (DateOfBirth.Month + 20).ToString(),
            _ => null
        };

        return monthPart != null ? $"{yearPart}{monthPart}{dayPart}" : null;
    }

    private string GetPeselCodeNumber(int currentIndex)
    {
        var codePart = (currentIndex + 1).ToString("000");
        return $"{codePart}{SexNumber()}";
    }
    
    private string SexNumber()
    {
        var rnd = new Random();
        var number = rnd.Next(1, 10);
        if (Sex == "female")
        {
            while(number % 2 != 0)
                number = rnd.Next(1, 10);
        }
        else
        {
            while(number % 2 == 0)
                number = rnd.Next(1, 10);
        }
        
        return number.ToString();
    }

    private string GetControlNumber(string currentPESEL)
    {
        var wages = new List<int>() { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
        var values = new List<int>();
        for (var idx = 0; idx < 10; idx++)
        {
             values.Add(int.Parse(currentPESEL[idx].ToString()) * wages[idx]);
        }

        var digits = values.Select(item => item.ToString().Length == 1 ? item 
            : int.Parse(item.ToString().Substring(1, 1))).ToList();

        var sum = digits.Sum();
        return sum.ToString().Length == 1 
            ? (10 - sum).ToString() 
            : (10 - (int.Parse(sum.ToString().Substring(1, 1)))).ToString();
    }
}