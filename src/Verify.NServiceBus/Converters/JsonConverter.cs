using System;
using Newtonsoft.Json;

abstract class JsonConverter :
    Newtonsoft.Json.JsonConverter
{
    public sealed override object ReadJson(JsonReader reader, Type type, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}