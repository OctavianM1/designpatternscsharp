namespace DesignPatterns.SOLID;

public static class D
{
    public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }

    public class Person
    {
        public string Name { get; set; }
    }

    // low-level
    public class Relationships
    {
        private List<(Person, Relationship, Person)> _relations = new();

        public void AddParentAndChild(Person parent, Person child)
        {
            _relations.Add((parent, Relationship.Parent, child)); 
            _relations.Add((child, Relationship.Parent, parent));
        }

        public IReadOnlyList<(Person, Relationship, Person)> Relations => _relations;
    }

    public class BadResearch
    {
        private readonly Relationships _relationships;

        public BadResearch(Relationships relationships)
        {
            _relationships = relationships;
        }

        public void GetParentChildResearch(Person parent, Person child)
        {
            var parentChildRelation = _relationships.Relations.Where(x => x.Item1.Name == parent.Name
                                                                          && x.Item2 == Relationship.Parent
                                                                          && x.Item3.Name == child.Name);
            foreach (var r in parentChildRelation)
            {
                Console.WriteLine($"Parent - {r.Item1.Name} - Child {r.Item3.Name}");
            }
        }
    }

    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }

    // low-level
    public class BetterRelationships : IRelationshipBrowser
    {
        private List<(Person, Relationship, Person)> _relations = new();

        public void AddParentAndChild(Person parent, Person child)
        {
            _relations.Add((parent, Relationship.Parent, child));
            _relations.Add((child, Relationship.Parent, parent));
        }

        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            return _relations.Where(x => x.Item1.Name == name && x.Item2 == Relationship.Parent).Select(x => x.Item3);
        }
    }
    public class BetterResearch
    {
        private readonly IRelationshipBrowser _relationships;

        public BetterResearch(IRelationshipBrowser relationships)
        {
            _relationships = relationships;
            foreach (var item in _relationships.FindAllChildrenOf("John"))
            {
                Console.WriteLine($"Name - {item.Name}");
            }
        }
    }

    public static void Render()
    {
        var parent = new Person { Name = "John" };
        var child1 = new Person { Name = "Chris" };
        var child2 = new Person { Name = "Mary" };

        var relationships = new Relationships();
        relationships.AddParentAndChild(parent, child1);
        relationships.AddParentAndChild(parent, child2);

        var badResearch = new BadResearch(relationships);
        badResearch.GetParentChildResearch(parent, child1);

        var betterRelationships = new BetterRelationships();
        betterRelationships.AddParentAndChild(parent, child1);
        betterRelationships.AddParentAndChild(parent, child2);
        var betterResearch = new BetterResearch(betterRelationships);
    }
}
