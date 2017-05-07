using Newtonsoft.Json;
using System;
public class DeckCardTag
{

    #region public properties

    [JsonProperty(Required = Required.Always)]
    public String Name;

    [JsonProperty(Required = Required.Always)]
    public String Value;

    #endregion

    #region public methods

    public override string ToString()
    {
        String pStrTag = String.Format("[Name={0}, Value={1}]", Name, Value);
        return (pStrTag);
    }

    #endregion

}
