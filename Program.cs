// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

if (args.Length != 1)
{
    Console.Error.WriteLine("must supply exactly one param, slug of problem to run");
    Environment.Exit(1);
}

Dictionary<string, Type> types = AppDomain.CurrentDomain.GetAssemblies()
.SelectMany(s => s.GetTypes())
.Where(p => typeof(AsyncProblem).IsAssignableFrom(p) && p != typeof(AsyncProblem))
.Select(t => (slug: GetSlug(t), type: t))
.Where(tuple => tuple.slug != null)
.ToDictionary(t => t.slug ?? "", t => t.type);


Console.WriteLine($"Enumerated {types.Count} implementations");

var slug = args[0];
Console.WriteLine($"Creating an instance of {slug}");

if (!types.TryGetValue(slug, out var type))
{
    Console.Error.WriteLine($"Could not find an implementation with the slug {slug}");
    Environment.Exit(2);
}

if (File.Exists($"input/{slug}-example.txt"))
{
    Console.WriteLine($"Example input for {slug} detected, trying that first");
    var exampleInput = await File.ReadAllLinesAsync($"input/{slug}-example.txt");
    if (Activator.CreateInstance(type) is AsyncProblem example)
    {
        var part1 = await example.RunPartOneAsync(exampleInput);
        Console.WriteLine($"Part 1: {part1}");
        var part2 = await example.RunPartTwoAsync(exampleInput);
        Console.WriteLine($"Part 2: {part2}");
    }

}

if (Activator.CreateInstance(type) is AsyncProblem problem)
{
    Console.WriteLine($"Reading input for {slug}");

    var input = await File.ReadAllLinesAsync($"input/{slug}.txt");
    var sw = new Stopwatch();

    sw.Start();
    var part1 = await problem.RunPartOneAsync(input);
    sw.Stop();

    Console.WriteLine($"Part 1: {part1} ({sw.ElapsedMilliseconds} ms)");

    sw.Restart();
    var part2 = await problem.RunPartTwoAsync(input);
    sw.Stop();
    Console.WriteLine($"Part 2: {part2} ({sw.ElapsedMilliseconds} ms)");
}
else
{
    Console.Error.WriteLine("This shouldn't happen");
    Environment.Exit(-1);
}




static string? GetSlug(Type t)
{
    // Get instance of the attribute.
    var attr = Attribute.GetCustomAttribute(t, typeof(SlugAttribute)) as SlugAttribute;

    // if (attr == null)
    // {
    //     throw new Exception($"No Slug Attribute defined on {t.FullName}");
    // }
    // else if (string.IsNullOrEmpty(attr.Name))
    // {
    //     throw new Exception($"Invalid slug defined on {t.FullName}");
    // }

    return attr?.Name;
}


