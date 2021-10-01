public class Path
{

    public Node from; //start node
    public Node to; //end node
    public float weight; //weight of the path
    public bool direct; //whether the path connects 2 direct neighbors or not

    public override string ToString()
    {
        return from.value + " -> " + to.value + " (" + weight + ")";
    }

    public Path(Node from, Node to, float weight, bool direct)
    {
        this.from = from;
        this.to = to;
        this.weight = weight;
        this.direct = direct;
    }

}
