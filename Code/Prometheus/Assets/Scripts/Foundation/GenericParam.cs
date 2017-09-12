public class XY<TYPE1, TYPE2>
{
    public TYPE1 x;
    public TYPE2 y;

    public XY() { }

    public XY(TYPE1 x, TYPE2 y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {

        return "(" + x.ToString() + "," + y.ToString() + ")";
    }
}

public class XYZ<T>
{
    public T x, y, z;

    public XYZ() { }

    public XYZ(T x, T y, T z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override string ToString()
    {
        return "(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + ")";
    }
}

public class XYZ<TYPE1, TYPE2, TYPE3>
{
    public TYPE1 x;
    public TYPE2 y;
    public TYPE3 z;

    public XYZ() { }

    public XYZ(TYPE1 x, TYPE2 y, TYPE3 z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override string ToString()
    {
        return "(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + ")";
    }
}

public class XYZW<T>
{
    public T x, y, z, w;

    public XYZW() { }

    public XYZW(T x, T y, T z, T w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public override string ToString()
    {
        return "(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + "," + w.ToString() + ")";
    }
}

public class XYZW<TYPE1, TYPE2, TYPE3, TYPE4>
{
    public TYPE1 x;
    public TYPE2 y;
    public TYPE3 z;
    public TYPE4 w;

    public XYZW() { }

    public XYZW(TYPE1 x, TYPE2 y, TYPE3 z, TYPE4 w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public override string ToString()
    {
        return "(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + "," + w.ToString() + ")";
    }
}

public class XYZWT<T>
{
    public T x, y, z, w, t;

    public XYZWT() { }

    public XYZWT(T x, T y, T z, T w, T t)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
        this.t = t;
    }

    public override string ToString()
    {
        return "(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + "," + w.ToString() + "," + t.ToString() + ")";
    }
}

public class XYZWT<TYPE1, TYPE2, TYPE3, TYPE4, TYPE5>
{
    public TYPE1 x;
    public TYPE2 y;
    public TYPE3 z;
    public TYPE4 w;
    public TYPE5 t;

    public XYZWT() { }

    public XYZWT(TYPE1 x, TYPE2 y, TYPE3 z, TYPE4 w, TYPE5 t)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
        this.t = t;
    }

    public override string ToString()
    {
        return "(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + "," + w.ToString() + "," + t.ToString() + ")";
    }
}

