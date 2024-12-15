// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Net;

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
Console.WriteLine($"Finding impl for {slug}");

if (!types.TryGetValue(slug, out var type))
{
    Console.Error.WriteLine($"Could not find an implementation with the slug {slug}");
    Environment.Exit(2);
}

var inputFile = Path.Combine("input", $"{SlugToPath(slug)}.txt");
var exampleFile = Path.Combine("examples", $"{SlugToPath(slug)}.txt");
if (!File.Exists(inputFile))
{
    Console.WriteLine($"for {slug}, {inputFile} does not exist, pulling");
    BuildFolderStructure(inputFile);
    using var client = BuildClient();
    var parts = slug.Split('/');
    var year = parts[0];
    var day = Convert.ToInt32(parts[1][1..]);
    var url = $"/{year}/day/{day}/input";
    Console.WriteLine($"Downloading from {url}");
    var res = await client.GetAsync(url);

    var content = await res.Content.ReadAsStringAsync();
    await File.WriteAllTextAsync(inputFile, content);

}

if (File.Exists(exampleFile))
{
    Console.WriteLine($"Example input for {slug} detected, trying that first");
    var exampleInput = await File.ReadAllLinesAsync(exampleFile);
    if (Activator.CreateInstance(type) is AsyncProblem example)
    {
        example.ExampleMode = true;
        var part1 = await example.RunPartOneAsync(exampleInput);
        Console.WriteLine($"Part 1: {part1}");
        var part2 = await example.RunPartTwoAsync(exampleInput);
        Console.WriteLine($"Part 2: {part2}");
    }

}

if (Activator.CreateInstance(type) is AsyncProblem problem)
{
    Console.WriteLine($"for {slug}, reading input {inputFile}");

    var input = await File.ReadAllLinesAsync(inputFile);
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


static string SlugToPath(string slug)
{
    return slug.Replace('/', Path.DirectorySeparatorChar);
}


static HttpClient BuildClient()
{
    var sessionCookie = Environment.GetEnvironmentVariable("AOC_SESSION_COOKIE");
    var baseAddress = new Uri("https://adventofcode.com");
    var cookieContainer = new CookieContainer();
    cookieContainer.Add(baseAddress, new Cookie("session", sessionCookie));
    var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
    return new HttpClient(handler) { BaseAddress = baseAddress };
}

static void BuildFolderStructure(string path)
{
    var dir = Path.GetDirectoryName(path);
    if (dir is not null && !Directory.Exists(dir))
    {
        Console.WriteLine($"creating directory {dir}");
        Directory.CreateDirectory(dir);
    }
}
