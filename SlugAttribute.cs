[System.AttributeUsage(AttributeTargets.Class |
                       AttributeTargets.Struct)
]
public class SlugAttribute(int year, int day) : Attribute
{
    public int Year = year;
    public int Day = day;
}