[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
public class ReferencableAttribute : System.Attribute
{
    public string Tag { get; private set; }

    public ReferencableAttribute(string tag)
    {
        Tag = tag;
    }
}