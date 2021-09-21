public class Node
{
    public string name;

    public override string ToString()
    {
        return name;
    }

    public override bool Equals(object obj) => this.Equals(obj as Node);

    public bool Equals(Node n)
    {
        if (n is null) return false;

        if (object.ReferenceEquals(this, n)) return true;

        if (this.GetType() != n.GetType()) return false;

        return name == n.name;
    }
    public static bool operator ==(Node obj1, Node obj2)
    {
        if (ReferenceEquals(obj1, obj2))
        {
            return true;
        }
        if (ReferenceEquals(obj1, null))
        {
            return false;
        }
        if (ReferenceEquals(obj2, null))
        {
            return false;
        }

        return obj1.Equals(obj2);
    }

    public static bool operator !=(Node obj1, Node obj2)
    {
        return !(obj1 == obj2);
    }

    public override int GetHashCode() => name.GetHashCode();

    public Node(string name)
    {
        this.name = name;
    }

}
