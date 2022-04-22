namespace DesignPatterns.Behavioral.MediatorCustom;

public static class ChatMediator
{
    public class Person 
    {
        public string Name { get; set; }
        public ChatRoom Room { get; set; }
        private readonly List<string> _chatLog = new();

        public Person(string name)
        {
            Name = name;
        }

        public void Say(string msg)
        {
            Room.BroadCast(Name, msg);
        }

        public void PrivateMessage(string who, string msg)
        {
            Room.Message(Name, who, msg);
        }

        public void Receive(string sender, string msg)
        {
            string s = $"{sender}: '{msg}'";
            _chatLog.Add(s);
            Console.WriteLine($"[{Name}'s chat session] {s}");
        }
    }

    public class ChatRoom
    {
        private readonly List<Person> _people = new();

        public void Join(Person p)
        {
            string joinMsg = $"{p.Name} joins the chat";
            BroadCast("room", joinMsg);
            p.Room = this;
            _people.Add(p);
        }

        public void BroadCast(string source, string msg)
        {
            foreach (var p in _people)
                if (p.Name != source)
                    p.Receive(source, msg);
        }

        public void Message(string source, string destination, string msg)
        {
            _people.FirstOrDefault(p => p.Name == destination)?.Receive(source, msg);

        }
    }
    
    public static void Render()
    {
        var room = new ChatRoom();

        var john = new Person("John");
        var jane = new Person("Jane");

        room.Join(john);
        room.Join(jane);

        john.Say("Hi");
        jane.Say("oh, hey john");

        var simon = new Person("Simon");
        room.Join(simon);

        jane.PrivateMessage("Simon", "Glad you can join us!");
    }
}
