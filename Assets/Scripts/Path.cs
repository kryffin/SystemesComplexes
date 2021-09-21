public class Path
{
    public Node from;
    public Node to;
    public float weight;

    public override string ToString()
    {
        return from.name + " -> " + to.name + " (" + weight + ")";
    }

    public Path(Node from, Node to, float weight)
    {
        this.from = from;
        this.to = to;
        this.weight = weight;
    }

}
