using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

public class LongConverter : IYamlTypeConverter
{
    public bool Accepts(Type type)
    {
        return type == typeof(long);
    }

    public object ReadYaml(IParser parser, Type type)
    {
        var scalar = (Scalar)parser.Current;
        long value = long.Parse(scalar.Value);
        parser.MoveNext();
        return value;
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
        var l = (long)value;
        emitter.Emit(new Scalar(
            null,
            "!l",
            l.ToString(),
            ScalarStyle.Plain,
            false,
            false
        ));
    }
}