
[Slug("2022/d13")]
public class Day202213 : SyncProblem
{
    public override string RunPartOneSync(string[] input)
    {

        var sum = 0;
        var pairs = 1;
        var inputIndex = 0;
        while (inputIndex < input.Length)
        {
            var lhs = Parse(input[inputIndex++]);
            var rhs = Parse(input[inputIndex++]);

            if (Compare(lhs, rhs) == -1)
            {
                sum += pairs;
            }

            inputIndex++; // Skip empty line
            pairs++;
        }

        return $"{sum}";
    }

    public override string RunPartTwoSync(string[] input)
    {
        var items = new List<Item>();
        var inputIndex = 0;

        // Parse input
        while (inputIndex < input.Length)
        {
            var inputLine = input[inputIndex++];
            if (inputLine.Length == 0)
            {
                continue;
            }

            items.Add(Parse(inputLine));
        }

        // Add divider packets
        var a = new ListItem(new ListItem(new IntItem(2)));
        var b = new ListItem(new ListItem(new IntItem(6)));

        items.Add(a);
        items.Add(b);

        // Sort list
        items.Sort(Compare);

        return $"{(items.IndexOf(a) + 1) * (items.IndexOf(b) + 1)}";
    }

    static Item Parse(ReadOnlySpan<char> input)
    {
        var items = new List<Item>();

        var inputIndex = 0;
        var inputEndIndex = input.Length;
        while (inputIndex < inputEndIndex)
        {
            var c = input[inputIndex];
            if (c == '[')
            {
                // Parse list
                var listStringIndex = inputIndex;
                var listStringLength = 0;
                int listNesting = 1;
                inputIndex++;
                while (listNesting > 0)
                {
                    c = input[inputIndex++];
                    listNesting = listNesting + (c switch
                    {
                        '[' => 1,
                        ']' => -1,
                        _ => 0
                    });
                    listStringLength++;

                }

                // Recursivly parse the contents of the list and store as a list item
                items.Add(Parse(input.Slice(listStringIndex + 1, listStringLength - 1)));

                inputIndex++; // Ignore comma
            }
            else
            {
                //Parse int
                int intStringIndex = inputIndex;
                var intStringLength = 0;

                // Loop until we find the next comma
                while (inputIndex < inputEndIndex && char.IsDigit(input[inputIndex++]))
                {
                    intStringLength++;
                }

                if (intStringLength > 0)
                {
                    // Add parsed int item
                    items.Add(new IntItem(int.Parse(input.Slice(intStringIndex, intStringLength))));
                }
            }

        }

        return new ListItem(items);
    }

    static int Compare(Item lhs, Item rhs)
    {
        if (lhs == rhs)
        {
            // Both are the same
            return 0;
        }

        if (lhs is IntItem lIntItem && rhs is IntItem rIntItem)
        {
            return lIntItem.Val.CompareTo(rIntItem.Val);
        }

        if (lhs is not ListItem lListItem)
        {
            // Convert to ListItem
            lListItem = new ListItem(lhs);
        }

        if (rhs is not ListItem rListItem)
        {
            // Convert to ListItem
            rListItem = new ListItem(rhs);
        }

        var index = 0;
        while (index < lListItem.Val.Count())
        {
            if (index >= rListItem.Val.Count())
            {
                // RHS ran out of items first, wrong order
                return 1;
            }

            // Compare next item in list
            var comparison = Compare(lListItem.Val[index], rListItem.Val[index]);
            if (comparison == 0)
            {
                // both are equal keep checking
                index++;
                continue;
            }

            // We determined the order was either correct / incorrect
            return comparison;
        }

        if (lListItem.Val.Count() == rListItem.Val.Count())
        {
            // if we could not determine the order and both lists are the same length
            return 0;
        }

        // The lists are in the correct order
        return -1;
    }

    abstract class Item
    {
    }

    class IntItem : Item
    {
        public IntItem(int val) { Val = val; }
        public int Val { get; init; }
    }
    class ListItem : Item
    {
        public ListItem() { Val = new List<Item>(); }
        public ListItem(Item item) { Val = new List<Item> { item }; }
        public ListItem(IEnumerable<Item> items) { Val = items.ToList(); }
        public List<Item> Val { get; init; }
    }
}