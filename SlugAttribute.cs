[System.AttributeUsage(AttributeTargets.Class |
                       AttributeTargets.Struct)
]
public class SlugAttribute(string name) : Attribute
{
    public string Name = name;
}