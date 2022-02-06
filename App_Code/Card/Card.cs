using System;

/// <summary>
/// Summary description for Card
/// </summary>
[Serializable]
public class Card
{

    private Int32 _id;
    /// <summary>
    /// ID (Int32)
    /// </summary>
    public Int32 ID
    {
        get { return this._id; }
        set { this._id = value; }
    }

    private String _color;
    /// <summary>
    /// Color (String)
    /// </summary>
    public String Color
    {
        get { return this._color; }
        set { this._color = value; }
    }

    private String _face;
    /// <summary>
    /// Face (String)
    /// </summary>
    public String Face
    {
        get { return this._face; }
        set { this._face = value; }
    }

    private String _suit;
    /// <summary>
    /// Suit (String)
    /// </summary>
    public String Suit
    {
        get { return this._suit; }
        set { this._suit = value; }
    }

    private Int32 _value;
    /// <summary>
    /// Value (Int32)
    /// </summary>
    public Int32 Value
    {
        get { return this._value; }
        set { this._value = value; }
    }





}
