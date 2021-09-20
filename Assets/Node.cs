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

    public override int GetHashCode() => name.GetHashCode();

    public Node(string name)
    {
        this.name = name;
    }

}
