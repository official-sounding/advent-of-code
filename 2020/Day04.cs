using System.Text.RegularExpressions;

[Slug(2020, 04)]
public class Day202004 : Problem
{
    public override long RunPartOne(string[] input)
    {
        var data = input.Select((line) =>
        {
            var split = line.Split(",");

            return new Passport()
            {
                Fields = split.Select(x =>
                {
                    var field = x.Split(":");
                    return (field[0], field[1]);
                }).ToList()
            };
        }).ToList();

        return data.Count(i => i.ValidPartOne);
    }

    public override long RunPartTwo(string[] input)
    {
        var data = input.Select((line) =>
        {
            var split = line.Split(",");

            return new Passport()
            {
                Fields = split.Select(x =>
                {
                    var field = x.Split(":");
                    return (field[0], field[1]);
                }).ToList()
            };
        }).ToList();

        return data.Count(i => i.ValidPartTwo);
    }
}

/*
 
    byr (Birth Year)
    iyr (Issue Year)
    eyr (Expiration Year)
    hgt (Height)
    hcl (Hair Color)
    ecl (Eye Color)
    pid (Passport ID)
    cid (Country ID)


    byr (Birth Year) - four digits; at least 1920 and at most 2002.
    iyr (Issue Year) - four digits; at least 2010 and at most 2020.
    eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
    hgt (Height) - a number followed by either cm or in:
        If cm, the number must be at least 150 and at most 193.
        If in, the number must be at least 59 and at most 76.
    hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
    ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
    pid (Passport ID) - a nine-digit number, including leading zeroes.
    cid (Country ID) - ignored, missing or not.

 */

public class Passport
{
    public required List<(string, string)> Fields { get; set; }

    public bool ValidPartOne => Fields.Count == 8 || Fields.Count == 7 && Fields.All(f => f.Item1 != "cid");
    public bool ValidPartTwo => ValidPartOne && Fields.All((f) =>
    {
        var (field, value) = f;
        return field switch
        {
            "byr" => int.TryParse(value, out int year) && year >= 1920 && year <= 2002,
            "iyr" => int.TryParse(value, out int year) && year >= 2010 && year <= 2020,
            "eyr" => int.TryParse(value, out int year) && year >= 2020 && year <= 2030,
            "hcl" => hairRegex.IsMatch(value),
            "ecl" => validEyeColors.Contains(value),
            "hgt" => ValidHeight(value),
            "pid" => pidRegex.IsMatch(value),
            "cid" => true,
            _ => false
        };
    });

    private bool ValidHeight(string value)
    {

        if (value.EndsWith("in") && int.TryParse(value.Replace("in", ""), out var num))
        {
            return num >= 59 && num <= 76;
        }
        else if (value.EndsWith("cm") && int.TryParse(value.Replace("cm", ""), out var numCm))
        {
            return numCm >= 150 && numCm <= 193;
        }

        return false;
    }

    private Regex hairRegex = new Regex("^#[0-9a-f]{6}$");
    private Regex pidRegex = new Regex("^[0-9]{9}$");
    private HashSet<string> validEyeColors = new() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
}