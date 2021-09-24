public class Path
{

    public Node from; //start node
    public Node to; //end node
    public float weight; //weight of the path

    public override string ToString()
    {
        return from.value + " -> " + to.value + " (" + weight + ")";
    }

    public Path(Node from, Node to, float weight)
    {
        this.from = from;
        this.to = to;
        this.weight = weight;
    }

}
