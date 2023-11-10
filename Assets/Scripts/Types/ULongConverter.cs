using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

public class ULongConverter : IYamlTypeConverter
{
    public bool Accepts(Type type)
    {
        return type == typeof(ulong);
    }

    public object ReadYaml(IParser parser, Type type)
    {
        var scalar = (Scalar)parser.Current;
        ulong value = ulong.Parse(scalar.Value);
        parser.MoveNext();
        return value;
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
        var ul = (ulong)value;
        emitter.Emit(new Scalar(
            null,
            "!ul",
            ul.ToString(),
            ScalarStyle.Plain,
            false,
            false
        ));
    }
}