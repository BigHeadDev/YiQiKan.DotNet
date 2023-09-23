using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using YiQiKan.DotNet.Model.ResponseModel;

namespace YiQiKan.DotNet.Model.JsonConverters {
    internal class StringToActorsConverter : JsonConverter<Actor[]> {
        public override Actor[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            return JsonSerializer.Deserialize<Actor[]>(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, Actor[] value, JsonSerializerOptions options) {
            writer.WriteStringValue(JsonSerializer.Serialize(value));
        }
    }
}
