namespace CustomProgram
{
    public interface IBuyable
    {
        string name { get; set;}
        float purchasePrice { get; set;}
    }

    public interface ISellable
    {
        string name { get; set;}
        float sellPrice { get; set;}
    }
}
