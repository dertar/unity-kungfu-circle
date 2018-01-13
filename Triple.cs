public class Triple<A,B,C>
{
    public A First
    {
        get; set;
    }

    public B Second
    {
        get; set;
    }

    public C Third
    {
        get; set;
    }

    public Triple ()
    {
    }

    public Triple (A a, B b, C c)
    {
        First = a;
        Second = b;
        Third = c;
    }
}
